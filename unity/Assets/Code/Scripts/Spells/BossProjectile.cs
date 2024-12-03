using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private float damageAmount;
    private float lifetime = 4f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("here");
        if (!collision.gameObject.CompareTag(TagManager.Player))
        {
            // Instantiate(puddle, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        var networkObject = collision.gameObject.GetComponent<NetworkObject>();
        if (networkObject.IsOwner)
        {
            PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
            playerData.PlayerTakeDamage(50); //TODO
            Destroy(gameObject);
        }
    }


    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}