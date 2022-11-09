using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    // 범위 내에 존재하는 다른 오브젝트의 인덱스를 저장하기 위한 리스트
    public List<int> DetectedObjectIndexes { get; private set; }

    private const float RADIUS = 7f;

    private SphereCollider _sphereCollider;
    private MyObject _myObject;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _myObject = GetComponentInParent<MyObject>();

        DetectedObjectIndexes = new List<int>();

        // 지정 범위로 콜라이더 크기 확대
        _sphereCollider.radius = RADIUS;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            MyObject otherObject = other.GetComponent<MyObject>();
            if (otherObject.IsConnected)
            {
                return;
            }

            DetectedObjectIndexes.Add(otherObject.Index);

            // 오브젝트가 범위 내에 들어올 때마다 상승
            _myObject.DetectedObjectCountUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            MyObject otherObject = other.GetComponent<MyObject>();
            DetectedObjectIndexes.Remove(otherObject.Index);

            // 오브젝트가 범위 내에서 사라질 때마다 하락
            _myObject.DetectedObjectCountDown();
        }
    }
}
