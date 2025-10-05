using UnityEngine;
using TMPro;

public class LogAnalysisUIController : MonoBehaviour
{
    public GameObject LogAnalysisCanvas;

    void Start()
    {
        LogAnalysisCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBoard();
        }
    }

    public void OpenBoard()
    {
        if (!LogAnalysisCanvas.activeSelf)
        LogAnalysisCanvas.SetActive(true);

        GameObject keyobj = GameObject.Find("InputText-LogAnalysis");
        keyobj.GetComponent<TextMeshProUGUI>().text = "";
    }

    public void CloseBoard()
    {
        LogAnalysisCanvas.SetActive(false);
    }
}
