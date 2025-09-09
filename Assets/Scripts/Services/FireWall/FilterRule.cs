using System.Collections.Generic;


namespace Services.Firewall
{
    // iptables のルールを保持するクラス
    public class FilterRule
    {
        // 基本プロパティ
        public string Table { get; set; } = "filter"; // デフォルトは filter
        public string Command { get; set; } = string.Empty;
        public string Chain { get; set; } = string.Empty;
        public int? RuleNumber { get; set; } = null; // -I, -R, -D で使用される番号
        
        // ネットワーク関連
        public string Protocol { get; set; } = "any";
        public string SrcIp { get; set; } = "any";
        public string DstIp { get; set; } = "any";
        public string SrcPort { get; set; } = "any";
        public string DstPort { get; set; } = "any";
        
        // インターフェース
        public string InInterface { get; set; } = string.Empty;
        public string OutInterface { get; set; } = string.Empty;
        
        // ターゲット/ジャンプ
        public string Jump { get; set; } = string.Empty;
        public string Goto { get; set; } = string.Empty;
        
        // マッチオプション
        public List<string> Matches { get; set; } = new();
        
        // その他のフラグ
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
    }
}