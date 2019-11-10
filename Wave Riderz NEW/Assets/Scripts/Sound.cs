/*-------------------------------------------------------------------*
|  Title:			Sound
|
|  Author:			Max Atkinson
| 
|  Description:		A sound object which contains all the details of
					a specific named audio clip and audio source.
*-------------------------------------------------------------------*/

using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    //[Range(0.1f, 3f)]
    //public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
