using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Services.IDS
{
    // snortのルールをパースするクラス
    public class Parser
    {
        // ルールをパースして、IDSのフィルタに変換するメソッド
        public static FilterRule Parse(string rule)
        {
            var headerPattern = @"^(?<action>\w+)\s+" +
                    @"(?<protocol>\w+)\s+" +
                    @"(?<src_ip>[\d\.\/]+|any|\$[a-zA-Z_]+)\s+" +
                    @"(?<src_port>[\d\*]+|any|\$[a-zA-Z_]+)\s+" +
                    @"(?<direction>->|<-|<>)\s+" +
                    @"(?<dst_ip>[\d\.\/]+|any|\$[a-zA-Z_]+)\s+" +
                    @"(?<dst_port>[\d\*]+|any|\$[a-zA-Z_]+)\s*";
            var headerMatch = Regex.Match(rule, headerPattern);

            var idsRule = new FilterRule
            {
                Action = headerMatch.Groups["action"].Value,
                Protocol = headerMatch.Groups["protocol"].Value,
                SrcIp = headerMatch.Groups["src_ip"].Value,
                SrcPort = headerMatch.Groups["src_port"].Value,
                Direction = headerMatch.Groups["direction"].Value,
                DstIp = headerMatch.Groups["dst_ip"].Value,
                DstPort = headerMatch.Groups["dst_port"].Value
            };

            var optionsPattern = @"\((?<options>.*)\)";
            var optionsMatch = Regex.Match(rule, optionsPattern);
            if (optionsMatch.Success)
            {
                var optionsString = optionsMatch.Groups["options"].Value;
                var options = optionsString.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var option in options)
                {
                    var keyValue = option.Split(new[] { ':' }, 2);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim().Trim('"', '\'');
                        var value = keyValue[1].Trim().Trim('"', '\'');
                        idsRule.Options[key] = value;
                    }
                    else
                    {
                        var key = keyValue[0].Trim();
                        idsRule.Options[key] = string.Empty;
                    }
                }
            }
            Debug.Log($"Parsed IDS Rule: Action={idsRule.Action}, Protocol={idsRule.Protocol}, SrcIp={idsRule.SrcIp}, SrcPort={idsRule.SrcPort}, Direction={idsRule.Direction}, DstIp={idsRule.DstIp}, DstPort={idsRule.DstPort}");
            foreach (var option in idsRule.Options)
            {
                Debug.Log($"Option: {option.Key} = {option.Value}");
            }
            return idsRule;
        }
    }
}