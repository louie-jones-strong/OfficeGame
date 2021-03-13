using UnityEngine;

public class ButtonInfo
{
	public bool Active { get {return Value != 0;}}
	public eButtonState State { get; private set;}
	public float TimeInState { get; private set;}
	public float Value { get; private set;}

	float DeadZone;
	public string InputName { get; private set;}


	public ButtonInfo(string inputName, float deadZone)
	{
		State = eButtonState.none;
		InputName = inputName;
		DeadZone = deadZone;
	}

	public void Refresh(float deltaTime)
	{
		SetValue(deltaTime, Input.GetAxisRaw(InputName));
	}

	void SetValue(float deltaTime, float rawValue)
	{
		TimeInState += deltaTime;

		Value = CheckDeadZone(rawValue);

		if (Active)
		{
			if (State == eButtonState.Held || State == eButtonState.Pressed)
			{
				SetState(eButtonState.Held);
			}
			else
			{
				SetState(eButtonState.Pressed);
			}
		}
		else
		{
			if (State == eButtonState.Held || State == eButtonState.Pressed)
			{
				SetState(eButtonState.Released);
			}
			else
			{
				SetState(eButtonState.none);
			}
		}
	}

	float CheckDeadZone(float rawValue)
	{
		if (rawValue >= -DeadZone && rawValue <= DeadZone)
		{
			return 0;
		}
		return rawValue;
	}

	void SetState(eButtonState state)
	{
		if (State != state)
		{
			TimeInState = 0;
		}
		State = state;
	}

	public override string ToString()
	{
		return $"Value: {Value}, state: {State}";
	}
}