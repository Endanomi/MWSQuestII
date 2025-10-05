using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropertiesCreator", menuName = "Services/PropertiesCreator")]
public class PropertiesCreator : ScriptableObject
{
    private List<PersonProperties> personPropertiesList = new List<PersonProperties>();

    private int currentIndex = 0;

    public bool isFinished;


    void OnEnable()
    {
        Debug.Log($"[PropertiesCreator] OnEnable called at {DateTime.Now}");

        try
        {
            var CsvParser = new CSVParser();
            CsvParser.LoadCSV();
            personPropertiesList = CsvParser.VisitorList;
            Debug.Log($"[PropertiesCreator] Loaded {personPropertiesList.Count} entries");
        }
        catch (Exception e)
        {
            Debug.LogError($"[PropertiesCreator] Exception during LoadCSV: {e}");
        }
    }


    public PersonProperties GetPersonProperties()
    {
        if (currentIndex < personPropertiesList.Count)
        {
            return personPropertiesList[currentIndex++];
        }
        else
        {
            // リストの終わりに達した場合の処理
            return null;
        }
    }
}