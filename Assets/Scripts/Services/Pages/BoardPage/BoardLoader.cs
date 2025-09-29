using UnityEngine;
using TMPro;

public class BoardLoader : MonoBehaviour
{
    public TextMeshProUGUI commentsText;
    public int stageNumber = 1;

    void Start()
    {
        LoadBoard(stageNumber);
    }

    public void LoadBoard(int stageNum)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Boards/Stage{stageNum}");
        if (jsonFile == null)
        {
            commentsText.text = $"Stage{stageNum} の掲示板データが見つかりません";
            return;
        }

        Complaint[] data = JsonHelper.FromJson<Complaint>(jsonFile.text);

        string display = "";
        foreach (var c in data)
        {
            display += $"【{c.name}】\n{c.detail}\n\n";
        }

        commentsText.text = display;
    }
}
