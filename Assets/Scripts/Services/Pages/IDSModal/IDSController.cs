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
                    case "Source":
                        textComp.text = filterRules[i].Source;
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
                        textComp.text = filterRules[i].MaxItemSize;
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
