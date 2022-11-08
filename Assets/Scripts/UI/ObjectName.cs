using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectName : MonoBehaviour
{
    [SerializeField] private MyObject _myObject;
    private TextMeshProUGUI _name;

    private void Awake()
    {
        _name = GetComponent<TextMeshProUGUI>();

        Change();
    }

    /// <summary>
    /// 오브젝트 위의 이름을 바꿔주는 메서드
    /// </summary>
    public void Change()
    {
        _name.text = _myObject.Name;
    }
}
