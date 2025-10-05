using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.IDS;
using System.Collections.Generic;

public class IDS : MonoBehaviour
{
    [Header("IDS Settings")]
    public IDSEmulator idsEmulator;

    public Logger Logger;
    public LogCreator LogCreator;

    public Scorer scorer;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 当たったオブジェクトに Person コンポーネントがあるか調べる
        Person person = collision.gameObject.GetComponent<Person>();

        if (person != null)
        {
            // person にアクセスして処理を書く
            person.state = idsEmulator.Challenge(person.properties);
            if (person.state == "pass")
            {
                // 自分（このスクリプトがついているオブジェクト）のColliderと
                // 相手（Personがついているオブジェクト）のColliderの衝突を無効にする
                Collider2D myCollider = GetComponent<Collider2D>();
                Collider2D otherCollider = collision.collider;
                Physics2D.IgnoreCollision(myCollider, otherCollider);
            }
            else if (person.state == "reject")
            {
                // 何もしない
            }
            else if (person.state == "drop")
            {
                // x座標を10〜17,y座標を2〜5の範囲でランダムに設定する
                Vector2 pos = person.transform.position;
                pos.x = Random.Range(4.5f, 12f);
                pos.y = Random.Range(5f, 10f);
                person.transform.position = pos;
            }
            scorer.Add(person.state, person.properties);
            Logger.Add(person.state, person.properties);
            LogCreator.AddRow(Logger.LogRows[Logger.LogRows.Count - 1]); 
        }
    }
}
