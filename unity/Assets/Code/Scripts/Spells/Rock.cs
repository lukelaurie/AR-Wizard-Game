using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private float damageAmount;
    private float lifetime = 4f;
    [SerializeField] private GameObject crumble;
    // Start is called before the first frame update
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
            enemyComponent.TakeDamage(damageAmount);
            Instantiate(crumble, collision.contacts[0].point, Quaternion.identity);
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
