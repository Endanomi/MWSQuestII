using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BoardLoader : MonoBehaviour
{
    public TextMeshProUGUI commentsText;
    public ScrollRect scrollRect;
    public int stageNumber = 1;
    private bool initialized = false;  // ← 追加

    void Start()
    {
        if (scrollRect == null)
            scrollRect = GetComponentInParent<ScrollRect>();

        StartCoroutine(LoadBoardCoroutine(stageNumber));
    }

    private IEnumerator LoadBoardCoroutine(int stageNum)
    {
        yield return null; // 1フレーム待機

        TextAsset jsonFile = Resources.Load<TextAsset>($"Boards/Stage{stageNum}");
        if (jsonFile == null)
        {
            commentsText.text = $"Stage{stageNum} の掲示板データが見つかりません";
            yield break;
        }

        Complaint[] data = JsonHelper.FromJson<Complaint>(jsonFile.text);

        string display = "";
        foreach (var c in data)
        {
            display += $"【{c.name}】\n{c.detail}\n\n";
        }

        commentsText.text = display;

        // 初回のみスクロール位置をトップに戻す
        if (!initialized && scrollRect != null)
        {
            yield return new WaitForEndOfFrame();
            scrollRect.verticalNormalizedPosition = 1f;
            initialized = true; // ← これで以後リセットしない
        }
    }
}
