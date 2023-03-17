using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Playables;

public class GaussianBlur : PostEffectsBase 
{
	private static GaussianBlur instance;
	public static GaussianBlur Instance => instance;

	public Shader gaussianBlurShader;
	private Material gaussianBlurMaterial = null;
	public PlayableDirector GaussionTimeline;

    private void Awake()
    {
        instance = this;
    }

    public Material material {  
		get {
			gaussianBlurMaterial = CheckShaderAndCreateMaterial(gaussianBlurShader, gaussianBlurMaterial);
			return gaussianBlurMaterial;
		}  
	}

	// Blur iterations - larger number means more blur.
	[Range(1, 4)]
	public int iterations = 0;
	
	// Blur spread for each iteration - larger value means more blur
	[Range(0.0f, 3.0f)]
	public float blurSpread = 0.2f;
	
	[Range(1, 8)]
	public int downSample = 1;
	
	/// 3rd edition: use iterations for larger blur
	void OnRenderImage (RenderTexture src, RenderTexture dest) 
	{
		if (material != null) {
			int rtW = src.width/downSample;
			int rtH = src.height/downSample;

			RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
			buffer0.filterMode = FilterMode.Bilinear;

			Graphics.Blit(src, buffer0);

			for (int i = 0; i < iterations; i++) {
				material.SetFloat("_BlurSize", 1.0f + i * blurSpread);

				RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

				// Render the vertical pass
				Graphics.Blit(buffer0, buffer1, material, 0);

				RenderTexture.ReleaseTemporary(buffer0);
				buffer0 = buffer1;
				buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

				// Render the horizontal pass
				Graphics.Blit(buffer0, buffer1, material, 1);

				RenderTexture.ReleaseTemporary(buffer0);
				buffer0 = buffer1;
			}

			Graphics.Blit(buffer0, dest);
			RenderTexture.ReleaseTemporary(buffer0);
		} else {
			Graphics.Blit(src, dest);
		}
	}
	

	public void gaussianFunction() => StartCoroutine(nameof(gaussianIEnumerator));
	
	IEnumerator gaussianIEnumerator()
	{
		
		GaussionTimeline.Play();
		yield return null;
	}
}
