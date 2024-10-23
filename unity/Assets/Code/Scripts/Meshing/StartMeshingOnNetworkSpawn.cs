using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StartMeshingOnNetworkSpawn : NetworkBehaviour
{

    [SerializeField] private ARMeshManager meshManager;

    // Start is called before the first frame update
    void Start()
    {
        meshManager.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        meshManager.enabled = true;
    }
}
