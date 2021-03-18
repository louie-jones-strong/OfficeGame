using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestTimesText : MonoBehaviour
{
	[SerializeField] Text Part1BestTimeText;
	[SerializeField] Text Part2BestTimeText;

	void OnEnable()
	{
		float part1Time = PlayerPrefsHelper.GetFloat(Settings.Part1BestTimePrefKey, -1f);
		bool hasPart1BestTime = part1Time >= 0f;

		Part1BestTimeText.gameObject.SetActive(hasPart1BestTime);
		if (hasPart1BestTime)
		{
			Part1BestTimeText.text = $"Part 1 Best Time: {TimeUtility.GetTimeString(part1Time)}";
		}

		float part2Time = PlayerPrefsHelper.GetFloat(Settings.Part2BestTimePrefKey, -1f);
		bool hasPart2BestTime = part2Time >= 0f;

		Part2BestTimeText.gameObject.SetActive(hasPart2BestTime);
		if (hasPart2BestTime)
		{
			Part2BestTimeText.text = $"Part 2 Best Time: {TimeUtility.GetTimeString(part2Time)}";
		}
	}
}
