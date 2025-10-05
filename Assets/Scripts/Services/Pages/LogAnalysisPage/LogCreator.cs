using UnityEngine;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework;
using UnityEngine.UI;

public class LogCreator : MonoBehaviour
{
    public GameObject logObject;
    public Transform parent;
    private int counter;

    public static LogCreator instance;

    public ScrollRect scrollRect;

    public void AddRow(LogRow logRow)
    {
        GameObject newLog = Instantiate(logObject, parent);
        SetLogValues(newLog, logRow);
    }


    void SetLogValues(GameObject newLog, LogRow logRow)
    {
        GameObject nameobj = newLog.transform.GetChild(0).gameObject;
        nameobj.GetComponent<TextMeshProUGUI>().text = logRow.Name;

        GameObject actionobj = newLog.transform.GetChild(1).gameObject;
        actionobj.GetComponent<TextMeshProUGUI>().text = logRow.Action;

        GameObject srcobj = newLog.transform.GetChild(2).gameObject;
        srcobj.GetComponent<TextMeshProUGUI>().text = logRow.Departure;

        GameObject dstobj = newLog.transform.GetChild(3).gameObject;
        dstobj.GetComponent<TextMeshProUGUI>().text = logRow.Destination;

        GameObject jobobj = newLog.transform.GetChild(4).gameObject;
        jobobj.GetComponent<TextMeshProUGUI>().text = logRow.Occupation;

        GameObject sizeobj = newLog.transform.GetChild(5).gameObject;
        sizeobj.GetComponent<TextMeshProUGUI>().text = logRow.MaxItemSize.ToString();

        GameObject itemobj = newLog.transform.GetChild(6).gameObject;
        itemobj.GetComponent<TextMeshProUGUI>().text = string.Join(", ", logRow.Items);
    }
}
