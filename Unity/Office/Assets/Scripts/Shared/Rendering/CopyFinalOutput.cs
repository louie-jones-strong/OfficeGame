using UnityEngine;
using UnityEngine.Rendering;

public class CopyFinalOutput : MonoBehaviour
{
	[SerializeField] RenderTexture OutputTexture;
	void Start()
	{
		RenderPipelineManager.endFrameRendering += OnEndFrameRendering;
	}

	void OnEndFrameRendering(ScriptableRenderContext context, Camera[] cameras)
	{
		// Put the code that you want to execute at the end of RenderPipeline.Render here
		// If you are using URP or HDRP, Unity calls this method automatically
		// If you are writing a custom SRP, you must call RenderPipeline.EndFrameRendering
		Graphics.Blit(null, OutputTexture);
	}

	void OnDestroy()
	{
		RenderPipelineManager.endFrameRendering -= OnEndFrameRendering;
	}
}