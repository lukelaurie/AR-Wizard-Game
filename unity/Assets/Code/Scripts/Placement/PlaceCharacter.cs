using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceCharacter : NetworkBehaviour
{
    [SerializeField] private GameObject placementObject;
    

    private bool isPlaced = false;
    [SerializeField] private Camera mainCam;

    public static event Action characterPlaced;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI Hit was recognized");
                return;
            }
            TouchToRay(Input.mousePosition);
        }
#endif
#if UNITY_IOS || UNITY_ANDROID
        
        if (Input.touchCount > 0 && Input.touchCount < 2 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Do raycasting to detect if any ui element was clicked on
            Touch touch = Input.GetTouch(0);
            
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = touch.position;

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0) {
                // We hit a UI element
                Debug.Log("We hit an UI Element");
                return;
            }
            
            Debug.Log("Touch detected, fingerId: " + touch.fingerId);  // Debugging line


            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Debug.Log("Is Pointer Over GOJ, No placement ");
                return;
            }

            // use the location of where clicked to create an object
            TouchToRay(touch.position);
        }
#endif
    }

    void TouchToRay(Vector3 touch)
    {
        Ray ray = mainCam.ScreenPointToRay(touch);
        RaycastHit hit;

        // checks if the ray from the camera hit a physical game object
        if (Physics.Raycast(ray, out hit))
        {
            // calculate rotation of the object relative to object location
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            SpawnPlayerServerRpc(hit.point, rotation, NetworkManager.Singleton.LocalClientId);
        }
    }


    [ServerRpc(RequireOwnership = false)] // saying that any client can execute this method
    void SpawnPlayerServerRpc(Vector3 positon, Quaternion rotation, ulong callerID)
    {
        GameObject character = Instantiate(placementObject, positon, rotation);

        NetworkObject characterNetworkObject = character.GetComponent<NetworkObject>();

        characterNetworkObject.SpawnWithOwnership(callerID);

        Debug.Log("here3");

        // AllPlayerDataManager.Instance.AddPlacedPlayer(callerID);
    }
}
