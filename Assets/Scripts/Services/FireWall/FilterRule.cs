using System.Collections.Generic;

namespace Services.Firewall
{
    // UFWスタイルのファイアウォールルール
    public class FilterRule
    {
        // 基本的なアクション
        public string Action { get; set; } = "allow"; // allow, deny, reject, limit
        
        // 方向
        public string Direction { get; set; } = "in"; // in, out
        
        // プロトコル
        public string Protocol { get; set; } = "any"; // tcp, udp, any
        
        // 送信元情報
        public string FromIp { get; set; } = "any";
        public string FromPort { get; set; } = "any";
        
        // 宛先情報
        public string ToIp { get; set; } = "any";
        public string ToPort { get; set; } = "any";
        
        // インターフェース
        public string Interface { get; set; } = "any";
        
        // アプリケーション
        public string App { get; set; } = "";
        
        // ログ記録
        public bool Log { get; set; } = false;
        
        // ルールの説明
        public string Comment { get; set; } = "";
        
        // 優先度 (UFWのルール番号)
        public int Priority { get; set; } = 0;
        
        // 追加オプション
        public Dictionary<string, string> Options { get; set; } = new();
        
        // ルールの文字列表現を取得
        public override string ToString()
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrEmpty(Comment))
                parts.Add($"# {Comment}");
                
            parts.Add(Action);
            parts.Add(Direction);
            
            if (Protocol != "any")
                parts.Add($"proto {Protocol}");
                
            if (FromIp != "any" || FromPort != "any")
            {
                var from = FromIp == "any" ? "anywhere" : FromIp;
                if (FromPort != "any")
                    from += $" port {FromPort}";
                parts.Add($"from {from}");
            }
            
            if (ToIp != "any" || ToPort != "any")
            {
                var to = ToIp == "any" ? "anywhere" : ToIp;
                if (ToPort != "any")
                    to += $" port {ToPort}";
                parts.Add($"to {to}");
            }
            
            if (Interface != "any")
                parts.Add($"on {Interface}");
                
            if (!string.IsNullOrEmpty(App))
                parts.Add($"app '{App}'");
                
            if (Log)
                parts.Add("log");
            
            return string.Join(" ", parts);
        }
    }
}
