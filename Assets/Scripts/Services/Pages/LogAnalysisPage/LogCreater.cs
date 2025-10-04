using UnityEngine;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework;

public class LogCreater : MonoBehaviour
{
    public GameObject logObject;
    public Transform parent;
    public RectTransform parentRect;
    private List<Visitor> visitors;
    private int counter;
    private float height;

    public static LogCreater instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(DelayStart), 1);
    }

    void DelayStart()
    {
        height = logObject.GetComponent<RectTransform>().sizeDelta.y;
        visitors = CSVParser.visitorList;

        LogCreate(visitors);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogCreate(List<Visitor> list)
    {
        counter = 0;
        parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, 72);

        

        for (int i = 0; i < list.Count; i++)
        {
            GameObject newLog = Instantiate(logObject, parent);
            newLog.transform.position = new Vector3(newLog.transform.position.x, newLog.transform.position.y - height * counter / 1.5f, newLog.transform.position.z);

            SetLogValues(newLog, list, i);

            parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, parentRect.sizeDelta.y + height * 1.65f);

            counter += 1;
        }
    }

    void SetLogValues(GameObject newLog, List<Visitor> list, int i)
    {
        GameObject nameobj = newLog.transform.GetChild(0).gameObject;
        nameobj.GetComponent<TextMeshProUGUI>().text = list[i].name;

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
