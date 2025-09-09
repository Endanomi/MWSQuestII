using System.Collections.Generic;
using Services.Firewall.Policies;

namespace Services.Firewall
{
    // iptables の実行時オプションと設定を管理するクラス
    public class Config
    {
        // 基本設定
        public bool Numeric { get; set; } = false;          // -n: 数値出力
        public bool Verbose { get; set; } = false;          // -v: 詳細モード
        public bool Exact { get; set; } = false;            // -x: 正確な値表示
        public bool LineNumbers { get; set; } = false;      // --line-numbers: 行番号表示
        public int Wait { get; set; } = 0;                  // -w: ロック待機時間
        
        // プロトコルバージョン
        public bool IPv4Only { get; set; } = false;         // -4: IPv4のみ
        public bool IPv6Only { get; set; } = false;         // -6: IPv6のみ
        
        // モジュール設定
        public string ModProbe { get; set; } = string.Empty; // --modprobe: モジュールプローブコマンド
        
        // ポリシー管理
        public PolicyManager PolicyManager { get; private set; }
        
        // 出力フォーマット設定
        public bool ShowPacketCounts { get; set; } = true;
        public bool ShowByteCounts { get; set; } = true;
        public bool ShowTargetDetails { get; set; } = true;
        
        // デバッグ設定
        public bool DebugMode { get; set; } = true;
        public bool LogParsing { get; set; } = true;
        public bool LogRuleChanges { get; set; } = true;
        
        // パース結果を保持するプロパティ（新規追加）
        public ParsedCommandInfo ParsedCommand { get; private set; }
        
        public Config()
        {
            PolicyManager = new PolicyManager();
            ParsedCommand = new ParsedCommandInfo();
        }
        
        // パース結果をConfigに設定するメソッド
        public void SetParsedCommand(string originalCommand)
        {
            ParsedCommand = new ParsedCommandInfo
            {
                OriginalCommand = originalCommand
            };
        }
        
        // ParsedCommandInfoからFilterRuleを作成
        public FilterRule CreateFilterRule()
        {
            var rule = new FilterRule();
            
            // 基本情報のコピー
            rule.Table = ParsedCommand.Table;
            rule.Command = ParsedCommand.Command;
            rule.Chain = ParsedCommand.Chain;
            rule.RuleNumber = ParsedCommand.RuleNumber;
            
            // ネットワーク情報のコピー
            rule.Protocol = string.IsNullOrEmpty(ParsedCommand.Protocol) ? "any" : ParsedCommand.Protocol;
            rule.SrcIp = string.IsNullOrEmpty(ParsedCommand.SrcIp) ? "any" : ParsedCommand.SrcIp;
            rule.DstIp = string.IsNullOrEmpty(ParsedCommand.DstIp) ? "any" : ParsedCommand.DstIp;
            rule.SrcPort = string.IsNullOrEmpty(ParsedCommand.SrcPort) ? "any" : ParsedCommand.SrcPort;
            rule.DstPort = string.IsNullOrEmpty(ParsedCommand.DstPort) ? "any" : ParsedCommand.DstPort;
            
            // インターフェース情報のコピー
            rule.InInterface = ParsedCommand.InInterface;
            rule.OutInterface = ParsedCommand.OutInterface;
            
            // ターゲット情報のコピー
            rule.Jump = ParsedCommand.Jump;
            rule.Goto = ParsedCommand.Goto;
            
            // マッチオプションのコピー
            rule.Matches = new List<string>(ParsedCommand.Matches);
            
            // フラグのコピー（設定されたフラグも含める）
            rule.Fragment = ParsedCommand.Fragment;
            rule.Numeric = ParsedCommand.Numeric || this.Numeric;
            rule.Verbose = ParsedCommand.Verbose || this.Verbose;
            rule.Exact = ParsedCommand.Exact || this.Exact;
            rule.LineNumbers = ParsedCommand.LineNumbers || this.LineNumbers;
            rule.IPv4 = ParsedCommand.IPv4 || this.IPv4Only;
            rule.IPv6 = ParsedCommand.IPv6 || this.IPv6Only;
            
            // その他の情報のコピー
            rule.Wait = ParsedCommand.Wait ?? this.Wait;
            rule.PacketCount = ParsedCommand.PacketCount;
            rule.ByteCount = ParsedCommand.ByteCount;
            rule.ModProbe = !string.IsNullOrEmpty(ParsedCommand.ModProbe) ? ParsedCommand.ModProbe : this.ModProbe;
            
            // 否定フラグのコピー
            rule.NotProtocol = ParsedCommand.NotProtocol;
            rule.NotSource = ParsedCommand.NotSource;
            rule.NotDestination = ParsedCommand.NotDestination;
            rule.NotInInterface = ParsedCommand.NotInInterface;
            rule.NotOutInterface = ParsedCommand.NotOutInterface;
            rule.NotFragment = ParsedCommand.NotFragment;
            
            // オプションのコピー
            rule.Options = new Dictionary<string, string>(ParsedCommand.Options);
            
            return rule;
        }
        
        // パース結果を保持する内部クラス
        public class ParsedCommandInfo
        {
            // 基本コマンド情報
            public string Table { get; set; } = "filter";
            public string Command { get; set; } = string.Empty;
            public string Chain { get; set; } = string.Empty;
            public int? RuleNumber { get; set; } = null;
            
            // ネットワーク関連
            public string Protocol { get; set; } = string.Empty;
            public string SrcIp { get; set; } = string.Empty;
            public string DstIp { get; set; } = string.Empty;
            public string SrcPort { get; set; } = string.Empty;
            public string DstPort { get; set; } = string.Empty;
            
            // インターフェース
            public string InInterface { get; set; } = string.Empty;
            public string OutInterface { get; set; } = string.Empty;
            
            // ターゲット/ジャンプ
            public string Jump { get; set; } = string.Empty;
            public string Goto { get; set; } = string.Empty;
            
            // マッチオプション
            public List<string> Matches { get; set; } = new();
            
            // フラグ類
            public bool Fragment { get; set; } = false;
            public bool Numeric { get; set; } = false;
            public bool Verbose { get; set; } = false;
            public bool Exact { get; set; } = false;
            public bool LineNumbers { get; set; } = false;
            public bool IPv4 { get; set; } = false;
            public bool IPv6 { get; set; } = false;
            
            // 待機時間とカウンター
            public int? Wait { get; set; } = null;
            public long? PacketCount { get; set; } = null;
            public long? ByteCount { get; set; } = null;
            
            // その他のオプション
            public string ModProbe { get; set; } = string.Empty;
            public Dictionary<string, string> Options { get; set; } = new();
            
            // 否定フラグ（[!]で示されるオプション）
            public bool NotProtocol { get; set; } = false;
            public bool NotSource { get; set; } = false;
            public bool NotDestination { get; set; } = false;
            public bool NotInInterface { get; set; } = false;
            public bool NotOutInterface { get; set; } = false;
            public bool NotFragment { get; set; } = false;
            
            // 元のコマンド文字列
            public string OriginalCommand { get; set; } = string.Empty;
            
            public ParsedCommandInfo()
            {
                Matches = new List<string>();
                Options = new Dictionary<string, string>();
            }
        }
        
        // チェーンがテーブルに存在するかチェック
        public bool IsValidChain(string table, string chain)
        {
            return PolicyManager.IsValidChain(table, chain);
        }
        
        // ターゲットがテーブルで有効かチェック
        public bool IsValidTarget(string table, string target)
        {
            return PolicyManager.IsValidTarget(table, target);
        }
        
        // カスタムチェーンを追加
        public bool AddCustomChain(string table, string chain)
        {
            return PolicyManager.AddCustomChain(table, chain);
        }
        
        // カスタムチェーンを削除
        public bool RemoveCustomChain(string table, string chain)
        {
            return PolicyManager.RemoveCustomChain(table, chain);
        }
        
        // ポリシーを設定
        public bool SetPolicy(string table, string chain, string policy)
        {
            return PolicyManager.SetPolicy(table, chain, policy);
        }
        
        // ポリシーを取得
        public string GetPolicy(string table, string chain)
        {
            return PolicyManager.GetPolicy(table, chain);
        }
        
        // 全テーブル名を取得
        public IEnumerable<string> GetTableNames()
        {
            return PolicyManager.GetTableNames();
        }
        
        // テーブルの全チェーン名を取得（ビルトイン + カスタム）
        public IEnumerable<string> GetChainNames(string table)
        {
            return PolicyManager.GetAllChains(table);
        }
        
        // テーブルの存在チェック
        public bool IsValidTable(string table)
        {
            return PolicyManager.IsValidTable(table);
        }
        
        // カスタムチェーン統計取得
        public Dictionary<string, int> GetCustomChainCounts()
        {
            return PolicyManager.GetCustomChainCounts();
        }
        
        // 全カスタムチェーンをクリア
        public void ClearAllCustomChains()
        {
            PolicyManager.ClearAllCustomChains();
        }
        
        // 指定テーブルのカスタムチェーンをクリア
        public bool ClearCustomChains(string table)
        {
            return PolicyManager.ClearCustomChains(table);
        }
        
        // 設定のリセット
        public void Reset()
        {
            Numeric = false;
            Verbose = false;
            Exact = false;
            LineNumbers = false;
            Wait = 0;
            IPv4Only = false;
            IPv6Only = false;
            ModProbe = string.Empty;
            ShowPacketCounts = true;
            ShowByteCounts = true;
            ShowTargetDetails = true;
            DebugMode = true;
            LogParsing = true;
            LogRuleChanges = true;
            
            // ポリシーマネージャーを再初期化
            PolicyManager = new PolicyManager();
        }
        
        // 設定の複製
        public Config Clone()
        {
            var clone = new Config
            {
                Numeric = this.Numeric,
                Verbose = this.Verbose,
                Exact = this.Exact,
                LineNumbers = this.LineNumbers,
                Wait = this.Wait,
                IPv4Only = this.IPv4Only,
                IPv6Only = this.IPv6Only,
                ModProbe = this.ModProbe,
                ShowPacketCounts = this.ShowPacketCounts,
                ShowByteCounts = this.ShowByteCounts,
                ShowTargetDetails = this.ShowTargetDetails,
                DebugMode = this.DebugMode,
                LogParsing = this.LogParsing,
                LogRuleChanges = this.LogRuleChanges
            };
            
            // カスタムチェーンをコピー
            foreach (var tableName in this.PolicyManager.GetTableNames())
            {
                var policy = this.PolicyManager.GetTablePolicy(tableName);
                var clonePolicy = clone.PolicyManager.GetTablePolicy(tableName);
                
                foreach (var chain in policy.CustomChains)
                {
                    clonePolicy.CustomChains.Add(chain);
                }
            }
            
            return clone;
        }
    }
}
