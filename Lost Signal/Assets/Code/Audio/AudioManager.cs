using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] Sound[] sounds;

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

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
    }
    void Start()
    {
        //Play("Walk");
    }
    private void Update()
    {
        //Debug.Log(sounds[0].source.time);   
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        //Debug.Log("play " + name + " " + s.source);

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();


        //Debug.Log(s.source.isPlaying);
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        //Debug.Log("stoping " + name + " " + s.source);
        s.source.Stop();
    }
}
