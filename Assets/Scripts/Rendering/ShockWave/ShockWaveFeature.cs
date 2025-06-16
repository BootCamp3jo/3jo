using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShockWaveFeature : ScriptableRendererFeature
{
    public static ShockWaveFeature Instance { get; private set; }

    public Material shockWaveMaterial;
    private ShockWaveRenderPass shockWaveRenderPass;

    public override void Create()
    {
        Instance = this;

        shockWaveRenderPass = new ShockWaveRenderPass(shockWaveMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(shockWaveRenderPass);
    }

    public void SetParameters(float speed, float width, float duration)
    {
        shockWaveRenderPass?.SetParameters(speed, width, duration);
    }

    public void SetCenter(Vector2 center)
    {
        shockWaveRenderPass?.SetCenter(center);
    }

    public void TriggerShockWave()
    {
        shockWaveRenderPass?.TriggerShockWave();
    }
}
