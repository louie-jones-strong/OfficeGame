using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundSource
{
	public readonly Transform Transform;
	public bool Tracked {get; private set;}
	public bool InUse {get {return Source != null && isPlaying;}}

	public bool isPlaying {get; private set;}

	public bool mute {get {return Source == null || Source.mute;} 
		set {
			if (Source != null)
			{
				Source.mute = value;
			};
		}} 

	public float time {get {return Source == null ? 0f : Source.time;}
		set {
			if (Source != null)
			{
				Source.time = value;
			};
		}}
	
	public AudioClip clip {get {return Source == null ? null : Source.clip;}} 

	Sound Sound;
	AudioSource Source;
	float Volume;
	float TargetFadeAmount;
	float FadeTime = 1f;
	float CurrentFadeTime = 1f;
	

	public SoundSource(Transform transform)
	{
		Source = transform.gameObject.AddComponent<AudioSource>();
		Transform = transform;
		Tracked = true;
	}

	public void PlaySound(Sound sound)
	{
		Sound = sound;
		var clipAndVolume = Sound.GetAudioClip();

		Source.enabled = true;
		Source.clip = clipAndVolume.Clip;

		Volume = clipAndVolume.Volume;
		Source.volume = Volume;
		JumpToFade(1);

		Source.outputAudioMixerGroup = AudioManger.Instance.GetGetAudioBus(Sound.AudioBus);
		Source.playOnAwake = false;
		Source.loop = Sound.LoopClip;
		Source.Play();
		isPlaying = true;
	}

	public bool Update(float deltaTime)
	{
		if (!Tracked)
		{
			//not sure if this would be possable. but worth logging if it happens
			Logger.LogError("update called on non tracked source");
			return !Tracked;
		}

		if (Source == null)
		{
			isPlaying = false;
			Tracked = false;
			Logger.Log($"removed SoundSource: {this} from tracking as source null");
		}
		else if (!Source.isPlaying)
		{
			isPlaying = false;
			Source.enabled = false;
		}

		CurrentFadeTime += deltaTime;
		Source.volume = Mathf.Lerp(Volume, Volume * TargetFadeAmount, CurrentFadeTime/FadeTime);
		return !Tracked;
	}
	
	public void SetFade(float targetFadeAmount, float fadeTime=1f)
	{
		TargetFadeAmount = targetFadeAmount;
		FadeTime = fadeTime;
		CurrentFadeTime = 0;
	}

	public void JumpToFade(float fadeAmount)
	{
		TargetFadeAmount = fadeAmount;
		CurrentFadeTime = -1f;
		FadeTime = 1f;
	}

	public void Stop()
	{
		Source.Stop();
	}

	public IEnumerator StopWithFade(float fadeTime=1f)
	{
		if(fadeTime > 0)
		{
			SetFade(0, fadeTime);
			while (CurrentFadeTime < FadeTime)
			{
				yield return null;
			}
		}
		Stop();
	}
}