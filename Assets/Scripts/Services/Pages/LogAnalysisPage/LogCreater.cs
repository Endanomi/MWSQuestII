using UnityEngine;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework;
using UnityEngine.UI;

public class LogCreater : MonoBehaviour
{
    public GameObject logObject;
    public Transform parent;
    private List<Visitor> visitors;
    private int counter;

    public static LogCreater instance;

    public ScrollRect scrollRect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var parser = new CSVParser();
        parser.LoadCSV();
        visitors = parser.VisitorList;
        LogCreate(visitors);
    }

    public void LogCreate(List<Visitor> list)
    {
        counter = 0;

        for (int i = 0; i < list.Count; i++)
        {
            GameObject newLog = Instantiate(logObject, parent);
            SetLogValues(newLog, list, i);
            counter += 1;
        }

        scrollRect.verticalNormalizedPosition = 1f;
    }

    void SetLogValues(GameObject newLog, List<Visitor> list, int i)
    {
        GameObject nameobj = newLog.transform.GetChild(0).gameObject;
        nameobj.GetComponent<TextMeshProUGUI>().text = list[i].name;

        GameObject actionobj = newLog.transform.GetChild(1).gameObject;
        actionobj.GetComponent<TextMeshProUGUI>().text = "pass";

        GameObject srcobj = newLog.transform.GetChild(2).gameObject;
        srcobj.GetComponent<TextMeshProUGUI>().text = list[i].src;

        GameObject dstobj = newLog.transform.GetChild(3).gameObject;
        dstobj.GetComponent<TextMeshProUGUI>().text = list[i].dst;

        GameObject jobobj = newLog.transform.GetChild(4).gameObject;
        jobobj.GetComponent<TextMeshProUGUI>().text = list[i].job;

        GameObject sizeobj = newLog.transform.GetChild(5).gameObject;
        sizeobj.GetComponent<TextMeshProUGUI>().text = list[i].size;

        GameObject itemobj = newLog.transform.GetChild(6).gameObject;
        itemobj.GetComponent<TextMeshProUGUI>().text = list[i].item;
    }
}
