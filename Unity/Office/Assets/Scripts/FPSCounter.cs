using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class FPSCounter : MonoBehaviour
{
	public static int CurrentFps {get; private set;}

	const float fpsMeasurePeriod = 0.5f;
	const string display = "{0} FPS";

	int FrameAccumulator = 0;
	float FpsNextPeriod = 0;
	Text Text;


	void Start()
	{
		FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
		Text = GetComponent<Text>();
	}


	void Update()
	{
		// measure average frames per second
		FrameAccumulator++;
		if (Time.realtimeSinceStartup >= FpsNextPeriod)
		{
			CurrentFps = (int) (FrameAccumulator/fpsMeasurePeriod);
			Text.text = string.Format(display, CurrentFps);

			FrameAccumulator = 0;
			FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
		}
	}
}
