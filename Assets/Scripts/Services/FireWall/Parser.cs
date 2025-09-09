using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Services.Firewall
{
    // iptables コマンドをパースするクラス
    public class Parser
    {
        // 第1段階：コマンド文字列をConfigにパース
        public static void ParseToConfig(string command, Config config)
        {
            config.SetParsedCommand(command);
            
            // コマンドを正規化（複数スペースを単一スペースに）
            command = Regex.Replace(command.Trim(), @"\s+", " ");

            // ヘルプオプションのチェック
            if (IsHelpCommand(command))
            {
                config.ParsedCommand.Command = "HELP";
                if (config.LogParsing && (Application.isEditor || Debug.isDebugBuild))
                    ShowHelp();
                return;
            }

            // テーブル指定 (-t table)
            ParseTable(command, config.ParsedCommand);
            
            // コマンドとチェーン (-A, -I, -D, etc.)
            ParseCommand(command, config.ParsedCommand);
            
            // ネットワーク関連オプション
            ParseNetworkOptions(command, config.ParsedCommand);
            
            // インターフェース関連
            ParseInterfaceOptions(command, config.ParsedCommand);
            
            // ターゲット/ジャンプ
            ParseTargetOptions(command, config.ParsedCommand);
            
            // マッチオプション
            ParseMatchOptions(command, config.ParsedCommand);
            
            // フラグオプション
            ParseFlagOptions(command, config.ParsedCommand);
            
            // その他のオプション
            ParseMiscOptions(command, config.ParsedCommand);

            // Debug出力
            if (config.LogParsing && (Application.isEditor || Debug.isDebugBuild))
                LogParsedConfig(config.ParsedCommand);
        }
        
        // 第2段階：ConfigからFilterRuleを作成
        public static FilterRule CreateFilterRuleFromConfig(Config config)
        {
            return config.CreateFilterRule();
        }
        
        // 完全なパースとルール作成（2段階を組み合わせ）
        public static FilterRule ParseWithConfig(string command, Config config)
        {
            ParseToConfig(command, config);
            return CreateFilterRuleFromConfig(config);
        }
        
        // 従来の互換性メソッド（デフォルトConfigを使用）
        public static FilterRule Parse(string command)
        {
            var config = new Config();
            return ParseWithConfig(command, config);
        }
        
        // ヘルプコマンドかどうかをチェック
        private static bool IsHelpCommand(string command)
        {
            return Regex.IsMatch(command, @"(?:^|\s)(-h|--help)(?:\s|$)", RegexOptions.IgnoreCase) ||
                   command.Trim().Equals("iptables -h", System.StringComparison.OrdinalIgnoreCase) ||
                   command.Trim().Equals("iptables --help", System.StringComparison.OrdinalIgnoreCase);
        }
        
        // ヘルプメッセージを表示
        private static void ShowHelp()
        {
            Debug.Log(@"iptables v1.8.10 (nf_tables)

Usage: iptables -[ACD] chain rule-specification [options]
       iptables -I chain [rulenum] rule-specification [options]
       iptables -R chain rulenum rule-specification [options]
       iptables -D chain rulenum [options]
       iptables -[LS] [chain [rulenum]] [options]
       iptables -[FZ] [chain] [options]
       iptables -[NX] chain
       iptables -E old-chain-name new-chain-name
       iptables -P chain target [options]
       iptables -h (print this help information)

Commands:
Either long or short options are allowed.
  --append  -A chain            Append to chain
  --check   -C chain            Check for the existence of a rule
  --delete  -D chain            Delete matching rule from chain
  --delete  -D chain rulenum
                                Delete rule rulenum (1 = first) from chain
  --insert  -I chain [rulenum]
                                Insert in chain as rulenum (default 1=first)
  --replace -R chain rulenum
                                Replace rule rulenum (1 = first) in chain
  --list    -L [chain [rulenum]]
                                List the rules in a chain or all chains
  --list-rules -S [chain [rulenum]]
                                Print the rules in a chain or all chains
  --flush   -F [chain]          Delete all rules in  chain or all chains
  --zero    -Z [chain [rulenum]]
                                Zero counters in chain or all chains
  --new     -N chain            Create a new user-defined chain
  --delete-chain
            -X [chain]          Delete a user-defined chain
  --policy  -P chain target
                                Change policy on chain to target
  --rename-chain
            -E old-chain new-chain
                                Change chain name, (moving any references)

Options:
    --ipv4      -4              Nothing (line is ignored by ip6tables-restore)
    --ipv6      -6              Error (line is ignored by iptables-restore)
[!] --protocol  -p proto        protocol: by number or name, eg. `tcp'
[!] --source    -s address[/mask][...]
                                source specification
[!] --destination -d address[/mask][...]
                                destination specification
[!] --in-interface -i input name[+]
                                network interface name ([+] for wildcard)
 --jump -j target
                                target for rule (may load target extension)
  --goto      -g chain
                               jump to chain with no return
  --match       -m match
                                extended match (may load extension)
  --numeric     -n              numeric output of addresses and ports
[!] --out-interface -o output name[+]
                                network interface name ([+] for wildcard)
  --table       -t table        table to manipulate (default: `filter')
  --verbose     -v              verbose mode
  --wait        -w [seconds]    maximum wait to acquire xtables lock before give up
  --line-numbers                print line numbers when listing
  --exact       -x              expand numbers (display exact values)
[!] --fragment  -f              match second or further fragments only
  --modprobe=<command>          try to insert modules using this command
  --set-counters -c PKTS BYTES  set the counter during insert/append
[!] --version   -V              print package version.");
        }

        private static void ParseTable(string command, Config.ParsedCommandInfo parsedInfo)
        {
            var tableMatch = Regex.Match(command, @"(?:^|\s)-t\s+(?<table>\w+)");
            if (tableMatch.Success)
                parsedInfo.Table = tableMatch.Groups["table"].Value;
        }

        private static void ParseCommand(string command, Config.ParsedCommandInfo parsedInfo)
        {
            // コマンドとチェーンをパース
            var cmdMatch = Regex.Match(command, @"(?:^|\s)-(?<cmd>[A-Z]+)\s+(?<chain>\w+)(?:\s+(?<rulenum>\d+))?");
            if (cmdMatch.Success)
            {
                parsedInfo.Command = cmdMatch.Groups["cmd"].Value;
                parsedInfo.Chain = cmdMatch.Groups["chain"].Value;
                
                // ルール番号（-I, -R, -D で使用）
                if (cmdMatch.Groups["rulenum"].Success)
                {
                    if (int.TryParse(cmdMatch.Groups["rulenum"].Value, out int ruleNum))
                        parsedInfo.RuleNumber = ruleNum;
                }
            }
            
            // -I chain [rulenum] の形式もサポート
            var insertMatch = Regex.Match(command, @"(?:^|\s)-I\s+(?<chain>\w+)(?:\s+(?<rulenum>\d+))?");
            if (insertMatch.Success)
            {
                parsedInfo.Command = "I";
                parsedInfo.Chain = insertMatch.Groups["chain"].Value;
                if (insertMatch.Groups["rulenum"].Success)
                {
                    if (int.TryParse(insertMatch.Groups["rulenum"].Value, out int ruleNum))
                        parsedInfo.RuleNumber = ruleNum;
                }
            }
        }

        private static void ParseNetworkOptions(string command, Config.ParsedCommandInfo parsedInfo)
        {
            // プロトコル (-p proto)
            var protoPattern = @"(?<not>!)?\s*-p\s+(?<proto>\w+)";
            var protoMatch = Regex.Match(command, protoPattern);
            if (protoMatch.Success)
            {
                parsedInfo.Protocol = protoMatch.Groups["proto"].Value;
                parsedInfo.NotProtocol = protoMatch.Groups["not"].Success;
            }

            // ソース IP (-s address[/mask])
            var srcPattern = @"(?<not>!)?\s*-s\s+(?<src>[^\s]+)";
            var srcMatch = Regex.Match(command, srcPattern);
            if (srcMatch.Success)
            {
                parsedInfo.SrcIp = srcMatch.Groups["src"].Value;
                parsedInfo.NotSource = srcMatch.Groups["not"].Success;
            }

            // 宛先 IP (-d address[/mask])
            var dstPattern = @"(?<not>!)?\s*-d\s+(?<dst>[^\s]+)";
            var dstMatch = Regex.Match(command, dstPattern);
            if (dstMatch.Success)
            {
                parsedInfo.DstIp = dstMatch.Groups["dst"].Value;
                parsedInfo.NotDestination = dstMatch.Groups["not"].Success;
            }

            // ソースポート (--sport port)
            var sportMatch = Regex.Match(command, @"--sport\s+(?<sport>\d+(?::\d+)?)");
            if (sportMatch.Success)
                parsedInfo.SrcPort = sportMatch.Groups["sport"].Value;

            // 宛先ポート (--dport port)
            var dportMatch = Regex.Match(command, @"--dport\s+(?<dport>\d+(?::\d+)?)");
            if (dportMatch.Success)
                parsedInfo.DstPort = dportMatch.Groups["dport"].Value;
        }

        private static void ParseInterfaceOptions(string command, Config.ParsedCommandInfo parsedInfo)
        {
            // 入力インターフェース (-i interface)
            var inIfPattern = @"(?<not>!)?\s*-i\s+(?<iface>[^\s]+)";
            var inIfMatch = Regex.Match(command, inIfPattern);
            if (inIfMatch.Success)
            {
                parsedInfo.InInterface = inIfMatch.Groups["iface"].Value;
                parsedInfo.NotInInterface = inIfMatch.Groups["not"].Success;
            }

            // 出力インターフェース (-o interface)
            var outIfPattern = @"(?<not>!)?\s*-o\s+(?<iface>[^\s]+)";
            var outIfMatch = Regex.Match(command, outIfPattern);
            if (outIfMatch.Success)
            {
                parsedInfo.OutInterface = outIfMatch.Groups["iface"].Value;
                parsedInfo.NotOutInterface = outIfMatch.Groups["not"].Success;
            }
        }

        private static void ParseTargetOptions(string command, Config.ParsedCommandInfo parsedInfo)
        {
            // ジャンプターゲット (-j target)
            var jumpMatch = Regex.Match(command, @"-j\s+(?<jump>\w+)");
            if (jumpMatch.Success)
                parsedInfo.Jump = jumpMatch.Groups["jump"].Value;

            // Goto (-g chain)
            var gotoMatch = Regex.Match(command, @"-g\s+(?<goto>\w+)");
            if (gotoMatch.Success)
                parsedInfo.Goto = gotoMatch.Groups["goto"].Value;
        }

        private static void ParseMatchOptions(string command, Config.ParsedCommandInfo parsedInfo)
        {
            // マッチオプション (-m match)
            var matchPattern = @"-m\s+(?<match>\w+)";
            foreach (Match match in Regex.Matches(command, matchPattern))
            {
                parsedInfo.Matches.Add(match.Groups["match"].Value);
            }
        }

        private static void ParseFlagOptions(string command, Config.ParsedCommandInfo parsedInfo)
        {
            // フラグメント (-f)
            var fragmentPattern = @"(?<not>!)?\s*-f";
            var fragmentMatch = Regex.Match(command, fragmentPattern);
            if (fragmentMatch.Success)
            {
                parsedInfo.Fragment = true;
                parsedInfo.NotFragment = fragmentMatch.Groups["not"].Success;
            }

            // 数値出力 (-n)
            if (Regex.IsMatch(command, @"(?:^|\s)-n(?:\s|$)"))
                parsedInfo.Numeric = true;

            // 詳細モード (-v)
            if (Regex.IsMatch(command, @"(?:^|\s)-v(?:\s|$)"))
                parsedInfo.Verbose = true;

            // 正確な値 (-x)
            if (Regex.IsMatch(command, @"(?:^|\s)-x(?:\s|$)"))
                parsedInfo.Exact = true;

            // 行番号 (--line-numbers)
            if (Regex.IsMatch(command, @"--line-numbers"))
                parsedInfo.LineNumbers = true;

            // IPv4 (-4)
            if (Regex.IsMatch(command, @"(?:^|\s)-4(?:\s|$)"))
                parsedInfo.IPv4 = true;

            // IPv6 (-6)
            if (Regex.IsMatch(command, @"(?:^|\s)-6(?:\s|$)"))
                parsedInfo.IPv6 = true;
        }

        private static void ParseMiscOptions(string command, Config.ParsedCommandInfo parsedInfo)
        {
            // 待機時間 (-w [seconds])
            var waitMatch = Regex.Match(command, @"-w\s+(?<wait>\d+)");
            if (waitMatch.Success)
            {
                if (int.TryParse(waitMatch.Groups["wait"].Value, out int wait))
                    parsedInfo.Wait = wait;
            }

            // カウンター設定 (-c PKTS BYTES)
            var counterMatch = Regex.Match(command, @"-c\s+(?<pkts>\d+)\s+(?<bytes>\d+)");
            if (counterMatch.Success)
            {
                if (long.TryParse(counterMatch.Groups["pkts"].Value, out long pkts))
                    parsedInfo.PacketCount = pkts;
                if (long.TryParse(counterMatch.Groups["bytes"].Value, out long bytes))
                    parsedInfo.ByteCount = bytes;
            }

            // モジュールプローブ (--modprobe=command)
            var modprobeMatch = Regex.Match(command, @"--modprobe=(?<cmd>[^\s]+)");
            if (modprobeMatch.Success)
                parsedInfo.ModProbe = modprobeMatch.Groups["cmd"].Value;

            // その他の長いオプション
            var longOptPattern = @"--(?<key>[\w-]+)(?:=(?<value>[^\s]+)|\s+(?<value2>[^\s-][^\s]*))?";
            foreach (Match match in Regex.Matches(command, longOptPattern))
            {
                var key = match.Groups["key"].Value;
                var value = match.Groups["value"].Success ? match.Groups["value"].Value :
                           match.Groups["value2"].Success ? match.Groups["value2"].Value : "";

                // 既に処理済みのオプションはスキップ
                if (IsProcessedOption(key))
                    continue;

                parsedInfo.Options[key] = value;
            }
        }

        private static bool IsProcessedOption(string key)
        {
            var processedOptions = new HashSet<string>
            {
                "sport", "dport", "line-numbers", "modprobe", 
                "to-destination", "to-source", "to-ports"
            };
            return processedOptions.Contains(key);
        }

        private static void LogParsedConfig(Config.ParsedCommandInfo parsedInfo)
        {
            Debug.Log($"Parsed iptables Config:");
            Debug.Log($"  Table={parsedInfo.Table}, Command={parsedInfo.Command}, Chain={parsedInfo.Chain}");
            if (parsedInfo.RuleNumber.HasValue)
                Debug.Log($"  RuleNumber={parsedInfo.RuleNumber}");
            Debug.Log($"  Protocol={parsedInfo.Protocol} (Not: {parsedInfo.NotProtocol})");
            Debug.Log($"  SrcIp={parsedInfo.SrcIp} (Not: {parsedInfo.NotSource}), DstIp={parsedInfo.DstIp} (Not: {parsedInfo.NotDestination})");
            Debug.Log($"  SrcPort={parsedInfo.SrcPort}, DstPort={parsedInfo.DstPort}");
            Debug.Log($"  InInterface={parsedInfo.InInterface} (Not: {parsedInfo.NotInInterface})");
            Debug.Log($"  OutInterface={parsedInfo.OutInterface} (Not: {parsedInfo.NotOutInterface})");
            Debug.Log($"  Jump={parsedInfo.Jump}, Goto={parsedInfo.Goto}");
            
            if (parsedInfo.Matches.Count > 0)
                Debug.Log($"  Matches: {string.Join(", ", parsedInfo.Matches)}");
            
            if (parsedInfo.Fragment)
                Debug.Log($"  Fragment=true (Not: {parsedInfo.NotFragment})");
            
            var flags = new List<string>();
            if (parsedInfo.Numeric) flags.Add("Numeric");
            if (parsedInfo.Verbose) flags.Add("Verbose");
            if (parsedInfo.Exact) flags.Add("Exact");
            if (parsedInfo.LineNumbers) flags.Add("LineNumbers");
            if (parsedInfo.IPv4) flags.Add("IPv4");
            if (parsedInfo.IPv6) flags.Add("IPv6");
            if (flags.Count > 0)
                Debug.Log($"  Flags: {string.Join(", ", flags)}");
            
            if (parsedInfo.Wait.HasValue)
                Debug.Log($"  Wait={parsedInfo.Wait}");
            if (parsedInfo.PacketCount.HasValue)
                Debug.Log($"  PacketCount={parsedInfo.PacketCount}, ByteCount={parsedInfo.ByteCount}");
            if (!string.IsNullOrEmpty(parsedInfo.ModProbe))
                Debug.Log($"  ModProbe={parsedInfo.ModProbe}");
            
            foreach (var option in parsedInfo.Options)
            {
                Debug.Log($"  Option: {option.Key} = {option.Value}");
            }
        }
    }
}
