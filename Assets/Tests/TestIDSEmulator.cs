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

        iDSEmulator.Execute(rule1);

    }

}
