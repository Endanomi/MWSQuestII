using UnityEngine;
using TMPro;

public class IDSUIController : MonoBehaviour
{
    public GameObject IDSCanvas;

    public GameObject HelpCanvas;

    void Start()
    {
        IDSCanvas.SetActive(false);
        HelpCanvas.SetActive(false);
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
        if (!IDSCanvas.activeSelf)
            IDSCanvas.SetActive(true);
    }

    public void CloseBoard()
    {
        IDSCanvas.SetActive(false);
        HelpCanvas.SetActive(false);
    }

    public void OpenHelp()
    {
        HelpCanvas.SetActive(true);
    }

    public void CloseHelp()
    {
        HelpCanvas.SetActive(false);
    }
}
