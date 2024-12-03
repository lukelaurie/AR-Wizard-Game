using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossPuddle : MonoBehaviour
{
    private float damageAmount;
    private float lifetime;
    private float damageTickTime;
    private float currentTickTime;
    private Vector3 maxSize;


    void Start()
    {
        lifetime = 5f;
        damageTickTime = 0.5f;
        currentTickTime = damageTickTime;
        maxSize = new Vector3(3f, 0f, 3f);

        StartCoroutine(GrowPuddle());
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision other)
    // void OnCollisionStay(Collision other)
    {
        Debug.Log(currentTickTime);
        currentTickTime += Time.deltaTime;
        if (currentTickTime < damageTickTime || !other.gameObject.CompareTag(TagManager.Player))
        {
            return;
        }

        var networkObject = other.gameObject.GetComponent<NetworkObject>();
        if (networkObject.IsOwner)
        {
            PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
            Debug.Log("here");
            playerData.PlayerTakeDamage(5); //TODO
            currentTickTime = 0f;
        }
    }

    private IEnumerator GrowPuddle()
    {
        Vector3 initialSize = transform.localScale;
        float timePassed = 0f; 
        float growthDuration = 2f;

        while (timePassed < growthDuration)
        {
            transform.localScale = Vector3.Lerp(initialSize, maxSize, timePassed / growthDuration);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}