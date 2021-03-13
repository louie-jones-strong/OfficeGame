using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eButtonState { none = -1, Pressed, Held, Released };
public enum eInput { none = -1, XMoveAxis, YMoveAxis, XLookAxis, YLookAxis, Jump, Interact, Esc, NumberOfInputs};
public enum eControlType { none = -1, WASD, Arrows, Controller, Touch};

public class SimpleInput : MonoBehaviour
{
	static SimpleInput Instance;
	public static List<ControlSet> ControlSets {private set; get;} = new List<ControlSet>();
	public static int ControlSetCount {get {return ControlSets.Count;}}
	float LastTouchTime = float.MaxValue;

	void Awake()
	{
		if (Instance != null)
		{
			enabled = false;
			return;
		}

		Instance = this;

		int i = (int)eControlType.WASD + 1;

		var controlSet = new ControlSet("Keyboard(WASD)");
		controlSet.AddButton(eInput.XMoveAxis, $"Horizontal_Move_{i}");
		controlSet.AddButton(eInput.YMoveAxis, $"Vertical_Move_{i}");

		controlSet.AddButton(eInput.XLookAxis, $"Horizontal_Look_{i}");
		controlSet.AddButton(eInput.YLookAxis, $"Vertical_Look_{i}");
		controlSet.AddButton(eInput.Jump, $"Jump_{i}");

		controlSet.AddButton(eInput.Interact, $"Interact_{i}");
		controlSet.AddButton(eInput.Esc, $"Exit_{i}");
		ControlSets.Add(controlSet);

		i = (int)eControlType.Arrows + 1;

		controlSet = new ControlSet("Keyboard(Arrows)");
		controlSet.AddButton(eInput.XMoveAxis, $"Horizontal_Move_{i}");
		controlSet.AddButton(eInput.YMoveAxis, $"Vertical_Move_{i}");
		controlSet.AddButton(eInput.Jump, $"Jump_{i}");

		controlSet.AddButton(eInput.Interact, $"Interact_{i}");
		controlSet.AddButton(eInput.Esc, $"Exit_{i}");
		ControlSets.Add(controlSet);

		i = (int)eControlType.Controller + 1;
		controlSet = new ControlSet("Controller");
		controlSet.AddButton(eInput.XMoveAxis, $"Horizontal_Move_{i}");
		controlSet.AddButton(eInput.YMoveAxis, $"Vertical_Move_{i}");

		controlSet.AddButton(eInput.XLookAxis, $"Horizontal_Look_{i}");
		controlSet.AddButton(eInput.YLookAxis, $"Vertical_Look_{i}");
		controlSet.AddButton(eInput.Jump, $"Jump_{i}");

		controlSet.AddButton(eInput.Interact, $"Interact_{i}");
		controlSet.AddButton(eInput.Esc, $"Exit_{i}");
		ControlSets.Add(controlSet);
	}

	void OnDestroy()
	{
		Instance = null;
	}

	void Update()
	{
		foreach (var controlSet in ControlSets)
		{
			controlSet.Refresh(Time.deltaTime);
		}

		if (Input.touchCount > 0)
		{
			LastTouchTime = Time.time;
		}
	}

#region  public API

	public static eControlType GetControlType()
	{
		var lastestIndex = -1;
		var smallestTime = float.MaxValue;

		for (int loop = 0; loop < ControlSets.Count; loop++)
		{
			var controlSet = ControlSets[loop];
			if (!controlSet.HasBeenUsed)
			{
				continue;
			}

			var time = controlSet.GetLastTimeUsed();
			if (time <= smallestTime)
			{
				smallestTime = time;
				lastestIndex = loop;
			}
		}

		eControlType controlType;
		if (Instance.LastTouchTime <= smallestTime)
		{
			controlType = eControlType.Touch;
		}
		else
		{
			controlType = (eControlType)lastestIndex;
		}
		return controlType;
	}

	public static bool IsInputInState(eInput input, eButtonState state, int index=-1)
	{
		if (index >= ControlSetCount || index < -1)
		{
			Logger.LogError($"IsInputInState called with index({index}) out of range");
		}

		for (int loop = 0; loop < ControlSetCount; loop++)
		{
			if (ControlSets[loop].GetInputState(input) == state && (index == -1 || index == loop))
			{
				return true;
			}
		}
		return false;
	}

	public static float GetInputValue(eInput input, int index=-1)
	{
		if (index >= ControlSetCount || index < -1)
		{
			Logger.LogError($"GetInputValue called with index({index}) out of range");
		}

		float value = 0;
		int count = 0;
		for (int loop = 0; loop < ControlSetCount; loop++)
		{
			if (ControlSets[loop].GetInputActive(input) && (index == -1 || index == loop))
			{
				value += ControlSets[loop].GetInputValue(input);
				count += 1;
			}
		}

		if (count == 0)
		{
			return value;
		}

		return value / count;
	}

	public static bool GetInputActive(eInput input, int index=-1)
	{
		if (index >= ControlSetCount || index < -1)
		{
			Logger.LogError($"GetInputActive called with index({index}) out of range");
			return false;
		}

		for (int loop = 0; loop < ControlSetCount; loop++)
		{
			if (ControlSets[loop].GetInputActive(input) && (index == -1 || index == loop))
			{
				return true;
			}
		}
		return false;
	}
#endregion
}
