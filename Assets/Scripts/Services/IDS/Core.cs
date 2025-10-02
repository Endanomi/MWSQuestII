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
            switch (ruleDict["command"])
            {
                case "add":
                    // キー「action」が存在するかどうか
                    FilterRule newRule = new FilterRule
                    {
                        Action = ruleDict["action"],
                        Source = ruleDict["source"],
                        Destination = ruleDict["destination"],
                        Occupation = ruleDict["occupation"],
                        Baggage = ruleDict["baggage"],
                        MaxBaggageSize = ruleDict["max_baggage_size"]
                    };
                    filterRules.Add(newRule);
                    break;
                case "remove":
                    int idToRemove = int.Parse(ruleDict["rule_id"]);
                    filterRules.RemoveAt(idToRemove);
                    break;
                case "move":
                    int fromId = int.Parse(ruleDict["from_id"]);
                    int toId = int.Parse(ruleDict["to_id"]);
                    var ruleToMove = filterRules[fromId];
                    filterRules.RemoveAt(fromId);
                    filterRules.Insert(toId, ruleToMove);
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
                if (!ruleDict.ContainsKey("command") || (ruleDict["command"] != "add" && ruleDict["command"] != "remove"))
                {
                    throw new System.Exception("Invalid or missing command. Must be 'add' or 'remove'.");
                }
                switch (ruleDict["command"])
                {
                    case "add":
                        string[] requiredKeys = { "action", "source", "direction", "destination", "occupation", "baggage" };
                        // 必須キーが存在しない場合、デフォルト値を設定
                        foreach (var key in requiredKeys)
                        {
                            if (!ruleDict.ContainsKey(key))
                            {
                                ruleDict[key] = "any";
                            }
                        }
                        // actionは、allowかdenyのみ許可
                        if (ruleDict["action"] != "allow" && ruleDict["action"] != "deny")
                        {
                            throw new System.Exception("Invalid action. Must be 'allow' or 'deny'.");
                        }
                        // directionは、->のみ許可
                        if (ruleDict["direction"] != "->")
                        {
                            throw new System.Exception("Invalid direction. Must be '->'.");
                        }
                        break;
                    case "delete":
                        if (!ruleDict.ContainsKey("rule_id") || !int.TryParse(ruleDict["rule_id"], out int id) || id < 0)
                        {
                            throw new System.Exception("Invalid or missing id for removal. Must be a non-negative integer.");
                        }
                        if (id >= filterRules.Count)
                        {
                            throw new System.Exception("Rule id out of range.");
                        }
                        break;
                    case "move":
                        if (!ruleDict.ContainsKey("from_id") || !int.TryParse(ruleDict["from_id"], out int fromId) || fromId < 0)
                        {
                            throw new System.Exception("Invalid or missing from_id for move. Must be a non-negative integer.");
                        }
                        if (!ruleDict.ContainsKey("to_id") || !int.TryParse(ruleDict["to_id"], out int toId) || toId < 0)
                        {
                            throw new System.Exception("Invalid or missing to_id for move. Must be a non-negative integer.");
                        }
                        if (fromId >= filterRules.Count || toId >= filterRules.Count)
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
                return null;
            }
        }

        public List<FilterRule> GetRules()
        {
            return filterRules;
        }
    } 
}