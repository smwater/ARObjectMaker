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

        // 지정 범위로 콜라이더 크기 확대
        _sphereCollider.radius = RADIUS;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            // 오브젝트가 범위 내에 들어올 때마다 상승
            _myObject.DetectedObjectCountCountUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            // 오브젝트가 범위 내에서 사라질 때마다 하락
            _myObject.DetectedObjectCountCountDown();
        }
    }
}
