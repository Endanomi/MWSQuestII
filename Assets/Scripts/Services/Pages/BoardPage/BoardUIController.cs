using UnityEngine;

public class BoardUIController : MonoBehaviour
{
    public GameObject boardCanvas;

    void Start()
    {
        boardCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBoard();
        }
    }

    public void CloseBoard()
    {
        boardCanvas.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void OpenBoard()
    {
        boardCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
}
