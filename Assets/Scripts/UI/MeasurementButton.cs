using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementButton : MonoBehaviour
{
    [SerializeField] PlaceObject _placeObject;

    public void Click()
    {
        float distance = Vector3.Distance(_placeObject.ObjectPosition(0), _placeObject.ObjectPosition(1));
        Debug.Log(distance);
    }
}
