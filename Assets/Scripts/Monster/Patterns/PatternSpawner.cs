using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    public PatternSpawnerData spawnerData;

    public void EffectOn()
    {
        for (int i = 0; i < spawnerData.effectNum; i++)
        {
            // !!! 시간이 없어서 생성/파괴로... 주말에 오브젝트 풀링으로 변경
            Transform tmpEffect = Instantiate(spawnerData.effectPrefab, transform);
        }
    }
}
