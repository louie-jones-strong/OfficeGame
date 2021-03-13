using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
public class SetSelected : MonoBehaviour
{
	Selectable RealSelectable;

	// we use this to select before we select the real one
	Selectable SelectableAlternative;

	void Awake()
	{
		RealSelectable = GetComponent<Selectable>();

		var temp = new GameObject("SetSelected SelectableAlternative");
		temp.transform.parent = transform;
		SelectableAlternative = temp.AddComponent<Selectable>();
		SelectableAlternative.transition = Selectable.Transition.None;
	}

	void OnEnable()
	{
		StartCoroutine(Select());
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}

	IEnumerator<YieldInstruction> Select()
	{
		yield return new WaitForSeconds(0.01f);

		if (RealSelectable != null)
		{
			if (SelectableAlternative != null)
			{
				SelectableAlternative.Select();
			}

			RealSelectable.Select();
			Logger.Log($"Setting selectable \"{name}\"");
		}
	}
}
