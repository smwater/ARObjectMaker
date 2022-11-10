using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MyObject : MonoBehaviour
{
    private const float UNDER_THE_CRUST = -20f;

    public ARAnchor ARAnchor { get; private set; }
    public int Index { get; private set; }
    public int DetectedObjectCount { get; private set; }
    public string Name { get; private set; }
    public bool IsConnected { get; private set; }

    [SerializeField] private GameObject _buttonUI;
    private ARAnchorManager _arAnchorManager;
    private PlaceObject _placeObject;

    private void Awake()
    {
        _arAnchorManager = GameObject.Find("AR Session Origin").GetComponent<ARAnchorManager>();
        _placeObject = _arAnchorManager.gameObject.GetComponentInChildren<PlaceObject>();

        DetectedObjectCount = 0;
        Name = "Default";
        IsConnected = false;
    }

    /// <summary>
    /// ARAnchor�� �����ϰ� ������Ʈ�� �����ϴ� �޼���
    /// </summary>
    public void Delete()
    {
        _placeObject.ChangeMode(Mode.Placement);
        _placeObject.FreeIndex(Index);

        // OnTriggerExit�� �����Ǳ� ���� ������Ʈ�� �̵���Ŵ
        transform.position = new Vector3(transform.position.x, UNDER_THE_CRUST, transform.position.z);

        ARAnchor = null;
        
        // ��� �����Ǹ� OnTriggerExit�� �������� ���� �ð����� ��
        Destroy(gameObject, 0.1f);
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
    /// DetectedObjectCount�� �ܺο��� �÷��ִ� �޼���
    /// </summary>
    public void DetectedObjectCountUp()
    {
        DetectedObjectCount++;
    }

    /// <summary>
    /// DetectedObjectCount�� �ܺο��� �����ִ� �޼���
    /// </summary>
    public void DetectedObjectCountDown()
    {
        DetectedObjectCount--;
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

    /// <summary>
    /// �ش� ������Ʈ�� Ŭ���� ��Ŀ�� ����Ǿ��� ��, ����ϴ� �޼���
    /// </summary>
    public void ConnectCloudAnchor()
    {
        IsConnected = true;
    }
}