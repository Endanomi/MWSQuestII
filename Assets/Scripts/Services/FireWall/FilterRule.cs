using System.Collections.Generic;


namespace Services.Firewall
{
    // iptables のルールを保持するクラス
    public class FilterRule
    {
        public string Table { get; set; } = "filter"; // デフォルトは filter
        public string Command { get; set; } = string.Empty;
        public string Chain { get; set; } = string.Empty;
        public string Protocol { get; set; } = "any";
        public string SrcIp { get; set; } = "any";
        public string DstIp { get; set; } = "any";
        public string SrcPort { get; set; } = "any";
        public string DstPort { get; set; } = "any";
        public string Jump { get; set; } = string.Empty;
        public Dictionary<string, string> Options { get; set; } = new();
    }
}