using System.Collections.Generic;

namespace Services.IDS
{
    public class FilterRule
    {
        public string Action { get; set; }
        public string Protocol { get; set; }
        public string SrcIp { get; set; }
        public string SrcPort { get; set; }
        public string Direction { get; set; }
        public string DstIp { get; set; }
        public string DstPort { get; set; }
        public Dictionary<string, string> Options { get; set; } = new();
    }
}