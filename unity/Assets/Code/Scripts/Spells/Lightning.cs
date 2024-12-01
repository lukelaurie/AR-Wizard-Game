using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    private float damageAmount;
    private bool hasDamagedBoss = false;

    void Start()
    {
        Destroy(gameObject, 0.2f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(TagManager.Boss) || hasDamagedBoss)
        {
            return;
        }

        Transform collidedObjectTransform = collision.transform;

        if (collidedObjectTransform.parent == null)
        {
            Debug.LogError("colided boss object parent is null");
            return;
        }

        GameObject parentObject = collidedObjectTransform.parent.gameObject;

        if (parentObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.TakeDamage(damageAmount);
            hasDamagedBoss = true;
        }
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}