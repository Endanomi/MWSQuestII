using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.IDS;

public class IDSController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField commandInputField;
    public Transform contentsTransform; // ScrollView/ViewPort/Contents
    public GameObject rowPrefab;

    [Header("IDS Settings")]
    public IDSEmulator idsEmulator;

    void Start()
    {
        // InputFieldのEnterキー入力イベントを設定
        if (commandInputField != null)
        {
            commandInputField.onEndEdit.AddListener(OnCommandEntered);
        }
    }

    void Update()
    {
        // Enterキーでコマンド実行（InputFieldがフォーカスされている場合）
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (commandInputField != null && commandInputField.isFocused)
            {
                ExecuteCommand();
            }
        }
    }

    /// <summary>
    /// InputFieldでEnterが押された時の処理
    /// </summary>
    /// <param name="command">入力されたコマンド</param>
    private void OnCommandEntered(string command)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ExecuteCommand();
        }
    }

    /// <summary>
    /// コマンドを実行してRowを生成
    /// </summary>
    public void ExecuteCommand()
    {
        if (commandInputField == null || string.IsNullOrEmpty(commandInputField.text))
            return;

        string command = commandInputField.text;
        
        // IDSEmulatorでコマンドを実行
        if (idsEmulator != null)
        {
            idsEmulator.Execute(command);
        }

        // Rowプレハブを生成してContentsに追加
        CreateRow(command);

        // InputFieldをクリア
        commandInputField.text = "";
        commandInputField.ActivateInputField();
    }

    /// <summary>
    /// Rowプレハブを生成してContentsに追加
    /// </summary>
    /// <param name="command">実行されたコマンド</param>
    private void CreateRow(string command)
    {
        if (rowPrefab == null || contentsTransform == null)
        {
            Debug.LogWarning("RowPrefab または ContentsTransform が設定されていません");
            return;
        }

        // Rowプレハブを生成
        GameObject newRow = Instantiate(rowPrefab, contentsTransform);
        
        // Rowにコマンドテキストを設定（Rowプレハブにテキストコンポーネントがある場合）
        Text rowText = newRow.GetComponentInChildren<Text>();
        if (rowText != null)
        {
            rowText.text = command;
        }

        // TextMeshProの場合
        TMPro.TextMeshProUGUI tmpText = newRow.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = command;
        }

        Debug.Log($"Command executed: {command}");
    }

    /// <summary>
    /// 外部からコマンドを実行する場合
    /// </summary>
    /// <param name="command">実行するコマンド</param>
    public void ExecuteCommand(string command)
    {
        if (string.IsNullOrEmpty(command))
            return;

        // IDSEmulatorでコマンドを実行
        if (idsEmulator != null)
        {
            idsEmulator.Execute(command);
        }

        // Rowプレハブを生成してContentsに追加
        CreateRow(command);
    }

    /// <summary>
    /// すべてのRowをクリア
    /// </summary>
    public void ClearAllRows()
    {
        if (contentsTransform == null)
            return;

        for (int i = contentsTransform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(contentsTransform.GetChild(i).gameObject);
        }
    }
}
