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

        // ���� ������ �ݶ��̴� ũ�� Ȯ��
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

            // ������Ʈ�� ���� ���� ���� ������ ���
            _myObject.DetectedObjectCountUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            // ������Ʈ�� ���� ������ ����� ������ �϶�
            _myObject.DetectedObjectCountDown();
        }
    }
}
