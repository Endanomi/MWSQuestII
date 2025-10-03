using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] pages;   // �y�[�W�iPanel�j��o�^����z��
    private int currentPage = 0;

    void Start()
    {
        ShowPage(0); // �ŏ���1�y�[�W�ڂ�\��
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);  // ���̃y�[�W�����\��
        }
    }
}
