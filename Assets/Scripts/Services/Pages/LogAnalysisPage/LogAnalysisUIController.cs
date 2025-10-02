using UnityEngine;

public class LogAnalysisUIController : MonoBehaviour
{
    public GameObject LogAnalysisCanvas;

    void Start()
    {
        LogAnalysisCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OpenBoard();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBoard();
        }
    }

    public void OpenBoard()
    {
        if (!LogAnalysisCanvas.activeSelf)
        LogAnalysisCanvas.SetActive(true);
    }

    public void CloseBoard()
    {
        LogAnalysisCanvas.SetActive(false);
    }
}
