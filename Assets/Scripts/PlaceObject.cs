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
                _objects[_objectIndex] = Instantiate(_objectPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                // local Anchor�� ����
                _objects[_objectIndex].GetComponent<MyObject>().CreateAnchor(arHit);
                // �� Object�� index�� �Ѱ���
                _objects[_objectIndex].GetComponent<MyObject>().SetIndex(_objectIndex);
                _objectIndex = (_objectIndex + 1) % _objectMaxCount;
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
        return _objects[index].transform.position;
    }
}
