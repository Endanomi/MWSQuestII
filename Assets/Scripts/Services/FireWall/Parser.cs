using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

namespace Services.Firewall
{
    // UFWコマンドをパースするクラス
    public class Parser
    {
        // UFWコマンドをパースして、Firewallのフィルタに変換するメソッド
        public static FilterRule Parse(string command)
        {
            var rule = new FilterRule();
            
            // コメントの処理
            var commentMatch = Regex.Match(command, @"#\s*(.*)$");
            if (commentMatch.Success)
            {
                rule.Comment = commentMatch.Groups[1].Value.Trim();
                command = command.Substring(0, commentMatch.Index).Trim();
            }
            
            // 基本アクションの抽出 (allow, deny, reject, limit)
            var actionMatch = Regex.Match(command, @"\b(allow|deny|reject|limit)\b", RegexOptions.IgnoreCase);
            if (actionMatch.Success)
            {
                rule.Action = actionMatch.Groups[1].Value.ToLower();
            }
            
            // 方向の抽出 (in, out)
            var directionMatch = Regex.Match(command, @"\b(in|out)\b", RegexOptions.IgnoreCase);
            if (directionMatch.Success)
            {
                rule.Direction = directionMatch.Groups[1].Value.ToLower();
            }
            
            // ログ設定の抽出
            if (Regex.IsMatch(command, @"\blog(-all)?\b", RegexOptions.IgnoreCase))
            {
                rule.Log = true;
            }
            
            // プロトコルの抽出
            var protoMatch = Regex.Match(command, @"\bproto\s+(tcp|udp|any)\b", RegexOptions.IgnoreCase);
            if (protoMatch.Success)
            {
                rule.Protocol = protoMatch.Groups[1].Value.ToLower();
            }
            
            // ポート指定のパターン (22, 22/tcp, 80:443, etc)
            var portMatch = Regex.Match(command, @"\b(\d+(?::\d+)?(?:/(?:tcp|udp))?)\b", RegexOptions.IgnoreCase);
            if (portMatch.Success)
            {
                var portSpec = portMatch.Groups[1].Value;
                var protocolInPort = Regex.Match(portSpec, @"/(tcp|udp)", RegexOptions.IgnoreCase);
                if (protocolInPort.Success)
                {
                    rule.Protocol = protocolInPort.Groups[1].Value.ToLower();
                    portSpec = portSpec.Replace("/" + protocolInPort.Groups[1].Value, "");
                }
                rule.ToPort = portSpec;
            }
            
            // from 句の処理
            var fromMatch = Regex.Match(command, @"\bfrom\s+([\w\.\-/]+)(?:\s+port\s+([\w\-:]+))?", RegexOptions.IgnoreCase);
            if (fromMatch.Success)
            {
                rule.FromIp = fromMatch.Groups[1].Value;
                if (fromMatch.Groups[2].Success)
                {
                    rule.FromPort = fromMatch.Groups[2].Value;
                }
            }
            
            // to 句の処理
            var toMatch = Regex.Match(command, @"\bto\s+([\w\.\-/]+)(?:\s+port\s+([\w\-:]+))?", RegexOptions.IgnoreCase);
            if (toMatch.Success)
            {
                rule.ToIp = toMatch.Groups[1].Value;
                if (toMatch.Groups[2].Success)
                {
                    rule.ToPort = toMatch.Groups[2].Value;
                }
            }
            
            // on 句（インターフェース）の処理
            var onMatch = Regex.Match(command, @"\bon\s+([\w\-]+)", RegexOptions.IgnoreCase);
            if (onMatch.Success)
            {
                rule.Interface = onMatch.Groups[1].Value;
            }
            
            // app 句の処理
            var appMatch = Regex.Match(command, @"\bapp\s+['""]?([^'""]+)['""]?", RegexOptions.IgnoreCase);
            if (appMatch.Success)
            {
                rule.App = appMatch.Groups[1].Value;
            }
            
            // 簡単なポート番号のみの場合の処理（例: ufw allow 22）
            if (rule.ToPort == "any" && rule.FromPort == "any")
            {
                var simplePortMatch = Regex.Match(command, @"\b(\d+)\b");
                if (simplePortMatch.Success)
                {
                    rule.ToPort = simplePortMatch.Value;
                }
            }
            
            // 一般的なサービス名の処理
            ParseCommonServices(command, rule);
            
            // アプリケーション名のみの場合の処理（例: ufw allow OpenSSH）
            ParseApplicationName(command, rule);
            
            // デバッグログ出力（IDSと同じ形式）
            Debug.Log($"Parsed Firewall Rule: Action={rule.Action}, Direction={rule.Direction}, Protocol={rule.Protocol}, FromIp={rule.FromIp}, FromPort={rule.FromPort}, ToIp={rule.ToIp}, ToPort={rule.ToPort}");
            if (!string.IsNullOrEmpty(rule.Interface))
            {
                Debug.Log($"Interface: {rule.Interface}");
            }
            if (!string.IsNullOrEmpty(rule.App))
            {
                Debug.Log($"App: {rule.App}");
            }
            if (rule.Log)
            {
                Debug.Log($"Logging: enabled");
            }
            foreach (var option in rule.Options)
            {
                Debug.Log($"Option: {option.Key} = {option.Value}");
            }
            
            return rule;
        }
        
        // 一般的なサービス名を処理
        private static void ParseCommonServices(string command, FilterRule rule)
        {
            var services = new Dictionary<string, (string port, string protocol)>
            {
                {"ssh", ("22", "tcp")},
                {"http", ("80", "tcp")},
                {"https", ("443", "tcp")},
                {"ftp", ("21", "tcp")},
                {"smtp", ("25", "tcp")},
                {"dns", ("53", "udp")},
                {"ntp", ("123", "udp")},
                {"mysql", ("3306", "tcp")},
                {"postgresql", ("5432", "tcp")},
                {"mongodb", ("27017", "tcp")}
            };
            
            foreach (var service in services)
            {
                if (Regex.IsMatch(command, $@"\b{service.Key}\b", RegexOptions.IgnoreCase))
                {
                    if (rule.ToPort == "any")
                        rule.ToPort = service.Value.port;
                    if (rule.Protocol == "any")
                        rule.Protocol = service.Value.protocol;
                    break;
                }
            }
        }
        
        // アプリケーション名を処理
        private static void ParseApplicationName(string command, FilterRule rule)
        {
            // アクションの後にあるアプリケーション名を検出
            var actionPattern = @"\b(allow|deny|reject|limit)\s+([A-Za-z][A-Za-z0-9]*)\b";
            var appMatch = Regex.Match(command, actionPattern, RegexOptions.IgnoreCase);
            
            if (appMatch.Success && string.IsNullOrEmpty(rule.App))
            {
                var appName = appMatch.Groups[2].Value;
                
                // 一般的なキーワードでない場合はアプリケーション名として扱う
                var keywords = new[] { "in", "out", "from", "to", "on", "port", "proto", "any", "anywhere" };
                if (!keywords.Contains(appName.ToLower()))
                {
                    rule.App = appName;
                    Debug.Log($"Detected application name: {appName}");
                    
                    // 既知のアプリケーションに対してポートを設定
                    SetPortForKnownApp(rule);
                }
            }
        }
        
        // 既知のアプリケーションに対してポートを設定
        private static void SetPortForKnownApp(FilterRule rule)
        {
            var appPorts = new Dictionary<string, (string port, string protocol)>
            {
                {"openssh", ("22", "tcp")},
                {"ssh", ("22", "tcp")},
                {"nginx", ("80", "tcp")},
                {"apache", ("80", "tcp")},
                {"apache2", ("80", "tcp")},
                {"mysql", ("3306", "tcp")},
                {"postgresql", ("5432", "tcp")},
                {"mongodb", ("27017", "tcp")}
            };
            
            var appLower = rule.App.ToLower();
            if (appPorts.ContainsKey(appLower))
            {
                if (rule.ToPort == "any")
                    rule.ToPort = appPorts[appLower].port;
                if (rule.Protocol == "any")
                    rule.Protocol = appPorts[appLower].protocol;
                    
                Debug.Log($"Set port {rule.ToPort}/{rule.Protocol} for app {rule.App}");
            }
        }
        
        // コマンドタイプを判定する（Core.csで使用）
        public static string GetCommandType(string command)
        {
            var trimmedCommand = command.Trim().ToLower();
            
            if (trimmedCommand.StartsWith("ufw enable"))
                return "enable";
            else if (trimmedCommand.StartsWith("ufw disable"))
                return "disable";
            else if (trimmedCommand.StartsWith("ufw reset"))
                return "reset";
            else if (trimmedCommand.StartsWith("ufw status"))
                return "status";
            else if (trimmedCommand.StartsWith("ufw default"))
                return "default";
            else if (trimmedCommand.StartsWith("ufw delete"))
                return "delete";
            else if (trimmedCommand.Contains("allow") || trimmedCommand.Contains("deny") || 
                     trimmedCommand.Contains("reject") || trimmedCommand.Contains("limit"))
                return "rule";
            else
                return "unknown";
        }
        
        // ヘルプコマンドかどうかをチェック
        public static bool IsHelpCommand(string command)
        {
            return command.Trim().ToLower().Contains("help") || 
                   command.Trim().ToLower().Contains("-h") || 
                   command.Trim().ToLower().Contains("--help");
        }
    }
}
