using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	[SerializeField] float MusicFadeTime = 1f;

	const string GameMusic = "GameMusic";
	SoundSource CurrentGameSource;

	void Update()
	{
		if (CurrentGameSource == null ||
		!CurrentGameSource.isPlaying)
		{
			CurrentGameSource = AudioManger.PlayEvent(GameMusic, fadeTime: MusicFadeTime);
		}
	}
}