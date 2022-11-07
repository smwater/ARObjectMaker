using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MyObject : MonoBehaviour
{
    public int Index { get; private set; }

    public void SetIndex(int value)
    {
        Index = value;
    }

    public void Click()
    {

    }

    public void CreateAnchor(ARRaycastHit hit)
    {

    }
}
