using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatchNotesPopup : MonoBehaviour
{
	[SerializeField] Animator Animator;
	[SerializeField] GameObject NewVersionBreadCrumb;
	[SerializeField] Text PatchNotesText;

	bool Open;

	void Start()
	{
		SetShowing(PatchNotes.HasUpdated);
		PatchNotesText.text = PatchNotes.BuildPatchNotes();
		NewVersionBreadCrumb.SetActive(PatchNotes.HasUpdated);
	}

	public void SetShowing(bool show)
	{
		if (Open != show)
		{
			AudioManger.PlayEvent(show ? "Popup Open" : "Popup Close");
		}
		Open = show;
		Animator.SetBool("Show", show);
	}
}