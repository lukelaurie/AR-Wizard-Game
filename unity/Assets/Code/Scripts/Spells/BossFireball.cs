using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossFireball : MonoBehaviour, IBossSpell
{
    private float damageAmount;
    private float lifetime = 4f;
    [SerializeField] private GameObject explosion;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(TagManager.Player))
        {
            return;
        }
        var networkObject = collision.gameObject.GetComponent<NetworkObject>();
        if (networkObject.IsOwner)
        {
            PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
            playerData.PlayerTakeDamage(2); //TODO
            Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}
