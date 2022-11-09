using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    [SerializeField] private GameObject _object;

    public void Click()
    {
        _object.GetComponent<MyObject>().Delete();
    }
}
