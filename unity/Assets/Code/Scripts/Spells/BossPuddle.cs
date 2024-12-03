using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossPuddle : MonoBehaviour, IBossSpell
{
    private float damageAmount;
    private float lifetime = 4f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag(TagManager.Player))
        {
            return;
        }
        var networkObject = other.gameObject.GetComponent<NetworkObject>();
        if (networkObject.IsOwner)
        {
            PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
            playerData.PlayerTakeDamage(5); //TODO
            Destroy(gameObject);
        }
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}