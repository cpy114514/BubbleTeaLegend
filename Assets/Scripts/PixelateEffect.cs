using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelateEffect : MonoBehaviour
{
    [Header("像素化强度（值越大越“块”）")]
    [Range(1, 16)]
    public int pixelSize = 4;

    [Header("像素化材质（使用 Custom/PixelateEffect Shader）")]
    public Material pixelateMaterial;

    private static readonly int PixelSizeID = Shader.PropertyToID("_PixelSize");

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (pixelateMaterial == null)
        {
            // 没有设置材质时，直接正常输出
            Graphics.Blit(source, destination);
            return;
        }

        pixelateMaterial.SetFloat(PixelSizeID, pixelSize);
        Graphics.Blit(source, destination, pixelateMaterial);
    }
}

