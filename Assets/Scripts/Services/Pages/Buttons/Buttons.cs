

using UnityEngine;

class Buttons : MonoBehaviour
{
    public GameObject logAnalysisCanvas;
    public GameObject idsCanvas;
    public GameObject boardCanvas;

    public void OnLogAnalysisButtonClick()
    {
        if (logAnalysisCanvas != null)
        {
            bool isActive = logAnalysisCanvas.activeSelf;
            logAnalysisCanvas.SetActive(!isActive);
        }
    }
    public void OnIDSButtonClick()
    {
        if (idsCanvas != null)
        {
            bool isActive = idsCanvas.activeSelf;
            idsCanvas.SetActive(!isActive);
        }
    }
    public void OnBoardButtonClick()
    {
        if (boardCanvas != null)
        {
            bool isActive = boardCanvas.activeSelf;
            boardCanvas.SetActive(!isActive);
        }
    }
}