using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class WaveGrayEffectManager : MonoBehaviour
{
    [SerializeField] private Material grayUnlitMaterial;
    [SerializeField] private float waveDuration = 1.0f;  // 확산 or 수축 각각 시간
    [SerializeField] private float maxWaveRadius = 5f;
    [SerializeField] private Transform playerTransform;

    private List<SpriteRenderer> spriteTargets = new List<SpriteRenderer>();
    private List<TilemapRenderer> tilemapTargets = new List<TilemapRenderer>();

    // 원본 머티리얼 저장
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();
    // 머티리얼 인스턴스 저장 (활성화 시 1회 생성)
    private Dictionary<Renderer, Material> effectMaterials = new Dictionary<Renderer, Material>();

    private bool isEffectActive = false;
    private float timer = 0f;
    private bool isExpanding = true;

    private void Start()
    {
        foreach (var renderer in FindObjectsOfType<Renderer>())
        {
            if (renderer.gameObject.CompareTag("Player")) continue;

            if (renderer is SpriteRenderer spriteRenderer)
            {
                spriteTargets.Add(spriteRenderer);
                originalMaterials[spriteRenderer] = spriteRenderer.sharedMaterial;
            }
            else if (renderer is TilemapRenderer tilemapRenderer)
            {
                tilemapTargets.Add(tilemapRenderer);
                originalMaterials[tilemapRenderer] = tilemapRenderer.sharedMaterial;
            }
        }
    }

    public void StartWaveEffect()
    {
        if (isEffectActive) return;

        timer = 0f;
        isExpanding = true;
        isEffectActive = true;

        // 머티리얼 인스턴스 초기화
        effectMaterials.Clear();
    }

    private void Update()
    {
        if (!isEffectActive || playerTransform == null) return;

        timer += Time.deltaTime;
        float clampedTimer = Mathf.Min(timer, waveDuration);
        Vector2 playerPos = playerTransform.position;

        float radius = isExpanding
            ? Mathf.Lerp(0f, maxWaveRadius, clampedTimer / waveDuration)
            : Mathf.Lerp(maxWaveRadius, 0f, clampedTimer / waveDuration);

        // SpriteRenderer 처리
        foreach (var spriteRenderer in spriteTargets)
        {
            float dist = Vector2.Distance(playerPos, spriteRenderer.transform.position);
            bool inRange = dist <= radius;

            if (inRange)
            {
                if (!effectMaterials.ContainsKey(spriteRenderer))
                {
                    // 머티리얼 인스턴스 생성 1회
                    Material instancedMat = new Material(grayUnlitMaterial);
                    instancedMat.renderQueue = 2000;
                    spriteRenderer.material = instancedMat;
                    effectMaterials[spriteRenderer] = instancedMat;
                }
            }
            else
            {
                if (effectMaterials.ContainsKey(spriteRenderer))
                {
                    // 원본 머티리얼로 복원
                    if (originalMaterials.TryGetValue(spriteRenderer, out var origMat))
                    {
                        spriteRenderer.material = origMat;
                    }
                    // 인스턴스 머티리얼 제거
                    effectMaterials.Remove(spriteRenderer);
                }
            }
        }

        // TilemapRenderer 처리
        foreach (var tilemapRenderer in tilemapTargets)
        {
            float dist = Vector2.Distance(playerPos, tilemapRenderer.transform.position);
            bool inRange = dist <= radius;

            if (inRange)
            {
                if (!effectMaterials.ContainsKey(tilemapRenderer))
                {
                    Material instancedMat = new Material(grayUnlitMaterial);
                    instancedMat.renderQueue = 2000;
                    tilemapRenderer.material = instancedMat;  // material로 인스턴스 유지
                    effectMaterials[tilemapRenderer] = instancedMat;
                }
            }
            else
            {
                if (effectMaterials.ContainsKey(tilemapRenderer))
                {
                    if (originalMaterials.TryGetValue(tilemapRenderer, out var origMat))
                    {
                        tilemapRenderer.material = origMat;
                    }
                    effectMaterials.Remove(tilemapRenderer);
                }
            }
        }

        // 머티리얼 파라미터 업데이트 (활성된 것만)
        foreach (var kvp in effectMaterials)
        {
            Material mat = kvp.Value;
            if (mat == null) continue;
            mat.SetVector("_PlayerPos", playerPos);
            mat.SetFloat("_WaveRadius", radius);
            mat.SetFloat("_GrayStrength", 1f);
        }

        // 효과 상태 전환 및 종료 처리
        if (timer >= waveDuration)
        {
            timer = 0f;

            if (isExpanding)
            {
                isExpanding = false;
            }
            else
            {
                // 모두 복원
                foreach (var kvp in effectMaterials)
                {
                    Renderer r = kvp.Key;
                    if (originalMaterials.TryGetValue(r, out var origMat))
                    {
                        r.material = origMat;
                    }
                }
                effectMaterials.Clear();
                isEffectActive = false;
            }
        }
    }
}
