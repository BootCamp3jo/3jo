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

        // 메인 카메라인지 체크
        if (!renderingData.cameraData.camera.CompareTag("MainCamera"))
            return;

        float radius = 0f;
        Vector2 center = new Vector2(0.5f, 0.5f); // 기본값

        if (CircleMaskEffectController.Instance != null)
        {
            radius = CircleMaskEffectController.Instance.CurrentRadius;
            center = CircleMaskEffectController.Instance.CurrentCenter; // ✅ 이 줄이 중요
        }

        renderPass.Setup(radius, center);
        renderer.EnqueuePass(renderPass);
    }
}
