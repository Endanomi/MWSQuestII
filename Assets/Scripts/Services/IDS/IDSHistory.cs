using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Services.IDS
{
    [CreateAssetMenu(fileName = "IDS History", menuName = "ScriptableObjects/IDS History", order = 2)]
    public class IDSHistory : ScriptableObject
    {
        private List<string> history = new List<string>();

        private int currentIndex = -1;

        public void Add(string command)
        {
            if (!string.IsNullOrWhiteSpace(command))
            {
                history.Add(command);
                currentIndex = history.Count; // 最新のコマンドの次の位置に設定
            }
        }
        public string GetPrevious()
        {
            if (history.Count == 0 || currentIndex <= 0)
            {
                return null; // 履歴がないか、最初のコマンドに到達している場合
            }
            currentIndex--;
            return history[currentIndex];
        }
        public string GetNext()
        {
            if (history.Count == 0 || currentIndex >= history.Count - 1)
            {
                return null; // 履歴がないか、最後のコマンドに到達している場合
            }
            currentIndex++;
            return history[currentIndex];
        }
    }
}