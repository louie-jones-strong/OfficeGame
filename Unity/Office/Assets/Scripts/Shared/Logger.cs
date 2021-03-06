using UnityEngine;

public class Logger
{

#if UNITY_EDITOR || DEVELOPMENT_BUILD
	static bool LoggingAllowed = true;
#else
	static bool LoggingAllowed = false;
#endif

	public static void Log(string message, Object context=null)
	{
		if (LoggingAllowed)
		{
			Debug.Log(message, context);
		}
	}

	public static void LogWarning(string message, Object context=null)
	{
		Debug.LogWarning(message, context);
	}

	public static void LogError(string message, Object context=null)
	{
		Debug.LogError(message, context);
		
	}
	
}