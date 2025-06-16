using System;
using UnityEngine;

// 특정 위치에 잠깐 소환했다 오브젝트 풀로 돌아갈 효과들
public class EffectController : LifeCycle, IPoolable
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void LifeEnd()
    {
        OnDespawn();
    }

    // 오브젝트 풀 순환용 액션과 이를 등록하는 절차
    Action<GameObject> returnToPool;

    // 초기화 때 풀에 돌아가는 액션 등록
    public void Init(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
    }

    // 오브젝트를 풀에 다시 넣고 비활성화
    public void OnDespawn()
    {
        returnToPool?.Invoke(gameObject);
    }
}
