using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSignal : MonoBehaviour {
    public AudioSource source;
    public AudioClip clip;
    public bool playingSound;
	// Use this for initialization
	void Start ()
    {
        source = gameObject.GetComponent<AudioSource>();
	}

    public void PlaySound(AudioClip clip)
    {

        this.clip = clip;
        if(!playingSound)
            playingSound = true;
    }
	// Update is called once per frame
	void Update ()
    {
        if (playingSound && !source.isPlaying)
        {
            //Debug.Log("goalSound");
            source.PlayOneShot(clip);
            playingSound = false;
        }

    }
}
