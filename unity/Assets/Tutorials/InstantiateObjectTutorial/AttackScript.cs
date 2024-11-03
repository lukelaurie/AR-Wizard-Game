using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public float radius = 15f;




    private void RadiusDamage()
    {

        Collider[] hitObjects = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in hitObjects)
        {
            Debug.Log(collider.name);
        }
    }






    private void OnCollisionEnter(Collision collision)
    {
        GameObject attack = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        RadiusDamage();
        GameObject.Destroy(attack, 2f);
        ObjectInstantiator.Attacks.AddFirst(gameObject);
        gameObject.SetActive(false);
    }
}
