using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameSaveButton : MonoBehaviour
{
    [SerializeField] private MyObject _myObject;
    [SerializeField] private TextMeshProUGUI _changeNameText;
    [SerializeField] private ObjectName _objectName;
    [SerializeField] private GameObject _buttonUI;
    [SerializeField] private GameObject _nameUI;

    /// <summary>
    /// 변경한 오브젝트 이름을 저장하는 메서드
    /// </summary>
    public void Click()
    {
        _myObject.SetName(_changeNameText.text);
        _objectName.Change();

        _buttonUI.SetActive(true);
        _nameUI.SetActive(false);
    }
}
