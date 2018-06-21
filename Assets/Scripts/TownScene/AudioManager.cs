using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    
    public static AudioManager Instance { get; private set; }

    private AudioSource bgm;

	// Use this for initialization
	void Awake () {
        bgm = GetComponent<AudioSource>();
        bgm.loop = true;
        bgm.Play();
    }
}
