using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundSignal : MonoBehaviour, IComparable {
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

    public int getValue(string signal)
    {
        int value = 0;
        if (signal.Contains("ExtraShoot"))
        {
            signal = signal.Replace("ExtraShoot", "");
            signal = signal.Replace("(", "");
            signal = signal.Replace(")", "");
            if (!int.TryParse(signal, out value))
            {

                value = 0;

            }

            
            //print(otherTag + " ");
            //print("value2 " + value2);

        }
        return value;
    }

    int IComparable.CompareTo(object obj)
    {
        SoundSignal signal = (SoundSignal)obj;
        int value1 = 0;
        int value2 = 0;
        string thisTag = gameObject.name;
        string otherTag = signal.gameObject.name;

        value1 = getValue(thisTag);
        value2 = getValue(otherTag);
        if (signal.CompareTag("Goal"))
        {
            return -1;
        }
        else if (gameObject.CompareTag("Goal"))
        {
            return 1;
        }
        else if (value1 > value2)
        {
            print("Value1 > value2");
            return 1;
        }
        else if (value1 < value2)
        {
            print("Value2>value1");
            return -1;
        }

        else
        {
            return 0;
        }

        
    }
}
