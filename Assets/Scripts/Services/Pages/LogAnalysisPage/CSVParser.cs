using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CSVParser
{
    public List<Visitor> VisitorList = new List<Visitor>();

    public void LoadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>($"LogData/Log_raw");

        string csvText = csvFile.text;
        string[] lines = csvText.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
            {
                continue;
            }

            string[] values = lines[i].Split(',');

            Visitor newVisitor = new Visitor(values);
            VisitorList.Add(newVisitor);
        }
    }
}
