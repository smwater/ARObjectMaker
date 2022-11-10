using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class AnchorManager : SingletonBehaviour<AnchorManager>
{
    private bool[] _isInputted;
    // ������Ʈ�� ������ �ٸ� ������Ʈ�� ������ �����ϴ� list
    private List<(int, int)> _detectedObjectCounts = new List<(int, int)>();
    private Dictionary<int, List<int>> _connectObjectDictionary = new Dictionary<int, List<int>>();

    [SerializeField] private PlaceObject _placeObject;

    /// <summary>
    /// ������Ʈ ī���� �迭�� �ε����� ī���ͷ� ���� �迭�� �����ϴ� �޼���
    /// </summary>
    /// <param name="counts">������Ʈ�� ī������ �迭</param>
    public void SaveDetectedObjectCounts(int[] counts)
    {
        // ������ ����� �迭�� �ִٸ� ����� �ٽ� ����
        if (_detectedObjectCounts.Count > 0)
        {
            _detectedObjectCounts.Clear();
        }    

        for (int i = 0; i < counts.Length; i++)
        {
            _detectedObjectCounts.Add((i, counts[i]));
        }
    }

    /// <summary>
    /// ������ �ε���, ī��Ʈ �迭�� ī��Ʈ �������� �������� �����ϴ� �޼���
    /// ī��Ʈ�� ���� ���, �ε����� ������������ ���ĵȴ�.
    /// </summary>
    public void SortDetectedObjectCounts()
    {
        if (_detectedObjectCounts.Count == 0)
        {
            Debug.Log("������ ī��Ʈ�� �����ϴ�.");
            return;
        }

        // count �������� ����
        _detectedObjectCounts.Sort(delegate((int, int) a, (int, int) b)
        {
            if (b.Item2 > a.Item2) return 1;
            else if (b.Item2 < a.Item2) return -1;
            else return 0;
        });

        for (int i = 0; i < _detectedObjectCounts.Count; i++)
        {
            Debug.Log($"index : {_detectedObjectCounts[i].Item1}, count : {_detectedObjectCounts[i].Item2}");
        }
    }

    public void ConnectObjects()
    {
        _isInputted = new bool[_detectedObjectCounts.Count];

        if (_connectObjectDictionary.Count != 0)
        {
            _connectObjectDictionary.Clear();
        }

        for (int i = 0; i < _detectedObjectCounts.Count; i++) 
        {
            int objectIndex = _detectedObjectCounts[i].Item1;

            if (_isInputted[objectIndex])
            {
                Debug.Log($"{objectIndex}���� �̹� ��ųʸ��� ���� �ε���");
                _connectObjectDictionary.Add(objectIndex, new List<int>());
                continue;
            }

            _isInputted[objectIndex] = true;

            List<int> detectedObjects = _placeObject.Objects[objectIndex].GetComponentInChildren<Radar>().DetectedObjectIndexes;

            _connectObjectDictionary.Add(objectIndex, detectedObjects);
            
            for (int j = 0; j < detectedObjects.Count; j++)
            {
                int inputtedObjectIndex = detectedObjects[j];
                _isInputted[inputtedObjectIndex] = true;
            }
        }

        for (int i = 0; i < _connectObjectDictionary.Count; i++)
        {
            Debug.Log($"key : {i} / item ���� : {_connectObjectDictionary[i].Count}");
        }
    }
}
