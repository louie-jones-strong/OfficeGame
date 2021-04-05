using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggManger : MonoBehaviour
{
	public static EggManger Instance;
	public List<Egg> EggList;
	public static int TotalNumberOfEggs {get {return GetTotalNumberOfEggs();}}
	public static int NumberOfEggsFound {get{return GetNumberOfFoundEggs();}}
	public static int CurrentPart {get; private set;} = -1;
	public const int MaxPart = 1;

	public static string PartBestTimePrefKey {get {return GetPartBestTimePrefKey(CurrentPart);}}

	public static string GetPartBestTimePrefKey(int part)
	{
		return $"Part{part+1}BestTime";
	}

	void Awake()
	{
		Instance = this;

		if (CurrentPart == -1)
		{
			CurrentPart = 0;
			var key = GetPartBestTimePrefKey(0);
			if (PlayerPrefsHelper.GetFloat(key, -1f) >= 0)
			{
				CurrentPart = 1;
			}
		}

		SetPart(CurrentPart);
	}

	static int GetTotalNumberOfEggs()
	{
		int count = 0;
		foreach (var egg in Instance.EggList)
		{
			if (egg.Part == CurrentPart)
			{
				count += 1;
			}
		}
		return count;
	}

	static int GetNumberOfFoundEggs()
	{
		int count = 0;
		foreach (var egg in Instance.EggList)
		{
			if (egg.Part == CurrentPart)
			{
				count += egg.IsFound ? 1 : 0;
			}
		}
		return count;
	}

	public static void SetPart(int part)
	{
		part = Mathf.Clamp(part, 0, MaxPart);
		CurrentPart = part;
		foreach (var item in Instance.EggList)
		{
			item.SetPart(CurrentPart);
		}
	}

	void OnDestroy()
	{
		Instance = null;
	}
}
