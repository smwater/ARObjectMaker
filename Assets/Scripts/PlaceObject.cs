using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public enum Mode
{
    Placement,
    Setting,
    None
}

public class PlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject _objectPrefab;
    private GameObject[] _objects;

    private Camera _camera;
    private ARRaycastManager _arRaycastManager;

    private int _objectMaxCount = 40;
    private int _objectIndex = 0;
    private int _objectUsedCount = 0;

    private Mode _currentMode = Mode.Placement;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _arRaycastManager = GetComponentInParent<ARRaycastManager>();
        _objects = new GameObject[_objectMaxCount];
    }

    private void Update()
    {
        // 입력이 없으면 return
        if (Input.touchCount == 0)
        {
            return;
        }
        
        // 동시에 여러 손가락으로 터치했을 경우, 첫번째 터치만 인식
        Touch touch = Input.GetTouch(0);

        // 첫번째 터치에 한해, UI 뒤쪽 인식 방지
        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        // GetKeyDown과 비슷
        if (touch.phase == TouchPhase.Began)
        {
            // Setting Mode라면 Object나 AR Plane에 대한 입력을 받지 않음
            if (_currentMode == Mode.Setting)
            {
                return;
            }

            Ray ray;
            ray = _camera.ScreenPointToRay(touch.position);

            // Raycast로 Object를 감지
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Object");
            if (Physics.Raycast(ray, out hit, 10f, layerMask))
            {
                // Object가 존재하면 click()
                hit.transform.GetComponent<MyObject>().Click();

                _currentMode = Mode.Setting;

                // return이 없으면 아래 AR raycast로 Object가 추가로 생성될 수 있음
                return;
            }
            
            List<ARRaycastHit> arHits = new List<ARRaycastHit>();
            ARRaycastHit arHit;
            
            // AR raycast로 AR Plane을 감지
            if (_arRaycastManager.Raycast(ray, arHits, TrackableType.PlaneWithinPolygon | TrackableType.FeaturePoint))
            {
                // Object를 모두 생성했다면, log 출력
                if (_objectUsedCount >= _objectMaxCount)
                {
                    Debug.Log("생성 가능한 오브젝트를 다 소모했습니다.");
                    return;
                }

                // 여러 물체가 raycast로 인식 됐다면 가장 가깝게 감지된 곳을 저장
                arHit = arHits[0];

                // 감지된 위치에 Object를 생성하고
                _objects[_objectIndex] = Instantiate(_objectPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                // local Anchor를 생성
                _objects[_objectIndex].GetComponent<MyObject>().CreateAnchor(arHit);
                // 이 Object의 index를 넘겨줌
                _objects[_objectIndex].GetComponent<MyObject>().SetIndex(_objectIndex);
                _objectIndex = (_objectIndex + 1) % _objectMaxCount;
                _objectUsedCount++;

                _currentMode = Mode.Setting;
            }
        }
    }

    /// <summary>
    /// 클래스 외부에서 모드를 변경하기 위한 메서드
    /// </summary>
    /// <param name="mode">변경할 모드</param>
    public void ChangeMode(Mode mode)
    {
        _currentMode = mode;
    }

    /// <summary>
    /// 클래스 외부에서 특정 오브젝트의 위치를 구하기 위한 메서드
    /// </summary>
    /// <param name="index">구하고 싶은 오브젝트의 인덱스</param>
    /// <returns>오브젝트의 position</returns>
    public Vector3 ObjectPosition(int index)
    {
        return _objects[index].transform.position;
    }
}
