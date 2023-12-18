using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;
        public float volumeVariance = 0.1f;
        public float pitchVariance = 0.1f;
        public bool loop;
        [NonSerialized]
        public AudioSource source;
    }

    public Sound[] sounds;
    public Sound[] themes;

    public static AudioManager instance;
    // Start is called before the first frame update
    private void Awake()
    {

       if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        //SFX
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
        //Themes
        foreach (Sound s in themes)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
        ThemeSetter.manager = this;

    }


    public void Play(string name, Sound[] soundArray)
    {
        Sound s = Array.Find(soundArray, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        //Debug.Log("play " + name + " " + s.source);
        //if the sounds we are talking about are the SFX then randomize pitch and volume
        if(soundArray == sounds)
        {
            s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
        }


        s.source.Play();


        //Debug.Log(s.source.isPlaying);
    }

    public void Stop(string name, Sound[] soundArray)
    {
        Sound s = Array.Find(soundArray, item => item.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        //Debug.Log("stoping " + name + " " + s.source);
        s.source.Stop();
    }



    //------------
    //Themes
    //------------
    [Serializable]
    public class ThemeSetter
    {
        public bool SetTheme = false;
        public int ThemeIndex;
        [Tooltip("Stop = 0, Start = 1")] public bool StopStart;
        public float ThemeDelay;

        public static AudioManager manager;
        public void Send()
        {
            //dialogue Ended
            if (SetTheme)
            {
                
                manager.StartCoroutine(SetThemeMusicCO(ThemeIndex, StopStart, ThemeDelay));
            }
        }
        protected IEnumerator SetThemeMusicCO(int Index, bool StartStop, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!StartStop)
            {
                manager.Stop(manager.themes[Index].name, manager.themes);
            }
            else
            {
                manager.Play(manager.themes[Index].name, manager.themes);
            }
        }
    }




}
