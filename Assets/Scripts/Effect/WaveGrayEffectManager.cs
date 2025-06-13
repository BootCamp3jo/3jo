using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class WaveGrayEffectManager : MonoBehaviour
{
    [SerializeField] private Material grayUnlitMaterial;
    [SerializeField] private float waveDuration = 1.0f;  // 확산 or 수축 각각 시간
    [SerializeField] private float maxWaveRadius = 5f;
    [SerializeField] private Transform playerTransform;

    private List<Renderer> targets = new List<Renderer>();
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    private bool isEffectActive = false;
    private float timer = 0f;
    private bool isExpanding = true;  // 확산 중인지 수축 중인지 상태

    private void Start()
    {
        foreach (var renderer in FindObjectsOfType<Renderer>())
        {
            if (renderer.gameObject.CompareTag("Player")) continue;

            if (renderer is SpriteRenderer || renderer is TilemapRenderer)
            {
                targets.Add(renderer);
                originalMaterials[renderer] = renderer.sharedMaterial;
            }
        }
    }

    public void StartWaveEffect()
    {
        if (isEffectActive) return;

        // 인스턴스화 및 초기 셋팅
        foreach (var renderer in targets)
        {
            Material instancedMat = new Material(grayUnlitMaterial);
            instancedMat.renderQueue = 2000; // Geometry queue (Tilemap보다 앞)
            renderer.material = instancedMat;
        }

        timer = 0f;
        isExpanding = true;
        isEffectActive = true;
    }

    private void Update()
    {
        if (!isEffectActive) return;
        if (playerTransform == null) return;

        timer += Time.deltaTime;

        Vector2 playerPos = playerTransform.position;
        float radius;
        float grayStrength;

        if (isExpanding)
        {
            // 확산 (0 -> maxWaveRadius)
            radius = Mathf.Lerp(0f, maxWaveRadius, timer / waveDuration);
            grayStrength = 1f;

            if (timer >= waveDuration)
            {
                // 확산 끝, 수축 시작
                timer = 0f;
                isExpanding = false;
            }
        }
        else
        {
            // 수축 (maxWaveRadius -> 0)
            radius = Mathf.Lerp(maxWaveRadius, 0f, timer / waveDuration);
            grayStrength = 1f;

            if (timer >= waveDuration)
            {
                // 수축 끝, 복원 및 종료
                foreach (var renderer in targets)
                {
                    if (originalMaterials.TryGetValue(renderer, out var mat))
                    {
                        renderer.material = mat;
                    }
                }
                isEffectActive = false;
                return;
            }
        }

        // 머티리얼 파라미터 업데이트
        foreach (var renderer in targets)
        {
            if (renderer.material == null) continue;

            renderer.material.SetVector("_PlayerPos", playerPos);
            renderer.material.SetFloat("_WaveRadius", radius);
            renderer.material.SetFloat("_GrayStrength", grayStrength);
        }
    }
}
