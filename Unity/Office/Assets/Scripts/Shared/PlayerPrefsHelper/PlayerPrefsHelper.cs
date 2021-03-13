using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsHelper : PlayerPrefs
{

#region Bool Player Prefs
	public static bool GetBool(string key, bool defaultValue)
	{
		if (!PlayerPrefs.HasKey(key))
		{
			return defaultValue;
		}

		return GetBool(key);
	}
	
	public static bool GetBool(string key)
	{
		var value = PlayerPrefs.GetInt(key) > 0;
		LogValueChange(false, key, value);
		return value;
	}

	public static void SetBool(string key, bool value)
	{
		PlayerPrefs.SetInt(key, value ? 1 : 0);
		LogValueChange(true, key, value);
	}
#endregion //Bool Player Prefs

#region Int Player Prefs
	public new static int GetInt(string key, int defaultValue)
	{
		if (!PlayerPrefs.HasKey(key))
		{
			return defaultValue;
		}

		return PlayerPrefs.GetInt(key);
	}
	
	public new static int GetInt(string key)
	{
		var value = PlayerPrefs.GetInt(key);
		LogValueChange(false, key, value);
		return value;
	}

	public new static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
		LogValueChange(true, key, value);
	}
#endregion //Int Player Prefs



	static void LogValueChange(bool setAction, string key, object value)
	{
		var actionType = setAction ? "Set" : "Get";
		Logger.Log($"PlayerPrefs {actionType} key: \"{key}\" [{value.GetType()}] to value: \"{value}\"");
	}
}