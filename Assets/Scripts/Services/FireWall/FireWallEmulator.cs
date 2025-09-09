using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Firewall
{
    [CreateAssetMenu(fileName = "Firewall Emulator", menuName = "ScriptableObjects/Firewall Emulator", order = 1)]
    public class FirewallEmulator : ScriptableObject
    {
        private FirewallEngine core = new FirewallEngine();

        public void AddRule(string ruleString)
        {
            var filterRule = Parser.Parse(ruleString);
            core.AddRule(filterRule);
        }

        public List<FilterRule> GetRules()
        {
            return core.GetRules();
        }
    }
}