using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementButton : MonoBehaviour
{
    [SerializeField] PlaceObject _placeObject;

    /// <summary>
    /// 가장 최근에 설치된 오브젝트와 그 직전에 설치된 오브젝트 사이의 거리를 재는 메서드
    /// </summary>
    public void Click()
    {
        int nowIndex = _placeObject.ObjectIndex;

        if (nowIndex < 2)
        {
            Debug.Log("설치된 오브젝트가 2개 미만입니다.");
            return;
        }

        float distance = Vector3.Distance(_placeObject.ObjectPosition(nowIndex - 2), _placeObject.ObjectPosition(nowIndex - 1));
        Debug.Log($"{nowIndex - 1}번째 설치된 오브젝트와 {nowIndex}번째 설치된 오브젝트와의 거리 : {distance}");
    }
}
