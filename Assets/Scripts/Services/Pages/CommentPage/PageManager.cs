using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;  // �� �����ǉ��I

public class PageManager : MonoBehaviour
{
    public GameObject[] pages;       // Panel�̔z��
    private int currentPage = 0;

    [Header("UI Buttons")]
    public Button nextButton;
    public Button prevButton;

    [Header("Button Labels (TMP)")]
    public TextMeshProUGUI nextButtonText;  // �� TMP�p�ɕύX
    public TextMeshProUGUI prevButtonText;  // �� TMP�p�ɕύX

    void Start()
    {
        ShowPage(currentPage);
    }

    void Update()
    {
        // ���L�[ �܂��� Enter
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return))
        {
            OnNextButton();
        }

        // ���L�[ �܂��� Backspace
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Backspace))
        {
            OnPrevButton();
        }
    }

    private void ShowPage(int index)
    {
        // �y�[�W�؂�ւ�
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }

        // �ŏ��̃y�[�W�Ȃ�u�O�ցv���u���U���g�ցv
        if (index == 0)
        {
            prevButtonText.text = "���U���g��";
        }
        else
        {
            prevButtonText.text = "�� �O��";
        }

        // �Ō�̃y�[�W�Ȃ�u���ցv���u�X�^�[�g�ցv
        if (index == pages.Length - 1)
        {
            nextButtonText.text = "�X�^�[�g��";
        }
        else
        {
            nextButtonText.text = "���� ��";
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
            // �Ō� �� �X�^�[�g��ʂ�
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
            // �ŏ� �� ���U���g��ʂ�
            SceneManager.LoadScene("ResultScene");
        }
    }
}
