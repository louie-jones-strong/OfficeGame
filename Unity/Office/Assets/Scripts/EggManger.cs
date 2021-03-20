using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggManger : MonoBehaviour
{
	public static EggManger Instance;
	public List<Egg> EggList;
	public static int TotalNumberOfEggs {get {return Instance.EggList.Count;}}
	public static int NumberOfEggsFound {get{return GetNumberOfEggs();}}
	public bool FindingMode {private set; get;} = true;
	static string FindModePrefKey = "IsFindMode";

	void Awake()
	{
		Instance = this;

		var mode = PlayerPrefsHelper.GetBool(FindModePrefKey, true);
		if (FindingMode != mode)
		{
			SetMode(mode);
		}
	}

	public static void SetMode(bool findMode)
	{
		Instance.FindingMode = findMode;

		foreach (var egg in Instance.EggList)
		{
			egg.EggCollider.enabled = true;
			egg.SetShow(findMode);
			egg.IsFound = !findMode;
		}

		PlayerPrefsHelper.SetBool(FindModePrefKey, findMode);
	}

	static int GetNumberOfEggs()
	{
		int count = 0;
		foreach (var egg in Instance.EggList)
		{
			count += egg.IsFound ? 1 : 0;
		}
		return count;
	}

	void OnDestroy()
	{
		Instance = null;
	}
}
