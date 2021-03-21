using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
	public static Hud Instance {private set; get;}
	[Header("Hud")]
	[SerializeField] Text TimerText;
	[SerializeField] Text PartText;
	[SerializeField] Text EggCountText;

	[SerializeField] Animator InteractAnimator;

	[Header("Main Menu")]
	//menu part
	[SerializeField] Animator MenuAnimator;

	[SerializeField] Slider SfxSlider;
	[SerializeField] Slider MusicSlider;
	[SerializeField] Slider AmbienceSlider;
	[SerializeField] Slider SensitivitySlider;

	[SerializeField] Button RestartButton;
	[SerializeField] Button PlayPart1Button;
	[SerializeField] Button PlayPart2Button;


	[Header("Game Over")]
	[SerializeField] Animator GameOverAnimator;
	[SerializeField] Text BodyText;

	public bool MenuOpen = true;
	public bool GameOverOpen = false;
	float TimeSinceSfxChange = -1f;
	float TimeSinceAmbienceChange = -1f;

	void Awake()
	{
		Instance = this;
		SetMenuShow(true);
		SetGameOverShow(false);
	}

	void Start()
	{
		SfxSlider.SetValueWithoutNotify(AudioManger.SfxVolume);
		MusicSlider.SetValueWithoutNotify(AudioManger.MusicVolume);
		AmbienceSlider.SetValueWithoutNotify(AudioManger.AmbienceVolume);

		var sensitivity = PlayerPrefsHelper.GetFloat("Sensitivity", 0.5f);
		UiSensitivityUpdated(sensitivity);
		SensitivitySlider.SetValueWithoutNotify(sensitivity);

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
		TimerText.text = TimeUtility.GetTimeString(PlayerController.CurrentPartTime);
		PartText.text = EggManger.Instance.FindingMode ? "Find All the Eggs" : "Put all the eggs back";
		EggCountText.text = $"EGGS: {EggManger.NumberOfEggsFound} / {EggManger.TotalNumberOfEggs}";

		if (!GameOverOpen && !MenuOpen && SimpleInput.IsInputInState(eInput.Esc, eButtonState.Pressed))
		{
			SetMenuShow(!MenuOpen);
		}

		float part1Time = PlayerPrefsHelper.GetFloat(Settings.Part1BestTimePrefKey, -1f);
		bool hasFinishedPart1 = part1Time >= 0f;

		RestartButton.gameObject.SetActive(!hasFinishedPart1);
		PlayPart1Button.gameObject.SetActive(hasFinishedPart1);
		PlayPart2Button.gameObject.SetActive(hasFinishedPart1);

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

	public void SetGameOverShow(bool show)
	{
		GameOverOpen = show;
		GameOverAnimator.SetBool("Show", show);

		BodyText.text = "Wow looks like your an eggspert at this. Would you like some more eggs-ercise?";
	}

	public void UiQuit()
	{
		MainManager.CloseGame();
	}

	public void UiPlayPart1()
	{
		EggManger.SetMode(true);
		MainManager.DoKickBack();
	}

	public void UiPlayPart2()
	{
		EggManger.SetMode(false);
		MainManager.DoKickBack();
	}

	public void UiClearData()
	{
		var sensitivity = PlayerPrefsHelper.GetFloat("Sensitivity");

		PlayerPrefsHelper.DeleteAll();

		//reset the volume values in the prefabs
		AudioManger.SfxVolume = AudioManger.SfxVolume;
		AudioManger.AmbienceVolume = AudioManger.AmbienceVolume;
		AudioManger.MusicVolume = AudioManger.MusicVolume;

		UiSensitivityUpdated(sensitivity);

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
		PlayerController.LookSpeed = 0.5f + value * 3f;
		PlayerPrefsHelper.SetFloat("Sensitivity", value);
	}
}