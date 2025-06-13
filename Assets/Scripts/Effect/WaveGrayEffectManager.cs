using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaveGrayEffectManager : MonoBehaviour
{
    [SerializeField] private Material grayUnlitMaterial;  // 회색조 머티리얼 (SpriteRenderer용)
    [SerializeField] private Material grayTilemapMaterial; // 회색조 머티리얼 (타일맵용)
    [SerializeField] private float maxWaveRadius = 5f;
    [SerializeField] private float waveDuration = 1f;

    private List<SpriteRenderer> spriteTargets = new List<SpriteRenderer>();
    private Dictionary<SpriteRenderer, Material> originalSpriteMaterials = new Dictionary<SpriteRenderer, Material>();

    private List<TilemapRenderer> tilemapTargets = new List<TilemapRenderer>();
    private Dictionary<TilemapRenderer, Material> originalTilemapMaterials = new Dictionary<TilemapRenderer, Material>();

    private bool isEffectActive = false;

    void Awake()
    {
        // SpriteRenderer 수집 (Player 태그 제외)
        SpriteRenderer[] allSprites = FindObjectsOfType<SpriteRenderer>();
        foreach (var sr in allSprites)
        {
            if (sr.CompareTag("Player")) continue;
            spriteTargets.Add(sr);
            originalSpriteMaterials[sr] = sr.material;
        }

        // TilemapRenderer 수집 (Player 태그는 없지만 혹시 태그로 필터링 필요하면 추가)
        TilemapRenderer[] allTilemaps = FindObjectsOfType<TilemapRenderer>();
        foreach (var tr in allTilemaps)
        {
            tilemapTargets.Add(tr);
            originalTilemapMaterials[tr] = tr.material;
        }
    }

    public void StartWaveEffect(Vector2 playerPos)
    {
        if (isEffectActive) return;
        StartCoroutine(WaveRoutine(playerPos));
    }

    private IEnumerator WaveRoutine(Vector2 playerPos)
    {
        isEffectActive = true;

        // 머티리얼 교체 (SpriteRenderer)
        foreach (var sr in spriteTargets)
        {
            sr.material = new Material(grayUnlitMaterial);
        }

        // 머티리얼 교체 (TilemapRenderer)
        foreach (var tr in tilemapTargets)
        {
            tr.material = new Material(grayTilemapMaterial);
        }

        float timer = 0f;
        // 확장하며 회색조 유지 (grayStrength=1)
        while (timer < waveDuration)
        {
            float radius = Mathf.Lerp(0, maxWaveRadius, timer / waveDuration);
            float grayStrength = 1f;

            foreach (var sr in spriteTargets)
            {
                sr.material.SetVector("_PlayerPos", playerPos);
                sr.material.SetFloat("_WaveRadius", radius);
                sr.material.SetFloat("_GrayStrength", grayStrength);
            }

            foreach (var tr in tilemapTargets)
            {
                tr.material.SetVector("_PlayerPos", playerPos);
                tr.material.SetFloat("_WaveRadius", radius);
                tr.material.SetFloat("_GrayStrength", grayStrength);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        // 회색조 서서히 풀기 (grayStrength 1->0)
        while (timer < waveDuration)
        {
            float grayStrength = Mathf.Lerp(1f, 0f, timer / waveDuration);

            foreach (var sr in spriteTargets)
            {
                sr.material.SetVector("_PlayerPos", playerPos);
                sr.material.SetFloat("_WaveRadius", maxWaveRadius);
                sr.material.SetFloat("_GrayStrength", grayStrength);
            }

            foreach (var tr in tilemapTargets)
            {
                tr.material.SetVector("_PlayerPos", playerPos);
                tr.material.SetFloat("_WaveRadius", maxWaveRadius);
                tr.material.SetFloat("_GrayStrength", grayStrength);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 머티리얼 원복
        foreach (var sr in spriteTargets)
        {
            if (originalSpriteMaterials.TryGetValue(sr, out var mat))
                sr.material = mat;
        }
        foreach (var tr in tilemapTargets)
        {
            if (originalTilemapMaterials.TryGetValue(tr, out var mat))
                tr.material = mat;
        }

        isEffectActive = false;
    }
}
