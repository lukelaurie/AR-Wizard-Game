using UnityEngine;
using Unity.Netcode; 
using System.Collections; 

public class Server : NetworkBehaviour
{
    void Start()
    {
        
        string[] args = System.Environment.GetCommandLineArgs(); 

        for(int i = 0; i < args.Length; i++)
        {
            if(args[i] == "-s")
            {
                Debug.Log("----------------Running as server-------------------"); 

                NetworkManager.Singleton.StartServer(); 

                NetworkManager.Singleton.OnClientConnectedCallback += ClientConnectMessage; 

                StartCoroutine(PingClients());
            }
        }
    }  

    public void ClientConnectMessage(ulong connectionID)
    {
        Debug.Log("---------" + connectionID + " has connected---------"); 
    }

    public IEnumerator PingClients()
    {
        yield return new WaitForSeconds(5f); 

        while(NetworkManager.Singleton.IsServer)
        {
            yield return new WaitForSeconds(5f); 
            Debug.Log("Sending ping..."); 
            PingClientRpc(); 
        }
    }


    [ClientRpc]
    public void PingClientRpc()
    {
        Debug.Log("Got ping from server!"); 
    }
}