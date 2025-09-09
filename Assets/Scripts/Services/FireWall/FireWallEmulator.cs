using System.Collections.Generic;
using UnityEngine;

namespace Services.Firewall
{
    [CreateAssetMenu(fileName = "Firewall Emulator", menuName = "ScriptableObjects/Firewall Emulator", order = 1)]
    public class FirewallEmulator : ScriptableObject
    {
        private FirewallEngine core = new FirewallEngine();

        // UFWコマンドを実行（Core.csに処理を委譲）
        public void ExecuteCommand(string command)
        {
            core.ExecuteCommand(command);
        }

        // ルールを追加（IDSEmulatorと同じAPIで互換性を保つ）
        public void AddRule(string ruleString)
        {
            core.AddRuleFromString(ruleString);
        }

        // 全ルールを取得（IDSEmulatorと同じAPI）
        public List<FilterRule> GetRules()
        {
            return core.GetRules();
        }
        
        // ファイアウォールの状態を取得
        public bool IsEnabled()
        {
            return core.IsEnabled();
        }
    }
}