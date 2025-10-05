using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject clearPanel;
    public GameObject failPanel;

    [Header("Clear Panel Texts")]
    public TextMeshProUGUI passPoint;
    public TextMeshProUGUI rejectPoint;
    public TextMeshProUGUI dropPoint;
    public TextMeshProUGUI totalPoint;

    [Header("Fail Panel Texts")]
    public TextMeshProUGUI passPointFail;
    public TextMeshProUGUI rejectPointFail;
    public TextMeshProUGUI dropPointFail;
    public TextMeshProUGUI totalPointFail;

    [Header("Buttons")]
    public Button nextButton;  
    public Button retryButton; 

    [Header("Scorer")]
    public Scorer scorer;

    [Header("Score Threshold")]
    public int clearScoreThreshold = 288;


    public LogCreator logCreator;

    private bool isOpened = false;

    void Update()
    {
        if (!isOpened && scorer.TotalCount >= 300)
        {
            isOpened = true;
            OpenResultBoard();
        } 
    }

    public void OpenResultBoard()
    {
        UpdateScoreUI();

        if (scorer.TotalScore() >= clearScoreThreshold)
        {
            ShowClearPanel();
        }
        else
        {
            ShowFailPanel();
        }

        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextButtonClicked);

        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryButtonClicked);
    }

    void UpdateScoreUI()
    {
        // クリア用
        if (passPoint != null) passPoint.text = $"{scorer.PassScore}人";
        if (rejectPoint != null) rejectPoint.text = $"{scorer.RejectScore}人";
        if (dropPoint != null) dropPoint.text = $"{scorer.DropScore}人";
        if (totalPoint != null) totalPoint.text = $"{scorer.TotalScore()}pt";

        // 失敗用
        if (passPointFail != null) passPointFail.text = $"{scorer.PassScore}人";
        if (rejectPointFail != null) rejectPointFail.text = $"{scorer.RejectScore}人";
        if (dropPointFail != null) dropPointFail.text = $"{scorer.DropScore}人";
        if (totalPointFail != null) totalPointFail.text = $"{scorer.TotalScore()}pt";
    }

    void ShowClearPanel()
    {
        clearPanel.SetActive(true);
        failPanel.SetActive(false);
    }

    void ShowFailPanel()
    {
        clearPanel.SetActive(false);
        failPanel.SetActive(true);
    }

    void OnNextButtonClicked()
    {
        Debug.Log("Next button clicked");
        SceneManager.LoadScene("CommentScene");
    }

    void OnRetryButtonClicked()
    {
        scorer.ResetScores();
        SceneManager.LoadScene("SampleScene");
    }
}
