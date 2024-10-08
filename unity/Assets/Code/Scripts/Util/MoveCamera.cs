using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MoverCamera : NetworkBehaviour
{
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private Vector3 offset;

    public override void OnNetworkSpawn()
    {
        cameraHolder.SetActive(IsOwner);
        base.OnNetworkSpawn(); 
    }


    void Update()
    {
        cameraHolder.transform.position = transform.position + offset;
    }
}
