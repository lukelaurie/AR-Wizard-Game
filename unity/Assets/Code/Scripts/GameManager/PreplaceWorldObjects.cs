using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Experimental.Lightship.AR.WorldPositioning;
using Niantic.Experimental.Lightship.AR.XRSubsystems;
using UnityEngine;
using UnityEngine.XR.ARKit;

public class PreplaceWorldObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> _possibleObjectsToPlace = new();
    [SerializeField] private List<LatLong> _latLongs = new();
    [SerializeField] private ARWorldPositioningManager _positioningManager;
    [SerializeField] private ARWorldPositioningObjectHelper _objectHelper;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var gpsCoord in _latLongs)
        {
            GameObject newObject = Instantiate(_possibleObjectsToPlace[_latLongs.IndexOf(gpsCoord) % _possibleObjectsToPlace.Count]);

            _objectHelper.AddOrUpdateObject(newObject, gpsCoord.latitude, gpsCoord.longitude, 0, Quaternion.identity);

            Debug.Log($"Added {newObject.name} with latitude {gpsCoord.latitude} and longitude {gpsCoord.longitude}");
        }

        _positioningManager.OnStatusChanged += OnStatusChanged;
    }

    private void OnStatusChanged(WorldPositioningStatus status)
    {
        Debug.Log("Status changed to " + status);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct LatLong
{
    public double latitude;
    public double longitude;
}