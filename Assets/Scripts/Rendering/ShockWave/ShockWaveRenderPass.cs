using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShockWaveRenderPass : ScriptableRenderPass
{
    private Material material;
    private RenderTargetHandle temporaryColorTexture;
    private string profilerTag = "ShockWave Effect";

    private float waveTime = 0f;
    private bool triggerWave = false;

    private float speed = 1.0f;
    private float waveWidth = 0.1f;
    private float duration = 1.0f;

    private Vector2 center = new Vector2(0.5f, 0.5f);

    public ShockWaveRenderPass(Material mat)
    {
        material = mat;
        temporaryColorTexture.Init("_TemporaryColorTexture");
    }

    public void SetParameters(float speed, float width, float duration)
    {
        this.speed = speed;
        this.waveWidth = width;
        this.duration = duration;
    }

    public void SetCenter(Vector2 center)
    {
        this.center = center;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ConfigureTarget(renderingData.cameraData.renderer.cameraColorTargetHandle);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (material == null || !triggerWave) return;

        waveTime += Time.deltaTime;

        material.SetFloat("_WaveTime", waveTime);
        material.SetVector("_Center", new Vector4(center.x, center.y, 0, 0));
        material.SetFloat("_Speed", speed);
        material.SetFloat("_WaveWidth", waveWidth);

        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
        var descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;

        cmd.GetTemporaryRT(temporaryColorTexture.id, descriptor, FilterMode.Bilinear);

        var source = renderingData.cameraData.renderer.cameraColorTargetHandle;
        cmd.Blit(source, temporaryColorTexture.Identifier(), material);
        cmd.Blit(temporaryColorTexture.Identifier(), source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);

        if (waveTime > duration)
        {
            triggerWave = false;
            waveTime = 0f;
        }
    }

    public void TriggerShockWave()
    {
        waveTime = 0f;
        triggerWave = true;
    }
}
