using System.Collections.Generic;

namespace Services.Firewall.Policies
{
    // 全てのポリシークラスの基底インターフェース
    public interface ITablePolicy
    {
        bool IsValidChain(string chain);
        bool IsValidTarget(string target);
        string GetPolicy(string chain);
        bool SetPolicy(string chain, string policy);
        bool AddCustomChain(string chain);
        bool RemoveCustomChain(string chain);
        IEnumerable<string> GetAllChains();
        List<string> CustomChains { get; set; }
    }
    
    // ポリシー管理のマネージャークラス
    public class PolicyManager
    {
        public FilterPolicy FilterTable { get; private set; }
        public NatPolicy NatTable { get; private set; }
        public ManglePolicy MangleTable { get; private set; }
        public RawPolicy RawTable { get; private set; }
        public SecurityPolicy SecurityTable { get; private set; }
        
        private readonly Dictionary<string, ITablePolicy> tables;
        
        public PolicyManager()
        {
            FilterTable = new FilterPolicy();
            NatTable = new NatPolicy();
            MangleTable = new ManglePolicy();
            RawTable = new RawPolicy();
            SecurityTable = new SecurityPolicy();
            
            tables = new Dictionary<string, ITablePolicy>
            {
                ["filter"] = FilterTable,
                ["nat"] = NatTable,
                ["mangle"] = MangleTable,
                ["raw"] = RawTable,
                ["security"] = SecurityTable
            };
        }
        
        public ITablePolicy GetTablePolicy(string tableName)
        {
            return tables.TryGetValue(tableName.ToLower(), out var policy) ? policy : null;
        }
        
        public bool IsValidTable(string tableName)
        {
            return tables.ContainsKey(tableName.ToLower());
        }
        
        public bool IsValidChain(string tableName, string chainName)
        {
            var policy = GetTablePolicy(tableName);
            return policy?.IsValidChain(chainName) ?? false;
        }
        
        public bool IsValidTarget(string tableName, string targetName)
        {
            var policy = GetTablePolicy(tableName);
            return policy?.IsValidTarget(targetName) ?? false;
        }
        
        public string GetPolicy(string tableName, string chainName)
        {
            var policy = GetTablePolicy(tableName);
            return policy?.GetPolicy(chainName) ?? "ACCEPT";
        }
        
        public bool SetPolicy(string tableName, string chainName, string policyName)
        {
            var policy = GetTablePolicy(tableName);
            return policy?.SetPolicy(chainName, policyName) ?? false;
        }
        
        public bool AddCustomChain(string tableName, string chainName)
        {
            var policy = GetTablePolicy(tableName);
            return policy?.AddCustomChain(chainName) ?? false;
        }
        
        public bool RemoveCustomChain(string tableName, string chainName)
        {
            var policy = GetTablePolicy(tableName);
            return policy?.RemoveCustomChain(chainName) ?? false;
        }
        
        public IEnumerable<string> GetAllChains(string tableName)
        {
            var policy = GetTablePolicy(tableName);
            return policy?.GetAllChains() ?? new List<string>();
        }
        
        public IEnumerable<string> GetTableNames()
        {
            return tables.Keys;
        }
        
        // 全テーブルの統計情報を取得
        public Dictionary<string, int> GetCustomChainCounts()
        {
            var counts = new Dictionary<string, int>();
            foreach (var kvp in tables)
            {
                counts[kvp.Key] = kvp.Value.CustomChains.Count;
            }
            return counts;
        }
        
        // 全カスタムチェーンをクリア
        public void ClearAllCustomChains()
        {
            foreach (var policy in tables.Values)
            {
                policy.CustomChains.Clear();
            }
        }
        
        // 指定テーブルの全カスタムチェーンをクリア
        public bool ClearCustomChains(string tableName)
        {
            var policy = GetTablePolicy(tableName);
            if (policy != null)
            {
                policy.CustomChains.Clear();
                return true;
            }
            return false;
        }
    }
}
