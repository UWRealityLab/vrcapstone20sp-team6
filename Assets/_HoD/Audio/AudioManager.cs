using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			Initiate(s);
		}
	}

	void Start() 
	{
		foreach (Sound s in sounds) 
		{
			if (s.playOnAwake) 
			{
				this.Play(s);
			}
		}
	}

	// Method to create and attach AudioSource based on Sound object
	private void Initiate(Sound s)
	{
		if (s.parent == null) 
		{
			// 2D ambient sounds attach to AudioManager game object
			s.source = gameObject.AddComponent<AudioSource>();
		} 
		else 
		{
			// 3D spatial sounds attach to game object to originate from
			s.source = s.parent.AddComponent<AudioSource>();
		}

		// populate AudioSource properties with Sound object properties
		s.source.volume = s.volume;
		s.source.pitch = s.pitch;
		s.source.clip = s.clip;
		s.source.loop = s.loop;
		s.source.playOnAwake = s.playOnAwake;

		s.source.spatialBlend = s.spatialBlend;
		s.source.minDistance = s.minDistance;
		s.source.maxDistance = s.maxDistance;
	}

	// Play a fully non-spatial sound
	public void Play(string sound)
	{
		Sound s = FindSound(sound);
		if (s == null)
		{
			return;
		}
		this.Play(s);
	}

	// Play a sound that is spatially bound to an object
	public void Play3D(string sound, GameObject origin)
	{
		Sound s = FindSound(sound);
		if (s == null)
		{
			return;
		}
		s.parent = origin;
		this.Initiate(s);
		this.Play(s);
	}

	private void Play(Sound s)
	{
		if (s.playMultiple) 
		{
			s.source.Play();
		}
		else
		{
			if (!s.source.isPlaying)
			{
				s.source.Play();
			}
		}
	}

	public void Stop(string sound)
	{
		Sound s = FindSound(sound);
		if (s == null)
		{
			return;
		}
		s.source.Stop();
	}

	// Find and return the Sound object from given sound name
	private Sound FindSound(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
		}
		return s;
	}

}
