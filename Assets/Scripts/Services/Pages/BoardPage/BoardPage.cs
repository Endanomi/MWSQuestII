using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoardPage : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private GameObject paperPrefab;

    [Header("Detail UI")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private TMP_Text detailNameText;   // 名前
    [SerializeField] private TMP_Text detailBodyText;   // 本文
    [SerializeField] private TMP_Text detailDetailText; // 詳細

    [Header("Board Settings")]
    [SerializeField] private int maxPapers = 10; // 最大紙枚数

    void Start()
    {
        LoadAndDisplay();
        detailPanel.SetActive(false); // 最初は非表示
    }

    void LoadAndDisplay()
    {
        string path = Application.dataPath + "/Scripts/Services/Pages/BoardPage/complaints.json";
        string json = File.ReadAllText(path);

        ComplaintData data = JsonUtility.FromJson<ComplaintData>(json);

        int count = 0;
        foreach (var item in data.complaints)
        {
            if (count >= maxPapers) break; // 10枚まででストップ

            GameObject paper = Instantiate(paperPrefab, container);

            TMP_Text nameText = paper.transform.Find("NameText").GetComponent<TMP_Text>();
            TMP_Text bodyText = paper.transform.Find("BodyText").GetComponent<TMP_Text>();

            nameText.text = item.name;
            bodyText.text = item.text;

            // ボタン化してクリック時に詳細を表示
            Button button = paper.AddComponent<Button>();
            button.onClick.AddListener(() => ShowDetail(item));

            count++;
        }
    }

    void ShowDetail(ComplaintItem item)
    {
        detailPanel.SetActive(true);

        detailNameText.text = item.name;
        detailBodyText.text = item.text;
        detailDetailText.text = item.detail;
    }

    public void HideDetail()
    {
        detailPanel.SetActive(false);
    }
}
