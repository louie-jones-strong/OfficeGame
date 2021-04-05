using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestTimesText : MonoBehaviour
{
	[SerializeField] Text Part1BestTimeText;
	[SerializeField] Text Part2BestTimeText;

	public GameObject Part1NewBest;
	public GameObject Part2NewBest;

	void OnEnable()
	{
		var key = EggManger.GetPartBestTimePrefKey(0);
		float part1Time = PlayerPrefsHelper.GetFloat(key, -1f);
		bool hasPart1BestTime = part1Time >= 0f;

		Part1BestTimeText.text = $"Part 1 Best Time: ";
		if (hasPart1BestTime)
		{
			Part1BestTimeText.text += TimeUtility.GetTimeString(part1Time);
		}

		key = EggManger.GetPartBestTimePrefKey(1);
		float part2Time = PlayerPrefsHelper.GetFloat(key, -1f);
		bool hasPart2BestTime = part2Time >= 0f;

		Part2BestTimeText.text = $"Part 2 Best Time: ";
		if (hasPart2BestTime)
		{
			Part2BestTimeText.text += TimeUtility.GetTimeString(part2Time);
		}
	}
}
