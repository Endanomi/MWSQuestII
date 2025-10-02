using UnityEngine;

public class GameToBoard : MonoBehaviour
{
    [SerializeField] private GameObject boardCanvas;

    void Start()
    {
        boardCanvas.SetActive(false);
    }

    public void OnClickBoardButton()
    {
        boardCanvas.SetActive(true);
    }

    public void OnClickCloseButton()
    {
        boardCanvas.SetActive(false);
    }
}
