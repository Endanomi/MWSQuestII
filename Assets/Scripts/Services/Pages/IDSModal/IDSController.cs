using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.IDS;
using System.Collections.Generic;

public class IDSController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField commandInputField;
    public Transform contentsTransform; // ScrollView/ViewPort/Contents
    public GameObject rowPrefab;

    [Header("IDS Settings")]
    public IDSEmulator idsEmulator;

    public IDSHistory idsHistory;

    void Start()
    {
        // InputFieldのEnterキー入力イベントを設定
        if (commandInputField != null)
        {
            commandInputField.onSubmit.AddListener(OnCommandEntered);
        }
        var filterRules = idsEmulator.GetRules();
        ClearAllRows();
        CreateRows(filterRules);
    }

    void Update()
    {
        // Shift + Enterキーでコマンド実行（InputFieldがフォーカスされている場合）
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            if (commandInputField != null && commandInputField.isFocused)
            {
                ExecuteCommand();
            }
        }

        // Upキーで履歴の前のコマンドを取得
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (commandInputField != null && commandInputField.isFocused)
            {
                string previousCommand = idsHistory.GetPrevious();
                if (previousCommand != null)
                {
                    commandInputField.text = previousCommand;
                    commandInputField.caretPosition = previousCommand.Length; // カーソルを末尾に移動
                }
            }
        }

        // Downキーで履歴の次のコマンドを取得
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (commandInputField != null && commandInputField.isFocused)
            {
                string nextCommand = idsHistory.GetNext();
                if (nextCommand != null)
                {
                    commandInputField.text = nextCommand;
                    commandInputField.caretPosition = nextCommand.Length; // カーソルを末尾に移動
                }
            }
        }
    }


    /// <summary>
    /// InputFieldでEnterが押された時の処理
    /// </summary>
    /// <param name="command">入力されたコマンド</param>
    private void OnCommandEntered(string command)
    {
        // Shift が押されていない場合は何もしない
        if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            // 改行したい場合もあるならここで return だけ
            return;
        }

        ExecuteCommand();
    }

    /// <summary>
    /// コマンドを実行してRowを生成
    /// </summary>
    public void ExecuteCommand()
    {
        if (commandInputField == null || string.IsNullOrEmpty(commandInputField.text))
            return;

        string command = commandInputField.text;

        // 履歴に追加
        idsHistory.Add(command);

        // IDSEmulatorでコマンドを実行
        if (idsEmulator != null)
        {
            idsEmulator.Execute(command);
        }

        var filterRules = idsEmulator.GetRules();

        ClearAllRows();

        CreateRows(filterRules);

        // InputFieldをクリア
        commandInputField.text = "";
        commandInputField.ActivateInputField();
    }

    /// <summary>
    /// Rowプレハブを生成してContentsに追加
    /// </summary>
    /// <param name="command">実行されたコマンド</param>
    private void CreateRows(List<FilterRule> filterRules)
    {
        if (rowPrefab == null || contentsTransform == null)
        {
            Debug.LogWarning("RowPrefab または ContentsTransform が設定されていません");
            return;
        }

        for (int i = 0; i < filterRules.Count; i++)
        {
            // Rowプレハブを生成
            GameObject newRow = Instantiate(rowPrefab, contentsTransform);

            TextMeshProUGUI[] textComponents = newRow.GetComponentsInChildren<TextMeshProUGUI>();

            // Actionという名前のTextコンポーネントを探す
            foreach (var textComp in textComponents)
            {
                switch (textComp.gameObject.name)
                {
                    case "Number":
                        textComp.text = (i + 1).ToString();
                        break;
                    case "Action":
                        textComp.text = filterRules[i].Action;
                        break;
                    case "Departure":
                        textComp.text = filterRules[i].Departure;
                        break;
                    case "Destination":
                        textComp.text = filterRules[i].Destination;
                        break;
                    case "Occupation":
                        textComp.text = filterRules[i].Occupation;
                        break;
                    case "Item":
                        textComp.text = filterRules[i].Item;
                        break;
                    case "MaxItemSize":
                        textComp.text = filterRules[i].MaxItemSize == int.MaxValue ? "any" : filterRules[i].MaxItemSize.ToString();
                        break;
                }
            }
        }
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
