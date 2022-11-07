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
    /// ARAnchor를 해제하고 오브젝트를 삭제하는 메서드
    /// </summary>
    public void Delete()
    {
        ARAnchor = null;
        Destroy(gameObject);
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
}
