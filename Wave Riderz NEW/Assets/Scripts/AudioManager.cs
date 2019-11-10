/*-------------------------------------------------------------------*
|  Title:			AudioManager
|
|  Author:			Max Atkinson / Seth Johnston
| 
|  Description:		Contains an array of sounds to be played statically
					from any other script.
*-------------------------------------------------------------------*/

using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance; //Store a static instance of itself

	public Sound[] sounds;
	
    void Awake()
    {
		instance = this;    //Set the instance to itself

		foreach (Sound s in instance.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

	public static void Play (string name)
    {
		Sound s = Array.Find(instance.sounds, sound => sound.name == name);
        // warning message if it cant find the right sounds 
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
            
        s.source.Play();
    }
}
