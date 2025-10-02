using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Services.IDS
{
    [CreateAssetMenu(fileName = "IDS Emulator", menuName = "ScriptableObjects/IDS Emulator", order = 1)]
    public class IDSEmulator : ScriptableObject
    {
        private IDSEngine core = new IDSEngine();

        public void Execute(string ruleString)
        {
            var parser = new Parser();
            var filterRule = parser.Parse(ruleString);
            core.Execute(filterRule);
        }

        public List<FilterRule> GetRules()
        {
            return core.GetRules();
        }
    }
}