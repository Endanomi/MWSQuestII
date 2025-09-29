using UnityEngine;

public class BoardUIController : MonoBehaviour
{
    public GameObject boardCanvas;

    void Update()
    {
        // Escキーで閉じる
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBoard();
        }
    }

    // ボタンから呼び出す用
    public void CloseBoard()
    {
        boardCanvas.SetActive(false);
        Time.timeScale = 1f; // ゲーム再開
    }

    // もし別のボタンや処理で開きたい場合用
    public void OpenBoard()
    {
        boardCanvas.SetActive(true);
        Time.timeScale = 0f; // ゲーム停止
    }
}
