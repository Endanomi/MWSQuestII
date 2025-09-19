using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Services.IDS
{
    [CreateAssetMenu(fileName = "IDS Emulator", menuName = "ScriptableObjects/IDS Emulator", order = 1)]
    public class IDSEmulator : ScriptableObject
    {
        private IDSEngine core = new IDSEngine();

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