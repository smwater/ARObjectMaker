using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MyObject : MonoBehaviour
{
    public ARAnchor ARAnchor { get; private set; }
    public int Index { get; private set; }
    public string Name { get; private set; }

    [SerializeField] private GameObject _buttonUI;
    private ARAnchorManager _arAnchorManager;

    private void Awake()
    {
        _arAnchorManager = GameObject.Find("AR Session Origin").GetComponent<ARAnchorManager>();
    }

    /// <summary>
    /// ARAnchor�� �����ϰ� ������Ʈ�� �����ϴ� �޼���
    /// </summary>
    public void Delete()
    {
        ARAnchor = null;
        Destroy(gameObject);
    }

    /// <summary>
    /// Ŭ���� �ܺο��� ������Ʈ �ε����� �����ϱ� ���� �޼���
    /// </summary>
    /// <param name="index">������ �ε����� ��</param>
    public void SetIndex(int index)
    {
        Index = index;
    }

    /// <summary>
    /// Ŭ���� �ܺο��� ������Ʈ �̸��� �����ϱ� ���� �޼���
    /// </summary>
    /// <param name="name">������ �̸�</param>
    public void SetName(string name)
    {
        Name = name;
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
