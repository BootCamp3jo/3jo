using UnityEngine;

public class Pattern : MonoBehaviour
{
    RangeAttack rangeAttack;

    public float lifeTime = 3f;

    private void Awake()
    {
        rangeAttack = GetComponentInChildren<RangeAttack>(true);
    }

    void OnEnable()
    {
        Invoke("LifeEnd", lifeTime);   
    }

    void LifeEnd()
    {
        gameObject.SetActive(false);
    }

    public void GetAtkData(Vector3 targetPos, float damage)
    {
        if (rangeAttack != null)
        {
            rangeAttack.atkPoint = targetPos;
            rangeAttack.Damage = damage;
        }
    }
}
