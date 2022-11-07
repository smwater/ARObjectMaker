using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    [SerializeField] private GameObject _buttonUI;

    public void Click()
    {
        _buttonUI.SetActive(false);
    }
}
