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
    }
}
