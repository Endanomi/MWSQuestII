using System.Collections.Generic;
using UnityEngine;

namespace Services.IDS
{
    public class IDSEngine
    {
        private List<FilterRule> filterRules = new List<FilterRule>();

        public void Execute(Dictionary<string, string> ruleDict)
        {
            ruleDict = ValidateRule(ruleDict);
            if (ruleDict.Count == 0)
            {
                Debug.LogWarning("Rule validation failed.");
                return;
            }
            switch (ruleDict["command"])
            {
                case "add":
                    // キー「action」が存在するかどうか
                    FilterRule newRule = new FilterRule
                    {
                        Action = ruleDict["action"],
                        Departure = ruleDict["departure"],
                        Destination = ruleDict["destination"],
                        Occupation = ruleDict["occupation"],
                        Item = ruleDict.ContainsKey("item") ? ruleDict["item"] : "any",
                        MaxItemSize = ruleDict.ContainsKey("max_item_size") && int.TryParse(ruleDict["max_item_size"], out int size) ? size : int.MaxValue
                    };
                    filterRules.Add(newRule);
                    break;
                case "delete":
                    int idToRemove = int.Parse(ruleDict["rule_id"]);
                    filterRules.RemoveAt(idToRemove - 1);
                    break;
                case "move":
                    int fromId = int.Parse(ruleDict["from_id"]);
                    int toId = int.Parse(ruleDict["to_id"]);
                    var ruleToMove = filterRules[fromId - 1];
                    filterRules.RemoveAt(fromId - 1);
                    filterRules.Insert(toId - 1, ruleToMove);
                    break;
                default:
                    Debug.LogWarning("Unknown command: " + ruleDict["command"]);
                    break;
            }
        }

        public Dictionary<string, string> ValidateRule(Dictionary<string, string> ruleDict)
        {
            try
            {
                Debug.Log("Validating rule: " + string.Join(", ", ruleDict));
                if (!ruleDict.ContainsKey("command") || (ruleDict["command"] != "add" && ruleDict["command"] != "delete" && ruleDict["command"] != "move"))
                {
                    throw new System.Exception("Invalid or missing command. Must be 'add', 'delete' or 'move'.");
                }
                switch (ruleDict["command"])
                {
                    case "add":
                        string[] requiredKeys = { "action", "departure", "direction", "destination", "occupation", "item", "max_item_size" };
                        // 必須キーが存在しない場合、デフォルト値を設定
                        foreach (var key in requiredKeys)
                        {
                            if (!ruleDict.ContainsKey(key))
                            {
                                ruleDict[key] = "any";
                            }
                        }
                        // actionは、pass, drop, rejectのみ許可
                        if (ruleDict["action"] != "pass" && ruleDict["action"] != "drop" && ruleDict["action"] != "reject")
                        {
                            throw new System.Exception("Invalid action. Must be 'pass', 'drop', or 'reject'.");
                        }
                        // directionは、->のみ許可
                        if (ruleDict["direction"] != "->")
                        {
                            throw new System.Exception("Invalid direction. Must be '->'.");
                        }
                        break;
                    case "delete":
                        if (!ruleDict.ContainsKey("rule_id") || !int.TryParse(ruleDict["rule_id"], out int id) || id <= 0)
                        {
                            throw new System.Exception("Invalid or missing id for removal. Must be a non-negative integer.");
                        }
                        if (id > filterRules.Count)
                        {
                            throw new System.Exception("Rule id out of range.");
                        }
                        break;
                    case "move":
                        if (!ruleDict.ContainsKey("from_id") || !int.TryParse(ruleDict["from_id"], out int fromId) || fromId <= 0)
                        {
                            throw new System.Exception("Invalid or missing from_id for move. Must be a non-negative integer.");
                        }
                        if (!ruleDict.ContainsKey("to_id") || !int.TryParse(ruleDict["to_id"], out int toId) || toId <= 0)
                        {
                            throw new System.Exception("Invalid or missing to_id for move. Must be a non-negative integer.");
                        }
                        if (fromId > filterRules.Count || toId > filterRules.Count)
                        {
                            throw new System.Exception("from_id or to_id out of range.");
                        }
                        break;
                }
                return ruleDict;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Validation error: " + e.Message);
                return new Dictionary<string, string>();
            }
        }

        public List<FilterRule> GetRules()
        {
            return filterRules;
        }

        public string Challenge(PersonProperties properties)
        {
            foreach (var rule in filterRules)
            {
                bool match = true;

                if (rule.Departure != "any" && rule.Departure != properties.Departure)
                {
                    match = false;
                }
                if (rule.Destination != "any" && rule.Destination != properties.Destination)
                {
                    match = false;
                }
                if (rule.Occupation != "any" && rule.Occupation != properties.Occupation)
                {
                    match = false;
                }
                if (rule.Item != "any" && !properties.Items.Contains(rule.Item))
                {
                    match = false;
                }
                if (rule.MaxItemSize != int.MaxValue)
                {
                    if (properties.MaxItemSize > rule.MaxItemSize)
                    {
                        match = false;
                    }
                }

                if (match)
                {
                    return rule.Action;
                }
            }
            return "pass"; // デフォルトのアクション
        }
    }
}