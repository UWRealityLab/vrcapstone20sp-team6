using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;

	public AudioClip clip;
	public GameObject parent;

	// Basic sound settings
	[Range(0f, 1f)]
	public float volume = 1f;
	[Range(.1f, 3f)]
	public float pitch = 1f;
	public bool loop = false;
	public bool playOnAwake = false;
	public bool playMultiple = true;
	
	// 3D sound settings
	[Range(0f, 1f)]
	public float spatialBlend = 1f;
	public float minDistance = 1f;
	public float maxDistance = 500f;

	[HideInInspector]
	public AudioSource source;
}
