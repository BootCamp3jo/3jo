using System.Collections;
using UnityEngine;

public abstract class ASlowAble : MonoBehaviour
{
    // 속도 관련 요소
    protected float speedFactor;

    // 속도랑 관련된 팩터를 slowRate 만큼 느리게
    // 20% 느리게 만들고 싶다 >> slowRate = 0.2
    public virtual void Slow(float slowRate, float time)
    {
        
    }
}
