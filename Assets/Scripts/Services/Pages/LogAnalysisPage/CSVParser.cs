using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CSVParser : MonoBehaviour
{
    public static List<Visitor> visitorList = new List<Visitor>();

    void Start()
    {
        LoadCSV();

        foreach (var item in visitorList)
        {
            Debug.Log($"名前: {item.name}, 目的地: {item.dst}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            visitorList.Add(newVisitor);
        }
    }
}
