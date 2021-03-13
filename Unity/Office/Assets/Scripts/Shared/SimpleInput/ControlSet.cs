using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSet
{
	public readonly string Name;
	public bool HasBeenUsed {private set; get;}
	public Dictionary<eInput, ButtonInfo> Buttons {private set; get;}

	public ControlSet(string name)
	{
		Name = name;
		Buttons = new Dictionary<eInput, ButtonInfo>();
		HasBeenUsed = false;
	}

	public void AddButton(eInput inputType, string buttonName)
	{
		Buttons[inputType] = new ButtonInfo(buttonName, Settings.DeadZone);
	}

	public void Refresh(float deltaTime)
	{
		foreach (var kvp in Buttons)
		{
			kvp.Value.Refresh(deltaTime);
			if (kvp.Value.Active)
			{
				HasBeenUsed = true;
			}
		}
	}

	public eButtonState GetInputState(eInput input)
	{
		if (!Buttons.ContainsKey(input))
		{
			return eButtonState.none;
		}

		return Buttons[input].State;
	}

	public float GetTimeInState(eInput input)
	{
		if (!Buttons.ContainsKey(input))
		{
			return 0;
		}
		return Buttons[input].TimeInState;
	}

	public float GetLastTimeUsed()
	{
		var smallestTime = float.MaxValue;
		foreach (var inputType in Buttons.Keys)
		{
			var time = GetTimeInState(inputType);
			if (time <= smallestTime)
			{
				smallestTime = time;
			}
		}
		return smallestTime;
	}

	public bool GetInputActive(eInput input)
	{
		if (!Buttons.ContainsKey(input))
		{
			return false;
		}
		return Buttons[input].Active;
	}

	public float GetInputValue(eInput input)
	{
		if (!Buttons.ContainsKey(input))
		{
			return 0f;
		}
		return Buttons[input].Value;
	}
}