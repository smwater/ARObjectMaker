using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MyObject : MonoBehaviour
{
    public ARAnchor ARAnchor { get; private set; }
    public int Index { get; private set; }

    [SerializeField] private GameObject _buttonUI;
    private ARAnchorManager _arAnchorManager;

    private void Awake()
    {
        _arAnchorManager = GameObject.Find("AR Session Origin").GetComponent<ARAnchorManager>();
    }

    /// <summary>
    /// PlaecObject ��ũ��Ʈ���� ������Ʈ ������ �ε����� �����ϱ� ���� �޼���
    /// </summary>
    /// <param name="value">������ �ε����� ��</param>
    public void SetIndex(int value)
    {
        Index = value;
    }

    /// <summary>
    /// Object�� Button UI�� ���� �޼���
    /// </summary>
    public void Click()
    {
        _buttonUI.SetActive(true);
    }

    /// <summary>
    /// Ư�� ������ ���� ��Ŀ�� �����ϴ� �޼���
    /// </summary>
    /// <param name="arHit">ARRaycastHit�� Ư���� ����</param>
    public void CreateAnchor(ARRaycastHit arHit)
    {
        if(arHit.trackable is ARPlane arPlane)
        {
            ARAnchor = _arAnchorManager.AttachAnchor(arPlane, arHit.pose);
        }
    }
}
