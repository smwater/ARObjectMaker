using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    [SerializeField] private GameObject _buttonUI;
    private PlaceObject _placeObject;

    private void Awake()
    {
        _placeObject = GameObject.Find("AR Camera").GetComponent<PlaceObject>();
    }

    public void Click()
    {
        _placeObject.ChangeMode(Mode.Placement);
        _buttonUI.SetActive(false);
    }
}
