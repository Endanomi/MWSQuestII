using UnityEngine;

public class BoardUIController : MonoBehaviour
{
    public GameObject boardCanvas;

    public Spawn spawn;

    private bool firstClose = false;

    void Start()
    {
        boardCanvas.SetActive(true);
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
        if (!firstClose)
        {
            firstClose = true;
            // Additional logic for the first close can be added here
            spawn.StartSpawn();
        }
    }

    public void OpenBoard()
    {
        boardCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
}
