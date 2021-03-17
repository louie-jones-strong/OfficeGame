using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
	public static Hud Instance {private set; get;}
	[SerializeField] Text TimerText;
	[SerializeField] Text EggCountText;

	[SerializeField] Animator InteractAnimator;


	//menu part
	[SerializeField] Animator MenuAnimator;
	public bool MenuOpen = true;
	float TimeSinceSfxChange = -1f;
	float TimeSinceAmbienceChange = -1f;

	void Awake()
	{
		Instance = this;
		PlayerPrefsHelper.GetFloat("Sensitivity", 0.5f);
		SetMenuShow(true);
	}

	void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	void Update()
	{
		TimerText.text = TimeUtility.GetTimeString(Time.time);
		EggCountText.text = $"EGGS: {Egg.NumberOfEggsFound} / {Egg.TotalNumberOfEggs}";

		if (SimpleInput.IsInputInState(eInput.Esc, eButtonState.Pressed))
		{
			SetMenuShow(!MenuOpen);
		}

		if (TimeSinceSfxChange >= 0)
		{
			TimeSinceSfxChange += Time.deltaTime;
		}

		if (TimeSinceAmbienceChange >= 0)
		{
			TimeSinceAmbienceChange += Time.deltaTime;
		}

		if (TimeSinceSfxChange >= 0.25f)
		{
			AudioManger.PlayEvent("SfxVolChange");
			TimeSinceSfxChange = -1f;
		}

		if (TimeSinceAmbienceChange >= 0.25f)
		{
			AudioManger.PlayEvent("AmbienceVolChange");
			TimeSinceAmbienceChange = -1f;
		}
	}

	public void SetShowInteractAnimator(bool show)
	{
		InteractAnimator.SetBool("Show", show);
	}

//menu
	public void UiCloseScreen()
	{
		if (MenuOpen)
		{
			SetMenuShow(false);
		}
	}

	void SetMenuShow(bool show)
	{
		MenuOpen = show;
		MenuAnimator.SetBool("Show", show);
	}

	public void UiQuit()
	{
		MainManager.CloseGame();
	}

	public void UiClearData()
	{
		PlayerPrefsHelper.DeleteAll();

		//reset the volume values in the prefabs
		AudioManger.SfxVolume = AudioManger.SfxVolume;
		AudioManger.AmbienceVolume = AudioManger.AmbienceVolume;
		AudioManger.MusicVolume = AudioManger.MusicVolume;
		MainManager.DoKickBack();
	}

	public void UiSFXVolume(float value)
	{
		AudioManger.SfxVolume = value;
		TimeSinceSfxChange = 0;
	}

	public void UiMusicVolume(float value)
	{
		AudioManger.MusicVolume = value;
	}

	public void UiAmbienceVolume(float value)
	{
		AudioManger.AmbienceVolume = value;
		TimeSinceAmbienceChange = 0;
	}

	public void UiSensitivityUpdated(float value)
	{
		PlayerController.LookSpeed = value;
		PlayerPrefsHelper.SetFloat("Sensitivity", value);
	}
}