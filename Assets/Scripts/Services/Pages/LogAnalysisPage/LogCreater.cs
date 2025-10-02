using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LogCreater : MonoBehaviour
{
    public GameObject logObject;
    public Transform parent;
    public RectTransform parentRect;
    private List<Visitor> visitors;
    private int counter = 0;
    private float height;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InvokeRepeating("logCreate", 0, 1);
        height = logObject.GetComponent<RectTransform>().sizeDelta.y;
        visitors = CSVParser.visitorList;
        LogCreate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LogCreate()
    {
        for (int i = 0; i < visitors.Count; i++)
        {
            GameObject newLog = Instantiate(logObject, parent);
            newLog.transform.position = new Vector3(newLog.transform.position.x, newLog.transform.position.y - height * counter / 1.5f, newLog.transform.position.z);

            SetLogValues(newLog, i);

            parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, parentRect.sizeDelta.y + height * 1.65f);

            counter += 1;
        }
    }

    void SetLogValues(GameObject newLog, int i)
    {
        GameObject nameobj = newLog.transform.GetChild(0).gameObject;
        nameobj.GetComponent<TextMeshProUGUI>().text = visitors[i].name;

        GameObject srcobj = newLog.transform.GetChild(2).gameObject;
        srcobj.GetComponent<TextMeshProUGUI>().text = visitors[i].src;

        GameObject dstobj = newLog.transform.GetChild(3).gameObject;
        dstobj.GetComponent<TextMeshProUGUI>().text = visitors[i].dst;

        GameObject jobobj = newLog.transform.GetChild(4).gameObject;
        jobobj.GetComponent<TextMeshProUGUI>().text = visitors[i].job;

        GameObject sizeobj = newLog.transform.GetChild(5).gameObject;
        sizeobj.GetComponent<TextMeshProUGUI>().text = visitors[i].size;

        GameObject itemobj = newLog.transform.GetChild(6).gameObject;
        itemobj.GetComponent<TextMeshProUGUI>().text = visitors[i].item;
    }
}
