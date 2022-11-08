using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementButton : MonoBehaviour
{
    [SerializeField] PlaceObject _placeObject;

    /// <summary>
    /// ���� �ֱٿ� ��ġ�� ������Ʈ�� �� ������ ��ġ�� ������Ʈ ������ �Ÿ��� ��� �޼���
    /// </summary>
    public void Click()
    {
        int nowIndex = _placeObject.ObjectIndex;

        if (nowIndex < 2)
        {
            Debug.Log("��ġ�� ������Ʈ�� 2�� �̸��Դϴ�.");
            return;
        }

        float distance = Vector3.Distance(_placeObject.ObjectPosition(nowIndex - 2), _placeObject.ObjectPosition(nowIndex - 1));
        Debug.Log($"{nowIndex - 1}��° ��ġ�� ������Ʈ�� {nowIndex}��° ��ġ�� ������Ʈ���� �Ÿ� : {distance}");
    }
}
