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

        iDSEmulator.AddRule(rule1);

        Assert.AreEqual("alert", iDSEmulator.GetRules()[0].Action);
        Assert.AreEqual("tcp", iDSEmulator.GetRules()[0].Protocol);
        Assert.AreEqual("any", iDSEmulator.GetRules()[0].SrcIp);
        Assert.AreEqual("any", iDSEmulator.GetRules()[0].SrcPort);
        Assert.AreEqual("->", iDSEmulator.GetRules()[0].Direction);
        Assert.AreEqual("any", iDSEmulator.GetRules()[0].DstIp);
        Assert.AreEqual("80", iDSEmulator.GetRules()[0].DstPort);
        Assert.AreEqual("Test rule 1", iDSEmulator.GetRules()[0].Options["msg"]);
        Assert.AreEqual("1000001", iDSEmulator.GetRules()[0].Options["sid"]);
    }

}
