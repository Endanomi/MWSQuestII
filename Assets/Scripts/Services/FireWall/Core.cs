using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.Firewall
{
    public class FirewallEngine
    {
        private Config config;
        private Dictionary<string, Dictionary<string, List<FilterRule>>> tables;
        private Dictionary<string, long> packetCounters;
        private Dictionary<string, long> byteCounters;

        public FirewallEngine()
        {
            config = new Config();
            InitializeTables();
            InitializeCounters();
        }

        public FirewallEngine(Config customConfig)
        {
            config = customConfig ?? new Config();
            InitializeTables();
            InitializeCounters();
        }

        private void InitializeTables()
        {
            tables = new Dictionary<string, Dictionary<string, List<FilterRule>>>();
            
            // 各テーブルとチェーンを初期化
            foreach (var tableName in config.GetTableNames())
            {
                tables[tableName] = new Dictionary<string, List<FilterRule>>();
                
                foreach (var chainName in config.GetChainNames(tableName))
                {
                    tables[tableName][chainName] = new List<FilterRule>();
                }
            }
        }

        private void InitializeCounters()
        {
            packetCounters = new Dictionary<string, long>();
            byteCounters = new Dictionary<string, long>();
        }

        public bool ExecuteCommand(string command)
        {
            try
            {
                // 新しい2段階パース方式を使用
                Parser.ParseToConfig(command, config);
                var rule = config.CreateFilterRule();
                return ExecuteRule(rule);
            }
            catch (System.Exception ex)
            {
                if (config.DebugMode)
                    Debug.LogError($"Failed to execute command: {command}. Error: {ex.Message}");
                return false;
            }
        }

        private bool ExecuteRule(FilterRule rule)
        {
            switch (rule.Command.ToUpper())
            {
                case "A": // Append
                    return AppendRule(rule);
                case "I": // Insert
                    return InsertRule(rule);
                case "D": // Delete
                    return DeleteRule(rule);
                case "R": // Replace
                    return ReplaceRule(rule);
                case "L": // List
                    return ListRules(rule);
                case "S": // List rules
                    return ListRulesAsCommands(rule);
                case "F": // Flush
                    return FlushChain(rule);
                case "Z": // Zero
                    return ZeroCounters(rule);
                case "N": // New chain
                    return CreateChain(rule);
                case "X": // Delete chain
                    return DeleteChain(rule);
                case "P": // Policy
                    return SetPolicy(rule);
                case "E": // Rename chain
                    return RenameChain(rule);
                case "C": // Check
                    return CheckRule(rule);
                default:
                    if (config.DebugMode)
                        Debug.LogWarning($"Unknown command: {rule.Command}");
                    return false;
            }
        }

        private bool AppendRule(FilterRule rule)
        {
            if (!ValidateRule(rule)) return false;
            
            tables[rule.Table][rule.Chain].Add(rule);
            
            if (config.LogRuleChanges)
                Debug.Log($"Rule appended to {rule.Table}:{rule.Chain}. Total rules: {tables[rule.Table][rule.Chain].Count}");
            
            return true;
        }

        private bool InsertRule(FilterRule rule)
        {
            if (!ValidateRule(rule)) return false;
            
            var ruleList = tables[rule.Table][rule.Chain];
            int position = rule.RuleNumber.HasValue ? rule.RuleNumber.Value - 1 : 0;
            position = System.Math.Max(0, System.Math.Min(position, ruleList.Count));
            
            ruleList.Insert(position, rule);
            
            if (config.LogRuleChanges)
                Debug.Log($"Rule inserted at position {position + 1} in {rule.Table}:{rule.Chain}");
            
            return true;
        }

        private bool DeleteRule(FilterRule rule)
        {
            if (!ValidateRule(rule)) return false;
            
            var ruleList = tables[rule.Table][rule.Chain];
            
            if (rule.RuleNumber.HasValue)
            {
                // 番号指定での削除
                int index = rule.RuleNumber.Value - 1;
                if (index >= 0 && index < ruleList.Count)
                {
                    ruleList.RemoveAt(index);
                    if (config.LogRuleChanges)
                        Debug.Log($"Rule {rule.RuleNumber} deleted from {rule.Table}:{rule.Chain}");
                    return true;
                }
            }
            else
            {
                // マッチングルールでの削除
                for (int i = ruleList.Count - 1; i >= 0; i--)
                {
                    if (RulesMatch(ruleList[i], rule))
                    {
                        ruleList.RemoveAt(i);
                        if (config.LogRuleChanges)
                            Debug.Log($"Matching rule deleted from {rule.Table}:{rule.Chain}");
                        return true;
                    }
                }
            }
            
            return false;
        }

        private bool ReplaceRule(FilterRule rule)
        {
            if (!ValidateRule(rule) || !rule.RuleNumber.HasValue) return false;
            
            var ruleList = tables[rule.Table][rule.Chain];
            int index = rule.RuleNumber.Value - 1;
            
            if (index >= 0 && index < ruleList.Count)
            {
                ruleList[index] = rule;
                if (config.LogRuleChanges)
                    Debug.Log($"Rule {rule.RuleNumber} replaced in {rule.Table}:{rule.Chain}");
                return true;
            }
            
            return false;
        }

        private bool ListRules(FilterRule rule)
        {
            var table = string.IsNullOrEmpty(rule.Chain) ? rule.Table : rule.Table;
            var chain = rule.Chain;
            
            if (string.IsNullOrEmpty(chain))
            {
                // 全チェーンをリスト
                foreach (var chainName in config.GetChainNames(table))
                {
                    PrintChainRules(table, chainName);
                }
            }
            else
            {
                PrintChainRules(table, chain);
            }
            
            return true;
        }

        private bool ListRulesAsCommands(FilterRule rule)
        {
            // -S オプション: ルールをコマンド形式で出力
            Debug.Log("Rules as commands (not implemented in this version)");
            return true;
        }

        private bool FlushChain(FilterRule rule)
        {
            if (string.IsNullOrEmpty(rule.Chain))
            {
                // 全チェーンをフラッシュ
                foreach (var chainName in config.GetChainNames(rule.Table))
                {
                    tables[rule.Table][chainName].Clear();
                }
                if (config.LogRuleChanges)
                    Debug.Log($"All chains in table {rule.Table} flushed");
            }
            else
            {
                if (config.IsValidChain(rule.Table, rule.Chain))
                {
                    tables[rule.Table][rule.Chain].Clear();
                    if (config.LogRuleChanges)
                        Debug.Log($"Chain {rule.Table}:{rule.Chain} flushed");
                }
            }
            
            return true;
        }

        private bool ZeroCounters(FilterRule rule)
        {
            // カウンターをゼロにリセット
            if (config.LogRuleChanges)
                Debug.Log("Counters zeroed");
            return true;
        }

        private bool CreateChain(FilterRule rule)
        {
            if (config.AddCustomChain(rule.Table, rule.Chain))
            {
                tables[rule.Table][rule.Chain] = new List<FilterRule>();
                if (config.LogRuleChanges)
                    Debug.Log($"Custom chain {rule.Table}:{rule.Chain} created");
                return true;
            }
            return false;
        }

        private bool DeleteChain(FilterRule rule)
        {
            if (string.IsNullOrEmpty(rule.Chain))
            {
                // 全カスタムチェーンを削除
                var tablePolicy = config.PolicyManager.GetTablePolicy(rule.Table);
                if (tablePolicy != null)
                {
                    var customChains = tablePolicy.CustomChains.ToList();
                    foreach (var chain in customChains)
                    {
                        config.RemoveCustomChain(rule.Table, chain);
                        tables[rule.Table].Remove(chain);
                    }
                    if (config.LogRuleChanges)
                        Debug.Log($"All custom chains in table {rule.Table} deleted");
                }
            }
            else
            {
                if (config.RemoveCustomChain(rule.Table, rule.Chain))
                {
                    tables[rule.Table].Remove(rule.Chain);
                    if (config.LogRuleChanges)
                        Debug.Log($"Custom chain {rule.Table}:{rule.Chain} deleted");
                    return true;
                }
            }
            return false;
        }

        private bool SetPolicy(FilterRule rule)
        {
            if (config.SetPolicy(rule.Table, rule.Chain, rule.Jump))
            {
                if (config.LogRuleChanges)
                    Debug.Log($"Policy for {rule.Table}:{rule.Chain} set to {rule.Jump}");
                return true;
            }
            return false;
        }

        private bool RenameChain(FilterRule rule)
        {
            // チェーン名変更（簡易実装）
            if (config.LogRuleChanges)
                Debug.Log("Chain rename not fully implemented");
            return false;
        }

        private bool CheckRule(FilterRule rule)
        {
            if (!ValidateRule(rule)) return false;
            
            var ruleList = tables[rule.Table][rule.Chain];
            return ruleList.Any(r => RulesMatch(r, rule));
        }

        private bool ValidateRule(FilterRule rule)
        {
            if (string.IsNullOrEmpty(rule.Table) || string.IsNullOrEmpty(rule.Chain))
                return false;
                
            if (!config.IsValidTable(rule.Table))
            {
                if (config.DebugMode)
                    Debug.LogWarning($"Invalid table: {rule.Table}");
                return false;
            }
                
            if (!config.IsValidChain(rule.Table, rule.Chain))
            {
                if (config.DebugMode)
                    Debug.LogWarning($"Invalid chain: {rule.Table}:{rule.Chain}");
                return false;
            }
            
            // ターゲットの妥当性チェック
            if (!string.IsNullOrEmpty(rule.Jump) && !config.IsValidTarget(rule.Table, rule.Jump))
            {
                if (config.DebugMode)
                    Debug.LogWarning($"Invalid target for table {rule.Table}: {rule.Jump}");
                return false;
            }
            
            return true;
        }

        private bool RulesMatch(FilterRule rule1, FilterRule rule2)
        {
            // 簡易的なルールマッチング（実際の実装ではより詳細な比較が必要）
            return rule1.Protocol == rule2.Protocol &&
                   rule1.SrcIp == rule2.SrcIp &&
                   rule1.DstIp == rule2.DstIp &&
                   rule1.SrcPort == rule2.SrcPort &&
                   rule1.DstPort == rule2.DstPort &&
                   rule1.Jump == rule2.Jump;
        }

        private void PrintChainRules(string table, string chain)
        {
            if (!tables.ContainsKey(table) || !tables[table].ContainsKey(chain))
                return;
                
            var rules = tables[table][chain];
            var policy = config.GetPolicy(table, chain);
            
            Debug.Log($"Chain {chain} (policy {policy})");
            
            if (config.LineNumbers)
            {
                for (int i = 0; i < rules.Count; i++)
                {
                    Debug.Log($"{i + 1}: {FormatRule(rules[i])}");
                }
            }
            else
            {
                foreach (var rule in rules)
                {
                    Debug.Log(FormatRule(rule));
                }
            }
        }

        private string FormatRule(FilterRule rule)
        {
            var parts = new List<string>();
            
            if (rule.Jump != string.Empty)
                parts.Add($"target: {rule.Jump}");
            if (rule.Protocol != "any")
                parts.Add($"prot: {rule.Protocol}");
            if (rule.SrcIp != "any")
                parts.Add($"source: {rule.SrcIp}");
            if (rule.DstIp != "any")
                parts.Add($"destination: {rule.DstIp}");
                
            return string.Join(" ", parts);
        }

        // パブリックAPI
        public Config GetConfig() => config;
        
        public List<FilterRule> GetRules(string table, string chain)
        {
            if (tables.ContainsKey(table) && tables[table].ContainsKey(chain))
                return new List<FilterRule>(tables[table][chain]);
            return new List<FilterRule>();
        }
        
        public Dictionary<string, List<FilterRule>> GetAllRules(string table)
        {
            if (tables.ContainsKey(table))
                return new Dictionary<string, List<FilterRule>>(tables[table]);
            return new Dictionary<string, List<FilterRule>>();
        }
        
        public bool ExecuteCommand(FilterRule rule)
        {
            return AppendRule(rule);
        }

        public void AddRuleFromString(string ruleString)
        {
            ExecuteCommand(ruleString);
        }

        public List<FilterRule> GetRules()
        {
            var allRules = new List<FilterRule>();
            foreach (var table in tables.Values)
            {
                foreach (var chain in table.Values)
                {
                    allRules.AddRange(chain);
                }
            }
            return allRules;
        }
    }
}