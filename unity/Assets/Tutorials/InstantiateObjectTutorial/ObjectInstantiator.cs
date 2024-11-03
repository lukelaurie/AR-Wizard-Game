using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectInstantiator : MonoBehaviour
{

    public GameObject Player;
    public GameObject AttackPrefab;
    public static LinkedList<GameObject> Attacks = new LinkedList<GameObject>();
    public int count = 10000;


    public IEnumerator Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(AttackPrefab, transform);
            Attacks.AddLast(obj);
            yield return new WaitForSeconds(.1f);
        }

    }



    public void Wait()
    {
        alreadyshooting = false;
    }

    bool alreadyshooting = false;


    private void Update()
    {
        if (Input.GetKey(KeyCode.P) && !alreadyshooting)
        {
            if (ObjectInstantiator.Attacks.Count < 1) return;

            alreadyshooting = true;
            Invoke(nameof(Wait), .05f);

            Vector3 fwd = Player.transform.forward;
            ObjectInstantiator.Attacks.Last.Value.transform.rotation = Player.transform.rotation * Quaternion.Euler(0, -90, 0);
            ObjectInstantiator.Attacks.Last.Value.transform.position = (Player.transform.position + (fwd * 20) + new Vector3(0, 20, 0));

            ObjectInstantiator.Attacks.Last.Value.SetActive(true);
            ObjectInstantiator.Attacks.Last.Value.GetComponent<Rigidbody>().velocity = fwd * 100;
            ObjectInstantiator.Attacks.RemoveLast();

        }
    }





}