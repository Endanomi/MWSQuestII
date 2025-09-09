using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Services.Firewall
{
    // iptables コマンドをパースするクラス
    public class Parser
    {
        public static FilterRule Parse(string command)
        {
            var rule = new FilterRule();

            // テーブル指定 (-t nat など)
            var tableMatch = Regex.Match(command, @"-t\s+(?<table>\w+)");
            if (tableMatch.Success)
                rule.Table = tableMatch.Groups["table"].Value;

            // コマンド (-A, -I, -D, etc.)
            var cmdMatch = Regex.Match(command, @"(?:^|\s)-(?<cmd>[A-Z]+)\s+(?<chain>\w+)");
            if (cmdMatch.Success)
            {
                rule.Command = cmdMatch.Groups["cmd"].Value;
                rule.Chain = cmdMatch.Groups["chain"].Value;
            }

            // プロトコル (-p tcp, udp, icmp)
            var protoMatch = Regex.Match(command, @"-p\s+(?<proto>\w+)");
            if (protoMatch.Success)
                rule.Protocol = protoMatch.Groups["proto"].Value;

            // ソース / 宛先 IP
            var srcIpMatch = Regex.Match(command, @"-s\s+(?<src>[^\s]+)");
            if (srcIpMatch.Success)
                rule.SrcIp = srcIpMatch.Groups["src"].Value;

            var dstIpMatch = Regex.Match(command, @"-d\s+(?<dst>[^\s]+)");
            if (dstIpMatch.Success)
                rule.DstIp = dstIpMatch.Groups["dst"].Value;

            // ポート
            var sportMatch = Regex.Match(command, @"--sport\s+(?<sport>\d+)");
            if (sportMatch.Success)
                rule.SrcPort = sportMatch.Groups["sport"].Value;

            var dportMatch = Regex.Match(command, @"--dport\s+(?<dport>\d+)");
            if (dportMatch.Success)
                rule.DstPort = dportMatch.Groups["dport"].Value;

            // ジャンプターゲット (-j ACCEPT, DROP, REJECT, MASQUERADE, DNAT, SNAT)
            var jumpMatch = Regex.Match(command, @"-j\s+(?<jump>\w+)");
            if (jumpMatch.Success)
                rule.Jump = jumpMatch.Groups["jump"].Value;

            // その他のオプションを拾う（例: --to-destination 192.168.1.10:80）
            var optionPattern = @"--(?<key>\w+)(?:\s+(?<value>[^\s]+))?";
            foreach (Match m in Regex.Matches(command, optionPattern))
            {
                var key = m.Groups["key"].Value;
                var value = m.Groups["value"].Success ? m.Groups["value"].Value : string.Empty;

                // 既にプロトコル/ポート等で拾ったものは除外
                if (key is "dport" or "sport" or "to-destination")
                    continue;

                rule.Options[key] = value;
            }

            // Debug出力
            Debug.Log($"Parsed iptables Rule: Table={rule.Table}, Command={rule.Command}, Chain={rule.Chain}, Proto={rule.Protocol}, SrcIp={rule.SrcIp}, DstIp={rule.DstIp}, SrcPort={rule.SrcPort}, DstPort={rule.DstPort}, Jump={rule.Jump}");
            foreach (var option in rule.Options)
            {
                Debug.Log($"Option: {option.Key} = {option.Value}");
            }

            return rule;
        }
    }
}
