using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Collections;
using UnityEngine;

namespace Services.IDS
{
    // snortのルールをパースするクラス
    public class Parser
    {
        public Dictionary<string, string> Parse(string rule)
        {

            var commandPattern = @"^(?<command>\w+)\s*";

            var commandMatch = Regex.Match(rule, commandPattern);
            switch (commandMatch.Groups["command"].Value)
            {
                case "add":
                    return ParseAddRule(rule);

                case "delete":
                    return ParseDeleteRule(rule);
                case "move":
                    return ParseMoveRule(rule);
                default:
                    Debug.LogWarning($"Unsupported IDS command: {commandMatch.Groups["command"].Value}");
                    return new Dictionary<string, string>();
            }

        }

        private Dictionary<string, string> ParseAddRule(string rule)
        {
            var pattern = @"^(?<command>\w+)\s+"
                        + @"(?<action>\w+)\s+"
                        + @"(?<source>\S+)\s+"
                        + @"(?<direction>\S+)\s+"
                        + @"(?<destination>\S+)\s+"
                        + @"(?<occupation>\S+)\s+"
                        + @"(?<baggage>\S+)\s*"
                        + @"\((?<options>.*)\)$";

            var match = Regex.Match(rule, pattern);
            if (!match.Success)
            {
                Debug.LogError($"Failed to parse rule: {rule}");
                return new Dictionary<string, string>();
            }

            var optionsPattern = @"(?<key>\w+):(?<value>[^;]+);?";
            var optionsMatches = Regex.Matches(match.Groups["options"].Value, optionsPattern);

            var options = new Dictionary<string, string>(optionsMatches.Count + 5);
            
            options["command"] = match.Groups["command"].Value;
            options["action"] = match.Groups["action"].Value;
            options["source"] = match.Groups["source"].Value;
            options["direction"] = match.Groups["direction"].Value;
            options["destination"] = match.Groups["destination"].Value;
            options["occupation"] = match.Groups["occupation"].Value;
            options["baggage"] = match.Groups["baggage"].Value;

            foreach (Match optionMatch in optionsMatches)
            {
                var key = optionMatch.Groups["key"].Value;
                var value = optionMatch.Groups["value"].Value;
                options[key] = value;
            }

            return options;
        }

        private Dictionary<string, string> ParseDeleteRule(string rule)
        {
            var pattern = @"^(?<command>\w+)\s+(?<rule_id>\d+)$";
            var match = Regex.Match(rule, pattern);
            if (!match.Success)
            {
                Debug.LogError($"Failed to parse rule: {rule}");
                return new Dictionary<string, string>();
            }

            var options = new Dictionary<string, string>
            {
                { "command", match.Groups["command"].Value },
                { "rule_id", match.Groups["rule_id"].Value }
            };

            return options;
        }

        private Dictionary<string, string> ParseMoveRule(string rule)
        {
            var pattern = @"^(?<command>\w+)\s+(?<from_id>\d+)\s+(?<to_id>\d+)$";
            var match = Regex.Match(rule, pattern);
            if (!match.Success)
            {
                Debug.LogError($"Failed to parse rule: {rule}");
                return new Dictionary<string, string>();
            }

            var options = new Dictionary<string, string>
            {
                { "command", match.Groups["command"].Value },
                { "from_id", match.Groups["from_id"].Value },
                { "to_id", match.Groups["to_id"].Value }
            };
            return options;
        }
    }
}