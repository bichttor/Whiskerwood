using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PSX
{
    public class PixelationRenderFeature : ScriptableRendererFeature
    {
        PixelationPass pixelationPass;

        public override void Create()
        {
            pixelationPass = new PixelationPass(RenderPassEvent.BeforeRenderingPostProcessing);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            pixelationPass.Setup(renderer.cameraColorTargetHandle);
            renderer.EnqueuePass(pixelationPass);
        }

        class PixelationPass : ScriptableRenderPass
        {
            private static readonly string shaderPath = "PostEffect/Pixelation";
            static readonly string k_RenderTag = "Render Pixelation Effects";
            static readonly int MainTexId = Shader.PropertyToID("_MainTex");
            static readonly int TempTargetId = Shader.PropertyToID("_TempTargetPixelation");

            // Pixelation shader properties
            static readonly int WidthPixelation = Shader.PropertyToID("_WidthPixelation");
            static readonly int HeightPixelation = Shader.PropertyToID("_HeightPixelation");
            static readonly int ColorPrecision = Shader.PropertyToID("_ColorPrecision");

            Pixelation pixelation;
            Material pixelationMaterial;
            RTHandle currentTarget;

            public PixelationPass(RenderPassEvent evt)
            {
                renderPassEvent = evt;

                var shader = Shader.Find(shaderPath);
                if (shader == null)
                {
                    Debug.LogError($"Shader not found: {shaderPath}");
                    return;
                }

                pixelationMaterial = CoreUtils.CreateEngineMaterial(shader);
            }

            public void Setup(RTHandle currentTarget)
            {
                this.currentTarget = currentTarget;
            }
            [System.Obsolete]
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (pixelationMaterial == null)
                    return;

                if (!renderingData.cameraData.postProcessEnabled)
                    return;

                var stack = VolumeManager.instance.stack;
                pixelation = stack.GetComponent<Pixelation>();

                if (pixelation == null || !pixelation.IsActive())
                    return;

                var cmd = CommandBufferPool.Get(k_RenderTag);
                Render(cmd, ref renderingData);
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            void Render(CommandBuffer cmd, ref RenderingData renderingData)
            {
                ref var cameraData = ref renderingData.cameraData;
                var source = currentTarget; // RTHandle
                var sourceId = source.nameID;
                int destination = TempTargetId;

                int w = cameraData.camera.scaledPixelWidth;
                int h = cameraData.camera.scaledPixelHeight;

                // Set material properties from volume
                pixelationMaterial.SetFloat(WidthPixelation, pixelation.widthPixelation.value);
                pixelationMaterial.SetFloat(HeightPixelation, pixelation.heightPixelation.value);
                pixelationMaterial.SetFloat(ColorPrecision, pixelation.colorPrecision.value);

                // Set render state
                cameraData.camera.depthTextureMode |= DepthTextureMode.Depth;

                // Do blit
                cmd.SetGlobalTexture(MainTexId, sourceId);
                cmd.GetTemporaryRT(destination, w, h, 0, FilterMode.Point, RenderTextureFormat.Default);
                cmd.Blit(sourceId, destination);
                cmd.Blit(destination, sourceId, pixelationMaterial, 0);
                cmd.ReleaseTemporaryRT(destination);
            }
        }
    }
}
