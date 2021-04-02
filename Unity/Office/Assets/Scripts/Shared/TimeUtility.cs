using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeUtility
{
	static string SecondsSuffix = "S";
	static string MinutesSuffix = "M";

	public static string GetTimeString(float totalSeconds)
	{
		var totalSecondsInt = Mathf.RoundToInt(totalSeconds);
		var secs = totalSecondsInt % 60;
		var mins = (totalSecondsInt / 60) % 60;

		var text = string.Empty;
		if (mins > 0)
		{
			text = $"{mins}{MinutesSuffix} ";
		}


		text += $"{Math.Round(totalSeconds, 2)}";

		if (totalSecondsInt == Math.Round(totalSeconds, 2))
		{
			text += ".00";
		}
		else if (Math.Round(totalSeconds, 1) == Math.Round(totalSeconds, 2))
		{
			text += "0";
		}

		text += $"{SecondsSuffix}";

		return text;
	}
}