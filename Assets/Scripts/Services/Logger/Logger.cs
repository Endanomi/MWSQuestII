using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


[CreateAssetMenu(fileName = "Logger", menuName = "Services/Logger")]

public class Logger : ScriptableObject
{
    public List<LogRow> LogRows = new List<LogRow>();
    public void Add(string action, PersonProperties personProperties)
    {
        LogRow logRow = new LogRow(action, personProperties);
        LogRows.Add(logRow);
    }
}