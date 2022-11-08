using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    private const float RADIUS = 7f;

    private SphereCollider _sphereCollider;
    private MyObject _myObject;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _myObject = GetComponentInParent<MyObject>();

        _sphereCollider.radius = RADIUS;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            _myObject.ConnectedObjectCountCountUp();
            Debug.Log(_myObject.ConnectedObjectCount);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            _myObject.ConnectedObjectCountCountDown();
            Debug.Log(_myObject.ConnectedObjectCount);
        }
    }
}