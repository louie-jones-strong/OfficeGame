using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StandaloneInputModuleOverride: StandaloneInputModule
{
	eControlType CurrentControlType = eControlType.none;
	protected override void Awake()
	{
		UpdateInputs();
		base.Awake();
	}

	public override void UpdateModule()
	{
		UpdateInputs();
		base.UpdateModule();
	}

	void UpdateInputs()
	{
		var controlType = SimpleInput.GetControlType();

		if (CurrentControlType != controlType &&
			SimpleInput.ControlSets.Count < (int)controlType &&
			(int)controlType >= 0)
		{
			var controlSet = SimpleInput.ControlSets[(int)controlType];

			horizontalAxis = controlSet.Buttons[eInput.XMoveAxis].InputName;
			verticalAxis = controlSet.Buttons[eInput.YMoveAxis].InputName;
			submitButton = controlSet.Buttons[eInput.Interact].InputName;
			cancelButton = "";
		}
	}
}