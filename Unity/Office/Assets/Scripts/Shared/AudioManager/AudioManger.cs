using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
	[SerializeField] AudioMixer Mixer;
	[SerializeField] AudioMixerGroup  MusicMixerGroup;
	[SerializeField] AudioMixerGroup  SfxMixerGroup;
	[SerializeField] AudioMixerGroup  AmbienceMixerGroup;
	public static AudioManger Instance;

	Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();

	public List<SoundSource> SoundSources {get; private set;} = new List<SoundSource>();

#region Volumes
#region MusicVolume
	const float DefaultMusicVolume = 0.5f;
	const string MusicPlayerPrefKey = "MusicVol";
	static float _MusicVolume;
	public static float MusicVolume {get {return _MusicVolume;} set {SetMusicVolume(value);}}
	static void SetMusicVolume(float value)
	{
		if (Instance == null)
		{
			Logger.LogError($"Trying to set Music Volume but Instance == null");
			return;
		}
		Logger.Log($"Setting Music Volume to {value}");
		_MusicVolume = value;
		Instance.Mixer.SetFloat("MusicVol", VolumeToDb(value));
		PlayerPrefsHelper.SetFloat(MusicPlayerPrefKey, value);
	}
#endregion

#region SfxVolume
	const float DefaultSfxVolume = 0.8f;
	const string SfxPlayerPrefKey = "SfxVol";
	static float _SfxVolume;
	public static float SfxVolume {get {return _SfxVolume;} set {SetSfxVolume(value);}}
	static void SetSfxVolume(float value)
	{
		if (Instance == null)
		{
			Logger.LogError($"Trying to set Sfx Volume but Instance == null");
			return;
		}
		Logger.Log($"Setting Sfx Volume to {value}");
		_SfxVolume = value;
		Instance.Mixer.SetFloat("SfxVol", VolumeToDb(value));
		PlayerPrefsHelper.SetFloat(SfxPlayerPrefKey, value);
	}
#endregion

#region AmbienceVolume
	const float DefaultAmbienceVolume = 0.8f;
	const string AmbiencePlayerPrefKey = "AmbienceVol";
	static float _AmbienceVolume;
	public static float AmbienceVolume {get {return _AmbienceVolume;} set {SetAmbienceVolume(value);}}
	static void SetAmbienceVolume(float value)
	{
		if (Instance == null)
		{
			Logger.LogError($"Trying to set Ambience Volume but Instance == null");
			return;
		}
		Logger.Log($"Setting Ambience Volume to {value}");
		_AmbienceVolume = value;
		Instance.Mixer.SetFloat("AmbienceVol", VolumeToDb(value));
		PlayerPrefsHelper.SetFloat(AmbiencePlayerPrefKey, value);
	}
#endregion
	public static float VolumeToDb(float volume)
	{
		if (volume != 0)
			return Mathf.Log10(volume) * 20;
		else
			return -144.0f;
	}
#endregion

	void Awake()
	{
		if (Instance != null)
		{
			return;
		}

		Instance = this;
		LoadSounds();
		MusicVolume = PlayerPrefsHelper.GetFloat(MusicPlayerPrefKey, DefaultMusicVolume);
		SfxVolume = PlayerPrefsHelper.GetFloat(SfxPlayerPrefKey, DefaultSfxVolume);
		AmbienceVolume = PlayerPrefsHelper.GetFloat(AmbiencePlayerPrefKey, DefaultAmbienceVolume);
	}

	void OnDestroy()
	{
		Instance = null;
	}

	void Start()
	{
		MusicVolume = MusicVolume;
		SfxVolume = SfxVolume;
		AmbienceVolume = AmbienceVolume;
	}

	void LoadSounds()
	{
		var soundArray = Resources.LoadAll<Sound>("");
		
		foreach (var sound in soundArray)
		{
			Sounds[sound.name] = sound;
		}
	}

	public static SoundSource PlayEvent(string path, Transform root=null, float fadeTime=-1f)
	{
		if (Instance == null)
		{
			Logger.LogError($"Cannot check if audio path valid as instance == null");
			return null;
		}
		
		if (!IsPathValid(path))
		{
			Logger.LogError($"PlayEvent called with path: \"{path}\" that not valid");
			return null;
		}

		var sound = Instance.Sounds[path];

		
		if (root == null)
		{
			root = Instance.transform;
		}
		
		SoundSource soundSource = null;
		foreach (var item in Instance.SoundSources)
		{
			if (!item.InUse && item.Transform == root)
			{
				Logger.Log("Found free source");
				soundSource = item;
				break;
			}
		}

		if (soundSource == null)
		{
			Logger.Log("didn't find free source so making one");
			soundSource = new SoundSource(root);
			Instance.SoundSources.Add(soundSource);
		}

		soundSource.PlaySound(sound);
		if (fadeTime > 0)
		{
			soundSource.JumpToFade(0);
			soundSource.SetFade(1, fadeTime);
		}

		Logger.Log($"Playing Audio event: \"{path}\" on source: {soundSource} on gameobject: {root}");
		return soundSource;
	}

	public AudioMixerGroup GetGetAudioBus(eAudioBusType busType)
	{
		switch (busType)
		{
			case eAudioBusType.Music:
				return MusicMixerGroup;
			case eAudioBusType.Ambience:
				return AmbienceMixerGroup;
			default:
				return SfxMixerGroup;
		}
	}
	
	void Update()
	{
		int listIndex = 0;
		while (listIndex < Instance.SoundSources.Count)
		{
			if(Instance.SoundSources[listIndex].Update(Time.deltaTime))
			{
				Instance.SoundSources.RemoveAt(listIndex);
			}
			else
			{
				listIndex += 1;
			}
		}
	}

	public static bool IsPathValid(string path)
	{
		if (Instance == null)
		{
			Logger.LogError($"Cannot check if audio path valid as instance == null");
			return false;
		}
		return Instance.Sounds.ContainsKey(path);
	}
}