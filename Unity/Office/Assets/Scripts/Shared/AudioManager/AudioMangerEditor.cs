#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(AudioManger))]
public class AudioMangerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var manger = (AudioManger)target;

		DrawDefaultInspector();

		EditorGUILayout.Space(20);

		//draw Sound list
		int numActive = 0;
		foreach (var item in manger.SoundSources)
		{
			if (item.InUse)
			{
				numActive += 1;
			}
		}
		EditorGUILayout.Space(10);

		var text = $"Total";
		text += $" | num sources: {manger.SoundSources.Count}";
		text += $" Active: {numActive}";
		EditorGUILayout.LabelField(text);

	}
}
#endif