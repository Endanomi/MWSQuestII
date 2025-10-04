using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PageManager : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPage = 0;

    public Button nextButton;
    public Button prevButton;

    public TextMeshProUGUI nextButtonText;
    public TextMeshProUGUI prevButtonText;

    public GameObject confirmPopup;

    private bool isPopupActive = false;

    void Start()
    {
        ShowPage(currentPage);

        if (confirmPopup != null && confirmPopup.activeSelf)
        {
            confirmPopup.SetActive(false);
        }
    }

    void Update()
    {
        if (isPopupActive) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return))
        {
            OnNextButton();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Backspace))
        {
            OnPrevButton();
        }
    }

    private void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }

        if (index == 0)
        {
            prevButton.gameObject.SetActive(false);
        }
        else
        {
            prevButton.gameObject.SetActive(true);
            prevButtonText.text = "← 前へ";
        }

        if (index == pages.Length - 1)
        {
            nextButtonText.text = "スタート";
        }
        else
        {
            nextButtonText.text = "次へ →";
        }
    }

    public void OnNextButton()
    {
        if (isPopupActive) return;

        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
        else
        {
            if (confirmPopup != null)
            {
                confirmPopup.SetActive(true);
                isPopupActive = true;
            }
        }
    }

    public void OnPrevButton()
    {
        if (isPopupActive) return;

        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
        else
        {
            SceneManager.LoadScene("ResultScene");
        }
    }

    public void OnConfirmYes()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void OnConfirmNo()
    {
        if (confirmPopup != null)
        {
            confirmPopup.SetActive(false);
            isPopupActive = false;
        }
    }
}
