using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class CircleMaskRenderPass : ScriptableRenderPass
{
    private Material material;
    private RenderTargetHandle temporaryColorTexture;

    private float radius;
    private Vector2 center;

    public CircleMaskRenderPass(Material material)
    {
        this.material = material;
        temporaryColorTexture.Init("_TemporaryColorTexture");
        renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public void Setup(float radius, Vector2 center)
    {
        this.radius = radius;
        this.center = center;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (material == null)
            return;

        CommandBuffer cmd = CommandBufferPool.Get("CircleMaskRenderPass");

        var source = renderingData.cameraData.renderer.cameraColorTarget;

        RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        cmd.GetTemporaryRT(temporaryColorTexture.id, opaqueDesc, FilterMode.Bilinear);

        material.SetFloat("_Radius", radius);
        material.SetVector("_Center", new Vector4(center.x, center.y, 0, 0));

        cmd.Blit(source, temporaryColorTexture.Identifier(), material);
        cmd.Blit(temporaryColorTexture.Identifier(), source);

        cmd.ReleaseTemporaryRT(temporaryColorTexture.id);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
