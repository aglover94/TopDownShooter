using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Public Variable
    public Sound[] sounds;

    private void Awake()
    {
        //Loop through the sounds array
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //Public method called when wanting to play a sound
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public bool IsAudioPlaying(string name)
    {
        bool soundPlaying;

        Sound s = Array.Find(sounds, sound => sound.name == name);

        soundPlaying = s.source.isPlaying;

        return soundPlaying;
    }
}
