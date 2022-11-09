using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class AnchorManager : SingletonBehaviour<AnchorManager>
{
    // ������Ʈ�� ������ �ٸ� ������Ʈ�� ������ �����ϴ� list
    public List<(int, int)> DetectedObjectCounts { get; private set; }

    private bool[] _isInputted;
    private Dictionary<int, List<int>> _connectObjectDictionary = new Dictionary<int, List<int>>();

    [SerializeField] private PlaceObject _placeObject;

    private void Start()
    {
        DetectedObjectCounts = new List<(int, int)>();
    }

    /// <summary>
    /// ������Ʈ ī���� �迭�� �ε����� ī���ͷ� ���� �迭�� �����ϴ� �޼���
    /// </summary>
    /// <param name="counts">������Ʈ�� ī������ �迭</param>
    public void SaveDetectedObjectCounts(int[] counts)
    {
        // ������ ����� �迭�� �ִٸ� ����� �ٽ� ����
        if (DetectedObjectCounts.Count > 0)
        {
            DetectedObjectCounts.Clear();
        }    

        for (int i = 0; i < counts.Length; i++)
        {
            DetectedObjectCounts.Add((i, counts[i]));
        }
    }

    /// <summary>
    /// ������ �ε���, ī��Ʈ �迭�� ī��Ʈ �������� �������� �����ϴ� �޼���
    /// ī��Ʈ�� ���� ���, �ε����� ������������ ���ĵȴ�.
    /// </summary>
    public void SortDetectedObjectCounts()
    {
        if (DetectedObjectCounts.Count == 0)
        {
            Debug.Log("������ ī��Ʈ�� �����ϴ�.");
            return;
        }

        // count �������� ����
        DetectedObjectCounts.Sort(delegate((int, int) a, (int, int) b)
        {
            if (b.Item2 > a.Item2) return 1;
            else if (b.Item2 < a.Item2) return -1;
            else return 0;
        });

        for (int i = 0; i < DetectedObjectCounts.Count; i++)
        {
            Debug.Log($"index : {DetectedObjectCounts[i].Item1}, count : {DetectedObjectCounts[i].Item2}");
        }
    }

    public void ConnectObjects()
    {
        _isInputted = new bool[DetectedObjectCounts.Count];

        for (int i = 0; i < DetectedObjectCounts.Count; i++)
        {
            if (_isInputted[i])
            {
                Debug.Log($"{i}���� �̹� ��ųʸ��� ���� �ε���");
                continue;
            }

            _isInputted[i] = true;

            int objectIndex = DetectedObjectCounts[i].Item1;
            List<int> detectedObjects = _placeObject.Objects[objectIndex].GetComponentInChildren<Radar>().DetectedObjectIndexes;

            _connectObjectDictionary.Add(objectIndex, detectedObjects);
            
            for (int j = 0; j < detectedObjects.Count; j++)
            {
                _isInputted[j] = true;
            }
        }

        for (int i = 0; i < _connectObjectDictionary.Count; i++)
        {
            Debug.Log($"key : {i} / item : {_connectObjectDictionary[i]}");
        }
    }
}
