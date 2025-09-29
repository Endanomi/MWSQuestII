using UnityEngine;

public class BoardUIController : MonoBehaviour
{
    public GameObject boardCanvas;

    void Update()
    {
        // Esc�L�[�ŕ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBoard();
        }
    }

    // �{�^������Ăяo���p
    public void CloseBoard()
    {
        boardCanvas.SetActive(false);
        Time.timeScale = 1f; // �Q�[���ĊJ
    }

    // �����ʂ̃{�^���⏈���ŊJ�������ꍇ�p
    public void OpenBoard()
    {
        boardCanvas.SetActive(true);
        Time.timeScale = 0f; // �Q�[����~
    }
}
