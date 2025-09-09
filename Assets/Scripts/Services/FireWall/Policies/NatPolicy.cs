using System.Collections.Generic;

namespace Services.Firewall.Policies
{
    // NAT テーブルのポリシー管理クラス
    public class NatPolicy : ITablePolicy
    {
        public string PreroutingPolicy { get; set; } = "ACCEPT";
        public string InputPolicy { get; set; } = "ACCEPT";
        public string OutputPolicy { get; set; } = "ACCEPT";
        public string PostroutingPolicy { get; set; } = "ACCEPT";
        
        public List<string> CustomChains { get; set; } = new();
        
        public static readonly List<string> BuiltInChains = new()
        {
            "PREROUTING", "INPUT", "OUTPUT", "POSTROUTING"
        };
        
        public static readonly List<string> ValidTargets = new()
        {
            "ACCEPT", "DROP", "RETURN", "DNAT", "SNAT", "MASQUERADE", "REDIRECT"
        };
        
        public NatPolicy()
        {
            CustomChains = new List<string>();
        }
        
        public bool IsValidChain(string chain)
        {
            return BuiltInChains.Contains(chain) || CustomChains.Contains(chain);
        }
        
        public bool IsValidTarget(string target)
        {
            return ValidTargets.Contains(target) || CustomChains.Contains(target);
        }
        
        public string GetPolicy(string chain)
        {
            return chain switch
            {
                "PREROUTING" => PreroutingPolicy,
                "INPUT" => InputPolicy,
                "OUTPUT" => OutputPolicy,
                "POSTROUTING" => PostroutingPolicy,
                _ => "ACCEPT"
            };
        }
        
        public bool SetPolicy(string chain, string policy)
        {
            if (!BuiltInChains.Contains(chain) || !ValidTargets.Contains(policy))
                return false;
                
            switch (chain)
            {
                case "PREROUTING":
                    PreroutingPolicy = policy;
                    return true;
                case "INPUT":
                    InputPolicy = policy;
                    return true;
                case "OUTPUT":
                    OutputPolicy = policy;
                    return true;
                case "POSTROUTING":
                    PostroutingPolicy = policy;
                    return true;
                default:
                    return false;
            }
        }
        
        public bool AddCustomChain(string chain)
        {
            if (BuiltInChains.Contains(chain) || CustomChains.Contains(chain))
                return false;
                
            CustomChains.Add(chain);
            return true;
        }
        
        public bool RemoveCustomChain(string chain)
        {
            return CustomChains.Remove(chain);
        }
        
        public IEnumerable<string> GetAllChains()
        {
            var allChains = new List<string>(BuiltInChains);
            allChains.AddRange(CustomChains);
            return allChains;
        }
    }
}
