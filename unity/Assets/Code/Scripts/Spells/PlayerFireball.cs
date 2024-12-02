using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerFireball : MonoBehaviour, IBossSpell
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
        if (!collision.gameObject.CompareTag(TagManager.Boss))
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
            if (damageAmount != 0)
                enemyComponent.TakeDamage(damageAmount);
                
            Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Enemy component could not be found");
        }
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}
