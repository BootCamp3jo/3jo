using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CircleMaskFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material material = null;
    }

    public Settings settings = new Settings();

    private CircleMaskRenderPass renderPass;

    public override void Create()
    {
        if (settings.material == null)
        {
            Debug.LogError("CircleMaskFeature: Material is not assigned.");
            return;
        }

        renderPass = new CircleMaskRenderPass(settings.material);
        renderPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material == null)
            return;

        // 여기서 메인 카메라인지 체크 (예: 태그가 "MainCamera" 인 경우만)
        if (!renderingData.cameraData.camera.CompareTag("MainCamera"))
            return;

        Vector3 playerPos = Vector3.zero;
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Camera cam = renderingData.cameraData.camera;
            Vector3 viewportPos = cam.WorldToViewportPoint(player.transform.position);
            playerPos = viewportPos;
        }

        float radius = 0f;
        if (CircleMaskEffectController.Instance != null)
            radius = CircleMaskEffectController.Instance.CurrentRadius;

        renderPass.Setup(radius, new Vector2(playerPos.x, playerPos.y));
        renderer.EnqueuePass(renderPass);
    }
}
