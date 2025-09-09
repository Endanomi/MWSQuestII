using NUnit.Framework;
using UnityEngine;
using Services.IDS;

public class TestIDSEmulator
{
    [Test]
    public void TestIDSEmulator_DefaultValues()
    {
        var iDSEmulator = ScriptableObject.CreateInstance<IDSEmulator>();

        string rule1 = "alert tcp any any -> any 80 (msg:\"Test rule 1\"; sid:1000001;)";
        string rule2 = "alert udp any any -> any 53 (msg:\"Test rule 2\"; sid:1000002;)";

        iDSEmulator.AddRule(rule1);
        iDSEmulator.AddRule(rule2);

        Assert.AreEqual(2, iDSEmulator.filterRules.Count);
    }

}
