using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPoolable
{
    void Init(Action<GameObject> returnAction);
    void OnDespawn();
}

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    // 몬스터가 OnEnable()에서 패턴 이펙트들 등록
    private void Start()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }

    public GameObject GetObject(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Debug.Log($"프리팹 인덱스 {prefabIndex}에 대한 풀이 존재하지 않습니다.");
            return null;
        }

        GameObject obj = null;
        if (pools.ContainsKey(prefabIndex))
        {
            // 해당 풀에 남은 오브젝트가 있다면
            if (pools[prefabIndex].Count > 0)
            {
                // 큐에서 하나 빼주기
                obj = pools[prefabIndex].Dequeue();
            }
            // 해당 풀에 남은 게 없다면, 새로 만들어서 풀에 돌아가는 메서드를 넣어주기
            else
            {
                obj = Instantiate(prefabs[prefabIndex]);
                if (obj.TryGetComponent(out IPoolable poolable))
                    poolable.Init(x => ReturnObject(prefabIndex, x));
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);

            return obj;
        }
        return null;
    }

    // 비활성화하고 해당 오브젝트풀 큐에 넣어주기
    public void ReturnObject(int prefabIndex, GameObject obj)
    {
        obj.SetActive(false);
        pools[prefabIndex].Enqueue(obj);
    }
}