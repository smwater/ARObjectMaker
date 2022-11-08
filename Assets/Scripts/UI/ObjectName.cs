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
    /// ������Ʈ ���� �̸��� �ٲ��ִ� �޼���
    /// </summary>
    public void Change()
    {
        _name.text = _myObject.Name;
    }
}
