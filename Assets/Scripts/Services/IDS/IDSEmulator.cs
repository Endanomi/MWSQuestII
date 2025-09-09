using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Services.IDS
{
    [CreateAssetMenu(fileName = "IDS Emulator", menuName = "ScriptableObjects/IDS Emulator", order = 1)]
    public class IDSEmulator : ScriptableObject
    {
        public List<FilterRule> filterRules = new List<FilterRule>();

        public void AddRule(string ruleString)
        {
            var filterRule = Parser.Parse(ruleString);
            filterRules.Add(filterRule);
        }
    }
}