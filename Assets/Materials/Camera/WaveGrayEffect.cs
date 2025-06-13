using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class WaveGrayEffect : MonoBehaviour
{
    public Material waveGrayMaterial;  // 위 셰이더로 만든 머티리얼
    public Transform playerTransform;
    public float maxRadius = 0.3f;      // 화면 비율 단위 (0~1)
    public float waveDuration = 1f;

    private float timer = 0f;
    private bool isExpanding = true;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        timer = 0f;
        isExpanding = true;
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        timer += Time.deltaTime;

        if (timer >= waveDuration)
        {
            timer = 0f;
            isExpanding = !isExpanding;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (waveGrayMaterial == null || playerTransform == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        // 플레이어 월드 위치 -> 화면 좌표 변환 (0~1)
        Vector3 screenPos = cam.WorldToViewportPoint(playerTransform.position);

        // 현재 반경 (확대 및 축소)
        float radius = isExpanding
            ? Mathf.Lerp(0f, maxRadius, timer / waveDuration)
            : Mathf.Lerp(maxRadius, 0f, timer / waveDuration);

        waveGrayMaterial.SetVector("_PlayerPos", new Vector4(screenPos.x, screenPos.y, 0, 0));
        waveGrayMaterial.SetFloat("_WaveRadius", radius);

        Graphics.Blit(src, dest, waveGrayMaterial);
    }
}
