using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    // ���� ���� �����ϴ� �ٸ� ������Ʈ�� �ε����� �����ϱ� ���� ����Ʈ
    public List<int> DetectedObjectIndexes { get; private set; }

    private const float RADIUS = 7f;

    private SphereCollider _sphereCollider;
    private MyObject _myObject;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _myObject = GetComponentInParent<MyObject>();

        DetectedObjectIndexes = new List<int>();

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

            DetectedObjectIndexes.Add(otherObject.Index);

            // ������Ʈ�� ���� ���� ���� ������ ���
            _myObject.DetectedObjectCountUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            MyObject otherObject = other.GetComponent<MyObject>();
            DetectedObjectIndexes.Remove(otherObject.Index);

            // ������Ʈ�� ���� ������ ����� ������ �϶�
            _myObject.DetectedObjectCountDown();
        }
    }
}
