using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float damageAmount;
    private float lifetime = 4f;
    private bool isBossAttack;
    [SerializeField] private GameObject explosion;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(IsCorrectObjCollision(collision));
        if (!IsCorrectObjCollision(collision))
        {
            return;
        }

        Transform collidedObjectTransform = collision.transform;

        if (collidedObjectTransform.parent == null)
        {
            Debug.LogError("colided boss object parent is null");
            return;
        }

        Debug.Log("hitting the player");

        GameObject parentObject = collidedObjectTransform.parent.gameObject;

        if (parentObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.TakeDamage(damageAmount);
            Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Enemy component could not be found");
        }
    }

    private bool IsCorrectObjCollision(Collision collision)
    {
        if (isBossAttack)
            return collision.gameObject.CompareTag(TagManager.Boss);

        if (!collision.gameObject.CompareTag(TagManager.Player))
            return false;

        var networkObj = collision.gameObject.GetComponent<NetworkObject>() ;
        return networkObj.IsOwner;
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }

    public void SetTargetToPlayer()
    {
        isBossAttack = false;
    }
    
    public void SetTargetToBoss()
    {
        isBossAttack = true;
    }
}
