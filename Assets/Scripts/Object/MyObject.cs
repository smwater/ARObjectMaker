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
    /// ARAnchor를 해제하고 오브젝트를 삭제하는 메서드
    /// </summary>
    public void Delete()
    {
        _placeObject.ChangeMode(Mode.Placement);
        _placeObject.FreeIndex(Index);

        // OnTriggerExit에 감지되기 위해 오브젝트를 이동시킴
        transform.position = new Vector3(transform.position.x, UNDER_THE_CRUST, transform.position.z);

        ARAnchor = null;
        
        // 즉시 삭제되면 OnTriggerExit에 감지되지 못해 시간차를 둠
        Destroy(gameObject, 0.1f);
    }

    /// <summary>
    /// 클래스 외부에서 오브젝트 인덱스를 변경하기 위한 메서드
    /// </summary>
    /// <param name="index">변경할 인덱스의 값</param>
    public void SetIndex(int index)
    {
        Index = index;
    }

    /// <summary>
    /// 클래스 외부에서 오브젝트 이름을 변경하기 위한 메서드
    /// </summary>
    /// <param name="name">변경할 이름</param>
    public void SetName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// DetectedObjectCount를 외부에서 올려주는 메서드
    /// </summary>
    public void DetectedObjectCountUp()
    {
        DetectedObjectCount++;
    }

    /// <summary>
    /// DetectedObjectCount를 외부에서 내려주는 메서드
    /// </summary>
    public void DetectedObjectCountDown()
    {
        DetectedObjectCount--;
    }

    /// <summary>
    /// Object의 Button UI를 띄우는 메서드
    /// </summary>
    public void Click()
    {
        _buttonUI.SetActive(true);
    }

    /// <summary>
    /// 특정 지점에 로컬 앵커를 부착하는 메서드
    /// </summary>
    /// <param name="arHit">ARRaycastHit로 특정된 지점</param>
    public void CreateAnchor(ARRaycastHit arHit)
    {
        if(arHit.trackable is ARPlane arPlane)
        {
            ARAnchor = _arAnchorManager.AttachAnchor(arPlane, arHit.pose);
        }
    }

    /// <summary>
    /// 해당 오브젝트가 클라우드 앵커와 연결되었을 때, 사용하는 메서드
    /// </summary>
    public void ConnectCloudAnchor()
    {
        IsConnected = true;
    }
}
