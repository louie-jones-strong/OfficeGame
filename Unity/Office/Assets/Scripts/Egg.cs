using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	public bool IsFound;

	public GameObject VisualEggMesh;
	public Collider EggCollider;
	[SerializeField] ParticleSystem Particles;

	public void Found()
	{
		if (EggManger.Instance.FindingMode && !IsFound)
		{
			EggCollider.enabled = false;
			SetShow(false);
			IsFound = true;

			if (EggManger.NumberOfEggsFound >= EggManger.TotalNumberOfEggs)
			{
				var bestPart1Time = PlayerPrefsHelper.GetFloat(Settings.Part1BestTimePrefKey, -1f);
				if (bestPart1Time < 0f || PlayerController.CurrentPartTime <= bestPart1Time)
				{
					PlayerPrefsHelper.SetFloat(Settings.Part1BestTimePrefKey, PlayerController.CurrentPartTime);
				}
				EggManger.SetMode(false);
				Hud.Instance.SetGameOverShow(true);
			}
			Particles.Play();
		}
		else if (!EggManger.Instance.FindingMode && IsFound)
		{
			EggCollider.enabled = false;
			SetShow(true);
			IsFound = false;

			if (EggManger.NumberOfEggsFound <= 0)
			{
				var bestPart2Time = PlayerPrefsHelper.GetFloat(Settings.Part2BestTimePrefKey, -1f);
				if (bestPart2Time < 0f || PlayerController.CurrentPartTime <= bestPart2Time)
				{
					PlayerPrefsHelper.SetFloat(Settings.Part2BestTimePrefKey, PlayerController.CurrentPartTime);
				}
				EggManger.SetMode(true);
				Hud.Instance.SetGameOverShow(true);
			}
			Particles.Play();
		}
	}

	public void SetShow(bool show)
	{
		VisualEggMesh.SetActive(show);
	}
}
