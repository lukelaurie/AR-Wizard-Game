using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossPuddle : MonoBehaviour
{
    [SerializeField] private float puddleDamage;
    [SerializeField] private float puddleWidth; 
    private float lifetime;
    private float damageTickTime;
    private float currentTickTime;
    private Vector3 maxSize;


    void Start()
    {
        lifetime = 5f;
        damageTickTime = 1f;
        currentTickTime = damageTickTime;

        // get a random range on the puddle width 
        float randPuddleWidth = UnityEngine.Random.Range(puddleWidth - 1f, puddleWidth + 2f);
        maxSize = new Vector3(randPuddleWidth, 0f, randPuddleWidth);

        StartCoroutine(GrowPuddle());
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        currentTickTime += Time.deltaTime;
    }

    // void OnCollisionEnter(Collision other)
    void OnCollisionStay(Collision other)
    {
        if (currentTickTime < damageTickTime || !other.gameObject.CompareTag(TagManager.Player))
        {
            return;
        }

        var networkObject = other.gameObject.GetComponent<NetworkObject>();
        if (networkObject.IsOwner)
        {
            PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
            playerData.PlayerTakeDamage(puddleDamage);
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
}