using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioClipID
{
	Null = 0,
	Walking = 1
}

[System.Serializable]
public class AudioClipInfo
{
	public AudioClipID audioClipID;
	public AudioClip audioCLip;
}

public class AudioManagerScript : MonoBehaviour {

	private static AudioManagerScript mInstance;
	public static AudioManagerScript Instance
	{
		get {
			if (mInstance == null) {
				if (GameObject.FindWithTag ("SoundManager") != null) {
					mInstance = GameObject.FindWithTag ("SoundManager").GetComponent<AudioManagerScript> ();
				} else {
					GameObject obj = new GameObject ("_SoundManager");
					mInstance = obj.AddComponent<AudioManagerScript> ();
				}
			}
			return mInstance;
		}
	}

	public float sfxVolume = 1.0f;
	public float bgmVolume = 1.0f;
	public List<AudioClipInfo> audioClipInfoList = new List<AudioClipInfo> ();
	public AudioSource sfxAudioSource;
	public AudioSource bgmAudioSource;
	public List<AudioSource> sfxAudioSourceList = new List<AudioSource> ();
	public List<AudioSource> bgmAudioSourceList = new List<AudioSource> ();

	// Use this for initialization
	void Start () {
		AudioSource[] audioSourceList = this.GetComponentsInChildren<AudioSource> ();
		if (audioSourceList [0].gameObject.name == "BGMAudioSource") {
			bgmAudioSource = audioSourceList [0];
			sfxAudioSource = audioSourceList [1];
		} else {
			bgmAudioSource = audioSourceList [1];
			sfxAudioSource = audioSourceList [0];
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	AudioClip FindAudioClip(AudioClipID audioClipID)
	{
		for (int i = 0; i < audioClipInfoList.Count; i++) {
			if (audioClipInfoList [i].audioClipID == audioClipID) {
				return audioClipInfoList [i].audioCLip;
			}
		}
		return null;
	}

	//Background Music
	public void PlayBGM(AudioClipID audioClipID)
	{
		bgmAudioSource.clip = FindAudioClip (audioClipID);
		bgmAudioSource.volume = bgmVolume;
		bgmAudioSource.Play ();
	}
	public void PauseBGM()
	{
		if (bgmAudioSource.isPlaying) {
			bgmAudioSource.Pause ();
		}
	}

	public void StopBGM()
	{
		if (bgmAudioSource.isPlaying) {
			bgmAudioSource.Stop ();
		}
	}

	//Sound Effect
	public void PlaySFX(AudioClipID audioClipID)
	{
		if (!sfxAudioSource.isPlaying) {
			sfxAudioSource.PlayOneShot (FindAudioClip (audioClipID), sfxVolume);
		}
	}

	public void StopSFX()
	{
		if (sfxAudioSource.isPlaying) {
			sfxAudioSource.Stop ();
		}
	}

	public void PlayLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPlay = FindAudioClip (audioClipID);
		for (int i = 0; i < sfxAudioSourceList.Count; i++) {
			if (sfxAudioSourceList [i].clip == clipToPlay) {
				if (sfxAudioSourceList [i].isPlaying) {
					return;
				}
				sfxAudioSourceList [i].volume = sfxVolume;
				sfxAudioSourceList [i].Play ();
				return;
			}
		}
		AudioSource newInstance = gameObject.AddComponent<AudioSource> ();
		newInstance.clip = clipToPlay;
		newInstance.volume = sfxVolume;
		newInstance.loop = true;
		newInstance.Play ();
		sfxAudioSourceList.Add (newInstance);
	}

	public void PauseLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPause = FindAudioClip (audioClipID);
		for (int i = 0; i < sfxAudioSourceList.Count; i++) {
			if (sfxAudioSourceList [i].clip == clipToPause) {
				sfxAudioSourceList [i].Pause ();
				return;
			}
		}
	}

	public void StopLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToStop = FindAudioClip (audioClipID);
		for (int i = 0; i < sfxAudioSourceList.Count; i++) {
			if (sfxAudioSourceList [i].clip == clipToStop) {
				sfxAudioSourceList [i].Stop ();
				return;
			}
		}
	}
}
