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
        // �Է��� ������ return
        if (Input.touchCount == 0)
        {
            return;
        }
        
        // ���ÿ� ���� �հ������� ��ġ���� ���, ù��° ��ġ�� �ν�
        Touch touch = Input.GetTouch(0);

        // ù��° ��ġ�� ����, UI ���� �ν� ����
        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        // GetKeyDown�� ���
        if (touch.phase == TouchPhase.Began)
        {
            // Setting Mode��� Object�� AR Plane�� ���� �Է��� ���� ����
            if (_currentMode == Mode.Setting)
            {
                return;
            }

            Ray ray;
            ray = _camera.ScreenPointToRay(touch.position);

            // Raycast�� Object�� ����
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Object");
            if (Physics.Raycast(ray, out hit, 10f, layerMask))
            {
                // Object�� �����ϸ� click()
                hit.transform.GetComponent<MyObject>().Click();

                _currentMode = Mode.Setting;

                // return�� ������ �Ʒ� AR raycast�� Object�� �߰��� ������ �� ����
                return;
            }
            
            List<ARRaycastHit> arHits = new List<ARRaycastHit>();
            ARRaycastHit arHit;
            
            // AR raycast�� AR Plane�� ����
            if (_arRaycastManager.Raycast(ray, arHits, TrackableType.PlaneWithinPolygon | TrackableType.FeaturePoint))
            {
                // Object�� ��� �����ߴٸ�, log ���
                if (_objectUsedCount >= _objectMaxCount)
                {
                    Debug.Log("���� ������ ������Ʈ�� �� �Ҹ��߽��ϴ�.");
                    return;
                }

                // ���� ��ü�� raycast�� �ν� �ƴٸ� ���� ������ ������ ���� ����
                arHit = arHits[0];

                // ������ ��ġ�� Object�� �����ϰ�
                Objects[ObjectIndex] = Instantiate(_objectPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                // local Anchor�� ����
                Objects[ObjectIndex].GetComponent<MyObject>().CreateAnchor(arHit);
                // �� Object�� index�� �Ѱ���
                Objects[ObjectIndex].GetComponent<MyObject>().SetIndex(ObjectIndex);
                ObjectIndex = (ObjectIndex + 1) % _objectMaxCount;
                _objectUsedCount++;

                _currentMode = Mode.Setting;
            }
        }
    }

    /// <summary>
    /// Ŭ���� �ܺο��� ��带 �����ϱ� ���� �޼���
    /// </summary>
    /// <param name="mode">������ ���</param>
    public void ChangeMode(Mode mode)
    {
        _currentMode = mode;
    }

    /// <summary>
    /// Ŭ���� �ܺο��� Ư�� ������Ʈ�� ��ġ�� ���ϱ� ���� �޼���
    /// </summary>
    /// <param name="index">���ϰ� ���� ������Ʈ�� �ε���</param>
    /// <returns>������Ʈ�� position</returns>
    public Vector3 ObjectPosition(int index)
    {
        return Objects[index].transform.position;
    }

    /// <summary>
    /// ������ ������Ʈ���� index�� ���� ������Ʈ���� �������ִ� �޼���
    /// </summary>
    /// <param name="index">��� ������ ������Ʈ�� index</param>
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
    /// �� ������Ʈ�� ������ ������Ʈ�� ������ �迭�� ��ȯ�� AnchorManager �迭�� �����ϴ� �޼���
    /// </summary>
    public void SaveCountArray()
    {

        if (_objectUsedCount == 0)
        {
            Debug.Log("������ �����Ͱ� �����ϴ�.");
            return;
        }

        // ���� �����ϴ� ������Ʈ �� ��ŭ �迭 ����
        int[] countArray = new int[_objectUsedCount];

        for (int i = 0; i < _objectUsedCount; i++)
        {
            // ī��Ʈ�� �迭�� ����
            countArray[i] = Objects[i].GetComponent<MyObject>().DetectedObjectCount;
        }

        // ������ ���� AnchorManager�� �ű�
        AnchorManager.Instance.SaveDetectedObjectCounts(countArray);
    }
}
