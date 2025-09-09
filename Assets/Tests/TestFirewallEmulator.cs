using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Services.Firewall;

namespace Services.Tests
{
    public class TestFirewallEmulator
    {
        [Test]
        public void TestFirewallEmulator_BasicCommands()
        {
            // Arrange
            var firewall = ScriptableObject.CreateInstance<FirewallEmulator>();

            // Act - UFWコマンドを実行
            firewall.ExecuteCommand("ufw allow 22/tcp");
            firewall.ExecuteCommand("ufw allow 80");
            firewall.ExecuteCommand("ufw deny from 192.168.1.100");

            // Assert
            var rules = firewall.GetRules();
            Assert.That(rules.Count, Is.EqualTo(3));

            // SSH許可ルール
            Assert.That(rules[0].Action, Is.EqualTo("allow"));
            Assert.That(rules[0].Protocol, Is.EqualTo("tcp"));
            Assert.That(rules[0].ToPort, Is.EqualTo("22"));
            Assert.That(rules[0].Direction, Is.EqualTo("in"));

            // HTTP許可ルール
            Assert.That(rules[1].Action, Is.EqualTo("allow"));
            Assert.That(rules[1].ToPort, Is.EqualTo("80"));
            Assert.That(rules[1].Direction, Is.EqualTo("in"));

            // IP拒否ルール
            Assert.That(rules[2].Action, Is.EqualTo("deny"));
            Assert.That(rules[2].FromIp, Is.EqualTo("192.168.1.100"));
        }

        [Test]
        public void TestFirewallEmulator_AppCommands()
        {
            // Arrange
            var firewall = ScriptableObject.CreateInstance<FirewallEmulator>();

            // Act - アプリケーション関連のコマンド
            firewall.ExecuteCommand("ufw allow OpenSSH");
            firewall.ExecuteCommand("ufw deny Nginx");

            // Assert
            var rules = firewall.GetRules();
            Assert.That(rules.Count, Is.EqualTo(2));

            // OpenSSH許可
            Assert.That(rules[0].Action, Is.EqualTo("allow"));
            Assert.That(rules[0].App, Is.EqualTo("OpenSSH"));

            // Nginx拒否
            Assert.That(rules[1].Action, Is.EqualTo("deny"));
            Assert.That(rules[1].App, Is.EqualTo("Nginx"));
        }

        [Test]
        public void TestFirewallEmulator_DirectionalCommands()
        {
            // Arrange
            var firewall = ScriptableObject.CreateInstance<FirewallEmulator>();

            // Act - 方向指定コマンド
            firewall.ExecuteCommand("ufw allow out 53");
            firewall.ExecuteCommand("ufw allow in on eth0 to any port 80");

            // Assert
            var rules = firewall.GetRules();
            Assert.That(rules.Count, Is.EqualTo(2));

            // DNS出力許可
            Assert.That(rules[0].Action, Is.EqualTo("allow"));
            Assert.That(rules[0].Direction, Is.EqualTo("out"));
            Assert.That(rules[0].ToPort, Is.EqualTo("53"));

            // インターフェース指定HTTP許可
            Assert.That(rules[1].Action, Is.EqualTo("allow"));
            Assert.That(rules[1].Direction, Is.EqualTo("in"));
            Assert.That(rules[1].Interface, Is.EqualTo("eth0"));
            Assert.That(rules[1].ToPort, Is.EqualTo("80"));
        }

        [Test]
        public void TestFirewallEmulator_ComplexCommands()
        {
            // Arrange
            var firewall = ScriptableObject.CreateInstance<FirewallEmulator>();

            // Act - 複雑なコマンド
            firewall.ExecuteCommand("ufw allow from 192.168.1.0/24 to any port 22 proto tcp");
            firewall.ExecuteCommand("ufw limit ssh");

            // Assert
            var rules = firewall.GetRules();
            Assert.That(rules.Count, Is.EqualTo(2));

            // サブネットからのSSH許可
            Assert.That(rules[0].Action, Is.EqualTo("allow"));
            Assert.That(rules[0].FromIp, Is.EqualTo("192.168.1.0/24"));
            Assert.That(rules[0].ToPort, Is.EqualTo("22"));
            Assert.That(rules[0].Protocol, Is.EqualTo("tcp"));

            // SSH制限
            Assert.That(rules[1].Action, Is.EqualTo("limit"));
            Assert.That(rules[1].ToPort, Is.EqualTo("22"));
        }

        [Test]
        public void TestFirewallEmulator_HelpCommand()
        {
            // Arrange
            var firewall = ScriptableObject.CreateInstance<FirewallEmulator>();

            // Act - ヘルプコマンド
            string result = firewall.ExecuteCommand("ufw --help");

            // Assert - ヘルプコマンドはルールを追加せず、ヘルプテキストを返す
            var rules = firewall.GetRules();
            Assert.That(rules.Count, Is.EqualTo(0));
            Assert.That(result, Does.Contain("usage"));
        }

        [Test]
        public void TestFirewallEmulator_StatusCommand()
        {
            // Arrange
            var firewall = ScriptableObject.CreateInstance<FirewallEmulator>();
            firewall.ExecuteCommand("ufw allow 22");

            // Act - ステータスコマンド
            string result = firewall.ExecuteCommand("ufw status");

            // Assert - ステータス表示
            Assert.That(result, Does.Contain("Status: active"));
            Assert.That(result, Does.Contain("22"));
        }
    }
}
