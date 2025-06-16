using UnityEngine;

public class LifeCycle : MonoBehaviour
{
    // 기본 1초
    public float lifeTime { get; set; }  = 1f;

    protected virtual void OnEnable()
    {
      //  Invoke("LifeEnd", lifeTime);
    }

    public virtual void LifeEnd()
    {
        gameObject.SetActive(false);
    }
}
