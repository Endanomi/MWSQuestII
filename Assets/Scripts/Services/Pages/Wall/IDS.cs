using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.IDS;
using System.Collections.Generic;

public class IDS : MonoBehaviour
{
    [Header("IDS Settings")]
    public IDSEmulator idsEmulator;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
        // 当たったオブジェクトのMonoBehaviour Personを取得する
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

                Debug.Log($"{person.name} は pass 状態のため衝突を無視します。");
            }
        }
    }
}
