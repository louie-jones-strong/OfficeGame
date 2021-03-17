using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	[SerializeField] float MusicFadeTime = 1f;

	const string Music = "Music";
	SoundSource CurrentMusicSource;

	void Update()
	{
		if (CurrentMusicSource == null ||
		!CurrentMusicSource.isPlaying)
		{
			CurrentMusicSource = AudioManger.PlayEvent(Music, fadeTime: MusicFadeTime);
		}
	}
}