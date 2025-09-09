using System.Collections.Generic;
using UnityEngine;

namespace Services.Firewall
{
    public class FirewallEngine
    {
        private List<FilterRule> filterRules = new List<FilterRule>();

        public void AddRule(FilterRule rule)
        {
            filterRules.Add(rule);
            Debug.Log($"Rule added. Total rules count: {filterRules.Count}");
        }

        public void AddRuleFromString(string ruleString)
        {
            var filterRule = Parser.Parse(ruleString);
            AddRule(filterRule);
        }

        public List<FilterRule> GetRules()
        {
            return filterRules;
        }
    }
}