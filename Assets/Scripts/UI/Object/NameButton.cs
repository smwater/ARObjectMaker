using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameButton : MonoBehaviour
{
    [SerializeField] private GameObject _buttonUI;
    [SerializeField] private GameObject _nameUI;

    public void Click()
    {
        _nameUI.SetActive(true);
        _buttonUI.SetActive(false);
    }
}
