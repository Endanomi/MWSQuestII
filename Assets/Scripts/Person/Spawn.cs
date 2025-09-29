using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{
    public GameObject playerPrefab; // プレイヤーのプレハブ
    public Transform spawnPoint; // プレイヤーを生成する位置

    private void Start()
    {
        // 1秒ごとにSpawnPlayerメソッドを呼び出す
        InvokeRepeating("SpawnPlayer", 1f, 1f);
    }

    private void SpawnPlayer()
    {
        // 指定した位置にプレイヤーを生成する
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}