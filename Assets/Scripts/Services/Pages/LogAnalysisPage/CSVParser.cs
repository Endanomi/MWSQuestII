using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVParser
{
    public List<PersonProperties> VisitorList = new List<PersonProperties>();

    // CSV 1行を正しく分割する正規表現
    private static readonly Regex csvSplitRegex = new Regex(
        @"
        (?:^|,)              # 行頭またはカンマの後ろに続く
        (?:                  # グループ開始
            ""([^""]*)""     # ダブルクオートで囲まれた場合（中身をキャプチャ）
            |                # もしくは
            ([^,""\r\n]*)    # クオートなしの通常フィールド
        )
        ",
        RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace
    );

    public void LoadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("LogData/Log_raw");

        if (csvFile == null)
        {
            Debug.LogError("CSV file not found at Resources/LogData/Log_raw.csv");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // ヘッダ行をスキップ
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            // 正規表現で行を分割
            var matches = csvSplitRegex.Matches(line);
            List<string> values = new List<string>();
            foreach (Match m in matches)
            {
                // m.Groups[1] = クオート囲み, m.Groups[2] = 通常フィールド
                string value = m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value;
                values.Add(value);
            }

            try
            {
                PersonProperties newVisitor = new PersonProperties(values.ToArray());
                VisitorList.Add(newVisitor);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CSVParser] Error parsing line {i}: {e.Message}");
            }
        }

        Debug.Log($"Loaded {VisitorList.Count} records.");
    }
}
