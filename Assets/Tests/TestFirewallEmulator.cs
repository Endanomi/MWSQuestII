using NUnit.Framework;
using UnityEngine;
using Services.Firewall;

public class TestFirewallEmulator
{
    [Test]
    public void TestFirewallEmulator_DefaultValues()
    {
        var firewallEmulator = ScriptableObject.CreateInstance<FirewallEmulator>();
        string rule1 = "-A INPUT -p tcp --dport 80 -j ACCEPT";

        firewallEmulator.ExecuteCommand(rule1);
        firewallEmulator.ExecuteCommand("--help");

        Assert.AreEqual("ACCEPT", firewallEmulator.GetRules()[0].Jump);
        Assert.AreEqual("tcp", firewallEmulator.GetRules()[0].Protocol);
        Assert.AreEqual("any", firewallEmulator.GetRules()[0].SrcIp);
        Assert.AreEqual("any", firewallEmulator.GetRules()[0].SrcPort);
        Assert.AreEqual("any", firewallEmulator.GetRules()[0].DstIp);
        Assert.AreEqual("80", firewallEmulator.GetRules()[0].DstPort);
        Assert.AreEqual("INPUT", firewallEmulator.GetRules()[0].Chain);
        Assert.AreEqual("A", firewallEmulator.GetRules()[0].Command);
        Assert.AreEqual("filter", firewallEmulator.GetRules()[0].Table);
    }

}
