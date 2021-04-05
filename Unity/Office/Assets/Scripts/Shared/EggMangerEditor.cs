#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(EggManger))]
public class EggMangerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var manger = (EggManger)target;

		DrawDefaultInspector();

		EditorGUILayout.Space(20);

		if (GUILayout.Button("Find All Part 1"))
		{
			foreach (var item in manger.EggList)
			{
				if (item.Part == 0)
				{
					item.Found();
				}
			}
		}

		if (GUILayout.Button("Find All Part 2"))
		{
			foreach (var item in manger.EggList)
			{
				if (item.Part == 1)
				{
					item.Found();
				}
			}
		}
	}
}
#endif