using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
	public static Hud Instance {private set; get;}
	[SerializeField] Text TimerText;
	[SerializeField] Text EggCountText;

	[SerializeField] Animator InteractAnimator;

	void Awake()
	{
		Instance = this;
		PlayerPrefsHelper.GetFloat("Sensitivity", 0.5f);
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
	}

	public void SetShowInteractAnimator(bool show)
	{
		InteractAnimator.SetBool("Show", show);
	}

	public void UiCloseScreen()
	{

	}

	public void UiClearData()
	{
		PlayerPrefsHelper.DeleteAll();
	}

	public void UiSensitivityUpdated(float value)
	{
		PlayerController.LookSpeed = value;
		PlayerPrefsHelper.SetFloat("Sensitivity", value);
	}
}