using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	public bool IsFound;

	public GameObject VisualEggMesh;
	public Collider EggCollider;
	[SerializeField] ParticleSystem Particles;

	public int Part;

	public void Found()
	{
		if (!IsFound)
		{
			SetShow(false);
			IsFound = true;
			AudioManger.PlayEvent("Collect", transform);

			if (EggManger.NumberOfEggsFound >= EggManger.TotalNumberOfEggs)
			{
				var bestPartTime = PlayerPrefsHelper.GetFloat(EggManger.PartBestTimePrefKey, -1f);
				if (bestPartTime < 0f || PlayerController.CurrentPartTime <= bestPartTime)
				{
					PlayerPrefsHelper.SetFloat(EggManger.PartBestTimePrefKey, PlayerController.CurrentPartTime);
				}
				Hud.Instance.SetGameOverShow(true);
			}
			Particles.Play();
		}
	}

	public void SetShow(bool show)
	{
		VisualEggMesh.SetActive(show);
		EggCollider.enabled = show;
	}

	public void SetPart(int part)
	{
		SetShow(Part == part);
	}
}
