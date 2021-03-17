using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	public static int TotalNumberOfEggs {private set; get;} = 0;
	public static int NumberOfEggsFound {private set; get;} = 0;
	public bool IsFound {private set; get;} = false;

	[SerializeField] GameObject VisualEggMesh;
	[SerializeField] Collider EggCollider;

	void Awake()
	{
		TotalNumberOfEggs += 1;
	}

	public void Found()
	{
		if (!IsFound)
		{
			VisualEggMesh.SetActive(false);
			EggCollider.enabled = false;
			NumberOfEggsFound += 1;
			IsFound = true;
		}
	}

	void OnDestroy()
	{
		TotalNumberOfEggs -= 1;
		if (IsFound)
		{
			NumberOfEggsFound -= 1;
		}
	}
}
