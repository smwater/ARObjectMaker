using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class AnchorManager : SingletonBehaviour<AnchorManager>
{
    private bool[] _isInputted;
    // 오브젝트에 감지된 다른 오브젝트의 개수를 저장하는 list
    private List<(int, int)> _detectedObjectCounts = new List<(int, int)>();
    private Dictionary<int, List<int>> _connectObjectDictionary = new Dictionary<int, List<int>>();

    [SerializeField] private PlaceObject _placeObject;

    /// <summary>
    /// 오브젝트 카운팅 배열을 인덱스와 카운터로 나눠 배열에 저장하는 메서드
    /// </summary>
    /// <param name="counts">오브젝트를 카운팅한 배열</param>
    public void SaveDetectedObjectCounts(int[] counts)
    {
        // 기존에 저장된 배열이 있다면 지우고 다시 만듦
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
    /// 저장한 인덱스, 카운트 배열을 카운트 기준으로 내림차순 정렬하는 메서드
    /// 카운트가 같을 경우, 인덱스는 오름차순으로 정렬된다.
    /// </summary>
    public void SortDetectedObjectCounts()
    {
        if (_detectedObjectCounts.Count == 0)
        {
            Debug.Log("정렬할 카운트가 없습니다.");
            return;
        }

        // count 기준으로 정렬
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
                Debug.Log($"{objectIndex}번은 이미 딕셔너리에 넣은 인덱스");
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
            Debug.Log($"key : {i} / item 개수 : {_connectObjectDictionary[i].Count}");
        }
    }
}
