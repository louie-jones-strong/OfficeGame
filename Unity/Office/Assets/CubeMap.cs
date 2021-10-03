using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMap : MonoBehaviour
{
	[SerializeField] ReflectionProbe Probe;
	int CurrentResolution = -1;

	void Start()
	{
		SetResolution(2048);
	}

	void SetResolution(int newResolution)
	{
		if (newResolution == CurrentResolution)
		{
			return;
		}

		Probe.resolution = newResolution;
		Probe.RenderProbe();
		CurrentResolution = newResolution;
	}
}
