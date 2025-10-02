using UnityEngine;

public class LogAnalysisUIController : MonoBehaviour
{
    public GameObject LogAnalysisCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LogAnalysisCanvas.SetActive(false);
    }

    // Update is called once per frame
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
        //Time.timeScale = 1f;
    }

    public void CloseBoard()
    {
        LogAnalysisCanvas.SetActive(false);
    }
}
