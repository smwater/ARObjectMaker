using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    [SerializeField] private PlaceObject _placeObject;

    public void Click()
    {
        _placeObject.SaveCountArray();
        AnchorManager.Instance.SortDetectedObjectCounts();
        AnchorManager.Instance.ConnectObjects();
    }
}
