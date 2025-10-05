using UnityEngine;
using System.Collections;
using Unity.Collections;

public class Spawn : MonoBehaviour
{
    public GameObject playerPrefab; // プレイヤーのプレハブ

    private void Start()
    {
        PropertiesCreator propertiesCreator = new PropertiesCreator();
        // 1秒ごとにSpawnPlayerメソッドを呼び出す
        InvokeRepeating("SpawnPlayer", 1.0f, 1.0f);
    }

    private void SpawnPlayer()
    {
        float x = -30.0f;
        // yの値は-3.0から3.0の間でランダムに設定
        float y = Random.Range(-7.0f, 1.0f);
        float z = 0.0f;
        Vector3 position = new Vector3(x, y, z);
        // 指定した位置にプレイヤーを生成する
        Instantiate(playerPrefab, position, Quaternion.identity);
    }
}