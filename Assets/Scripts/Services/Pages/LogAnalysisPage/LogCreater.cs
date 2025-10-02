using UnityEngine;

public class LogCreate : MonoBehaviour
{
    public GameObject logObject;
    public Transform parent;
    public RectTransform parentRect;
    private int counter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(logObject, parent);
        InvokeRepeating("logCreate", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void logCreate()
    {
        counter += 1;
        GameObject newLog = Instantiate(logObject, parent);
        newLog.transform.position = new Vector3(newLog.transform.position.x, newLog.transform.position.y - 45 * counter, newLog.transform.position.z);
        parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, parentRect.sizeDelta.y + 90);
    }
}
