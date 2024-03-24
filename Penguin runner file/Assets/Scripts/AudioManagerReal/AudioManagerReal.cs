using UnityEngine;
using System.Collections;

public class AudioManagerReal : MonoBehaviour
{
    public static AudioManagerReal Instance { get { return instance; } }
    private static AudioManagerReal instance;

    [SerializeField] private float musicVolume = 1;

    private AudioSource music1;
    private AudioSource music2;
    private AudioSource sfxSource;

    private bool firstMusicSourceActve;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);

        music1 = gameObject.AddComponent<AudioSource>();
        music2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        music1.loop = true;
        music2.loop = true;


    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);

    }
    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusicWthXFade(AudioClip musicClip, float transtitionTime = 1)
    {
        //Determine which source is active
        AudioSource activeSource = (firstMusicSourceActve) ? music1 : music2;
        AudioSource newSource = (firstMusicSourceActve) ? music2 : music1;

        firstMusicSourceActve = !firstMusicSourceActve;

        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithXFade(activeSource, newSource, musicClip, transtitionTime));
    }

    private IEnumerator UpdateMusicWithXFade(AudioSource original, AudioSource newSource, AudioClip music, float transtitionTime)
    {
        // Make sure the source is active and playing
        if (!original.isPlaying)
            original.Play();

        newSource.Stop();
        newSource.clip = music;
        newSource.Play();

        float t = 0.0f;

        for (t = 0.0f ; t <= transtitionTime; t += Time.deltaTime)
        {
            original.volume = musicVolume - ((t / transtitionTime) * musicVolume);
            newSource.volume = (t / transtitionTime) * musicVolume;
            yield return null;
        }

        original.volume = 0;
        newSource.volume = musicVolume;

        original.Stop();

    }
}
