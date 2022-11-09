using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class AnchorManager : SingletonBehaviour<AnchorManager>
{
    // 오브젝트에 감지된 다른 오브젝트의 개수를 저장하는 list
    public List<(int, int)> DetectedObjectCounts { get; private set; }

    private void Start()
    {
        DetectedObjectCounts = new List<(int, int)>();
    }

    /// <summary>
    /// 오브젝트 카운팅 배열을 인덱스와 카운터로 나눠 배열에 저장하는 메서드
    /// </summary>
    /// <param name="counts">오브젝트를 카운팅한 배열</param>
    public void SaveDetectedObjectCounts(int[] counts)
    {
        // 기존에 저장된 배열이 있다면 지우고 다시 만듦
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
    /// 저장한 인덱스, 카운트 배열을 카운트 기준으로 내림차순 정렬하는 메서드
    /// 카운트가 같을 경우, 인덱스는 오름차순으로 정렬된다.
    /// </summary>
    public void SortDetectedObjectCounts()
    {
        if (DetectedObjectCounts.Count == 0)
        {
            Debug.Log("정렬할 카운트가 없습니다.");
            return;
        }

        // count 기준으로 정렬
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
}
