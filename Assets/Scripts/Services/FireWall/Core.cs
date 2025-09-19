using System.Collections.Generic;
using UnityEngine;

namespace Services.Firewall
{
    public class FirewallEngine
    {
        private List<FilterRule> filterRules = new List<FilterRule>();
        private bool isEnabled = false;
        private string defaultIncomingPolicy = "deny";
        private string defaultOutgoingPolicy = "allow";

        // ルールを追加
        public void AddRule(FilterRule rule)
        {
            // 優先度を自動設定
            if (rule.Priority == 0)
            {
                rule.Priority = filterRules.Count + 1;
            }
            
            filterRules.Add(rule);
            
            // ルールが追加されたら自動的にファイアウォールを有効化（UFWの動作に合わせる）
            if (!isEnabled)
            {
                isEnabled = true;
                Debug.Log("Firewall automatically enabled due to rule addition");
            }
            
            Debug.Log($"Firewall rule added: {rule}. Total rules: {filterRules.Count}");
        }

        // 文字列からルールを追加
        public void AddRuleFromString(string ruleString)
        {
            var filterRule = Parser.Parse(ruleString);
            AddRule(filterRule);
        }

        // UFWコマンドを実行（FirewallEmulatorから移動）
        public string ExecuteCommand(string command)
        {
            var trimmedCommand = command.Trim().ToLower();
            
            // 基本コマンドの処理
            if (trimmedCommand.StartsWith("ufw enable"))
            {
                Enable();
                return "Firewall is active and enabled on system startup";
            }
            else if (trimmedCommand.StartsWith("ufw disable"))
            {
                Disable();
                return "Firewall stopped and disabled on system startup";
            }
            else if (trimmedCommand.StartsWith("ufw reset"))
            {
                Reset();
                return "Resetting all rules to installed defaults";
            }
            else if (trimmedCommand.StartsWith("ufw status"))
            {
                var verbose = trimmedCommand.Contains("verbose");
                return ShowStatus(verbose);
            }
            else if (trimmedCommand.StartsWith("ufw default"))
            {
                ParseDefaultCommand(command);
                return "Default policy updated";
            }
            else if (trimmedCommand.StartsWith("ufw delete"))
            {
                ParseDeleteCommand(command);
                return "Rule deleted";
            }
            else if (trimmedCommand.Contains("allow") || trimmedCommand.Contains("deny") || 
                     trimmedCommand.Contains("reject") || trimmedCommand.Contains("limit"))
            {
                // ルール追加コマンド
                AddRuleFromString(command);
                return "Rule added";
            }
            else if (trimmedCommand.Contains("--help") || trimmedCommand.Contains("-h"))
            {
                return GetHelpText();
            }
            else
            {
                Debug.LogWarning($"Unknown UFW command: {command}");
                return $"ERROR: Invalid syntax: {command}";
            }
        }
        
        // デフォルトポリシー設定コマンドをパース
        private void ParseDefaultCommand(string command)
        {
            var parts = command.Split(' ');
            if (parts.Length >= 3)
            {
                var direction = parts[2];
                var policy = parts.Length > 3 ? parts[3] : "deny";
                SetDefaultPolicy(direction, policy);
            }
        }
        
        // 削除コマンドをパース
        private void ParseDeleteCommand(string command)
        {
            var parts = command.Split(' ');
            if (parts.Length >= 3)
            {
                if (int.TryParse(parts[2], out int ruleNumber))
                {
                    DeleteRule(ruleNumber);
                }
                else
                {
                    Debug.LogWarning("Invalid rule number for delete command");
                }
            }
        }

        // ルールを削除
        public bool DeleteRule(int ruleNumber)
        {
            if (ruleNumber > 0 && ruleNumber <= filterRules.Count)
            {
                var removedRule = filterRules[ruleNumber - 1];
                filterRules.RemoveAt(ruleNumber - 1);
                
                // 優先度を再計算
                for (int i = 0; i < filterRules.Count; i++)
                {
                    filterRules[i].Priority = i + 1;
                }
                
                Debug.Log($"Firewall rule deleted: {removedRule}");
                return true;
            }
            return false;
        }

        // ファイアウォールを有効化
        public void Enable()
        {
            isEnabled = true;
            Debug.Log("Firewall enabled");
        }

        // ファイアウォールを無効化
        public void Disable()
        {
            isEnabled = false;
            Debug.Log("Firewall disabled");
        }

        // ファイアウォールのリセット
        public void Reset()
        {
            filterRules.Clear();
            isEnabled = false;
            defaultIncomingPolicy = "deny";
            defaultOutgoingPolicy = "allow";
            Debug.Log("Firewall reset to defaults");
        }

        // ステータスを表示
        public string ShowStatus(bool verbose = false)
        {
            var status = isEnabled ? "active" : "inactive";
            var result = $"Status: {status}\n";
            
            if (verbose)
            {
                result += $"Default: {defaultIncomingPolicy} (incoming), {defaultOutgoingPolicy} (outgoing)\n";
            }
            
            if (filterRules.Count > 0)
            {
                result += $"Rules ({filterRules.Count}):\n";
                for (int i = 0; i < filterRules.Count; i++)
                {
                    var rule = filterRules[i];
                    result += $"[{i + 1}] {rule}\n";
                }
            }
            else
            {
                result += "No rules defined\n";
            }
            
            Debug.Log(result);
            return result;
        }

        // ヘルプテキストを取得
        private string GetHelpText()
        {
            return @"usage: ufw [--version] [--help] [--dry-run] [--force] [--verbose]

These are common UFW commands used in various situations:

 status:                     show firewall status
 enable:                     enable the firewall
 disable:                    disable the firewall
 reset:                      reset the firewall to default
 reload:                     reload the firewall

 allow RULE:                 add allow rule
 deny RULE:                  add deny rule
 reject RULE:                add reject rule
 limit RULE:                 add limit rule
 delete RULE|NUM:            delete RULE or NUM
 insert NUM RULE:            insert RULE at NUM

 default POLICY:             set default POLICY";
        }

        // 全ルールを取得
        public List<FilterRule> GetRules()
        {
            return new List<FilterRule>(filterRules);
        }

        // 有効状態を取得
        public bool IsEnabled()
        {
            return isEnabled;
        }

        // デフォルトポリシーを設定
        public void SetDefaultPolicy(string direction, string policy)
        {
            switch (direction.ToLower())
            {
                case "incoming":
                case "in":
                    defaultIncomingPolicy = policy.ToLower();
                    break;
                case "outgoing":
                case "out":
                    defaultOutgoingPolicy = policy.ToLower();
                    break;
            }
            Debug.Log($"Default {direction} policy set to {policy}");
        }
    }
}
