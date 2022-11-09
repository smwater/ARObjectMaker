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
    public int ObjectIndex { get; private set; }
    public GameObject[] Objects { get; private set; }

    [SerializeField] private GameObject _objectPrefab;

    private Camera _camera;
    private ARRaycastManager _arRaycastManager;

    private int _objectMaxCount = 40;
    private int _objectUsedCount = 0;

    private Mode _currentMode = Mode.Placement;

    private void Awake()
    {
        ObjectIndex = 0;
        Objects = new GameObject[_objectMaxCount];

        _camera = GetComponent<Camera>();
        _arRaycastManager = GetComponentInParent<ARRaycastManager>();
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
                Objects[ObjectIndex] = Instantiate(_objectPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                // local Anchor를 생성
                Objects[ObjectIndex].GetComponent<MyObject>().CreateAnchor(arHit);
                // 이 Object의 index를 넘겨줌
                Objects[ObjectIndex].GetComponent<MyObject>().SetIndex(ObjectIndex);
                ObjectIndex = (ObjectIndex + 1) % _objectMaxCount;
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
        return Objects[index].transform.position;
    }

    /// <summary>
    /// 해제된 오브젝트보다 index가 높은 오브젝트들을 정리해주는 메서드
    /// </summary>
    /// <param name="index">사용 해제된 오브젝트의 index</param>
    public void FreeIndex(int index)
    {
        _objectUsedCount--;

        for (int i = index; i < _objectUsedCount; i++)
        {
            Objects[i + 1].GetComponent<MyObject>().SetIndex(i);
        }

        ObjectIndex = _objectUsedCount;
    }

    /// <summary>
    /// 각 오브젝트가 감지한 오브젝트의 개수를 배열로 변환해 AnchorManager 배열에 저장하는 메서드
    /// </summary>
    public void SaveCountArray()
    {

        if (_objectUsedCount == 0)
        {
            Debug.Log("저장할 데이터가 없습니다.");
            return;
        }

        // 현재 존재하는 오브젝트 수 만큼 배열 생성
        int[] countArray = new int[_objectUsedCount];

        for (int i = 0; i < _objectUsedCount; i++)
        {
            // 카운트를 배열로 저장
            countArray[i] = Objects[i].GetComponent<MyObject>().DetectedObjectCount;
        }

        // 저장한 것을 AnchorManager로 옮김
        AnchorManager.Instance.SaveDetectedObjectCounts(countArray);
    }
}
