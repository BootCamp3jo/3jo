using System.Collections;
using UnityEngine;

public class RangeAttack : MonsterAttack
{
    Collider2D trigger;

    // 공격 지속 시간
    public float AttackRemainTime {  get; set; }

    SpriteRenderer[] spriteRenderers;

    protected override void Awake()
    {
        trigger = GetComponent<Collider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        waveEffect = GetComponent<ShockWaveEffect>();

        base.Awake();
    }
    protected override void OnEnable()
    {
        // 트리거를 끄기
        trigger.enabled = false;
        // 경고 범위가 다시 보이게끔
        ChangeMarkEnable(true);

        base.OnEnable();
    }

    Coroutine coroutine_atk;
    public void Attack()
    {
        if(coroutine_atk != null)
            StopCoroutine(coroutine_atk);
        coroutine_atk = StartCoroutine(AttackAndEnd());
    }

    IEnumerator AttackAndEnd()
    {
        // 연출
        if (waveEffect != null)
            waveEffect.TriggerShockWave(transform);
        // 트리거를 켜서 공격!
        trigger.enabled = true;
        AudioManager.instance.PlaySFX(SFXType.Boom, 0.8f, 1f);
        // 공격 범위가 남아있으면 안되니 영역 표시 오브젝트 비활성화
        ChangeMarkEnable(false);
        // 지속 시간 동안 영역에 대미지
        yield return new WaitForSeconds(AttackRemainTime);
        // 다시 트리거를 끄기
        trigger.enabled = false;

        // 오브젝트 풀링 안녕... 다른 작업자의 요청으로 인해 패턴 파괴로 변경
        DestroyPattern();
    }

    void ChangeMarkEnable(bool isEnable)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].enabled = isEnable;
        }
    }
}
