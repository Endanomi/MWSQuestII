using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;  // ← これを追加！

public class PageManager : MonoBehaviour
{
    public GameObject[] pages;       // Panelの配列
    private int currentPage = 0;

    [Header("UI Buttons")]
    public Button nextButton;
    public Button prevButton;

    [Header("Button Labels (TMP)")]
    public TextMeshProUGUI nextButtonText;  // ← TMP用に変更
    public TextMeshProUGUI prevButtonText;  // ← TMP用に変更

    void Start()
    {
        ShowPage(currentPage);
    }

    void Update()
    {
        // →キー または Enter
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return))
        {
            OnNextButton();
        }

        // ←キー または Backspace
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Backspace))
        {
            OnPrevButton();
        }
    }

    private void ShowPage(int index)
    {
        // ページ切り替え
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }

        // 最初のページなら「前へ」→「リザルトへ」
        if (index == 0)
        {
            prevButtonText.text = "リザルトへ";
        }
        else
        {
            prevButtonText.text = "← 前へ";
        }

        // 最後のページなら「次へ」→「スタートへ」
        if (index == pages.Length - 1)
        {
            nextButtonText.text = "スタートへ";
        }
        else
        {
            nextButtonText.text = "次へ →";
        }
    }

    public void OnNextButton()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
        else
        {
            // 最後 → スタート画面へ
            SceneManager.LoadScene("StartScene");
        }
    }

    public void OnPrevButton()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
        else
        {
            // 最初 → リザルト画面へ
            SceneManager.LoadScene("ResultScene");
        }
    }
}
