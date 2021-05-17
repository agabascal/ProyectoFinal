using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    [Header("Ambient Audio")]
    public AudioClip ambientClip;
    public AudioClip windClip;
    public AudioClip musicClip;
    public AudioClip music2Clip;
    public AudioClip intromusicClip;

    [Header("Saru Audio")]
    public AudioClip walkStepClip;
    public AudioClip flyClip;
    public AudioClip jumpClip;
    public AudioClip hitClip;
    public AudioClip voiceClip;
    public AudioClip landingClip;
    public AudioClip takeoffClip;
    public AudioClip shootClip;
    public AudioClip attackClip;

    [Header("Enemies Audio")]
    public AudioClip walkSpiderClip;
    public AudioClip walkWormClip;
    public AudioClip walkErizoClip;
    public AudioClip attackSpiderClip;
    public AudioClip attackWormClip;
    public AudioClip deathEnemiesClip;
    public AudioClip hitEnemiesClip;


    [Header("UI Audio")]
    public AudioClip backClip;
    public AudioClip buttonClip;

    [Header("Mixer Groups")]
    public AudioMixerGroup ambientGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup playerGroup;
    public AudioMixerGroup enemiesGroup;
    public AudioMixerGroup uiGroup;

    AudioSource ambientSource;
    AudioSource musicSource;
    AudioSource playerSource;
    AudioSource enemiesSource;
    AudioSource uiSource;


    private void Awake()
    {
        if(current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;
        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        musicSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        playerSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        enemiesSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        uiSource = gameObject.AddComponent<AudioSource>() as AudioSource;

        ambientSource.outputAudioMixerGroup = ambientGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        enemiesSource.outputAudioMixerGroup = enemiesGroup;
        uiSource.outputAudioMixerGroup = uiGroup;

        StartLevel1Audio();
    }

    void StartLevel1Audio()
    {
        ambientSource.clip = ambientClip;
        ambientSource.loop = true;
        ambientSource.Play();

        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    void StartLevel2Audio()
    {
        musicSource.clip = music2Clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public static void PlayFootStepAudio()
    {
        if(current == null || current.playerSource.isPlaying)
        {
            return;
        }

        current.playerSource.clip = current.walkStepClip;
        current.playerSource.Play();
    }

    public static void PlayJumpAudio()
    {
        if(current == null)
        {
            return;
        }

        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();
    }

    public static void PlayDeathAudio()
    {
        if(current == null)
        {
            return;
        }

        current.playerSource.clip = current.hitClip;
        current.playerSource.Play();
    }

    public static void PlayFlyAudio()
    {
        if (current == null)
        {
            return;
        }

        current.playerSource.clip = current.flyClip;
        current.playerSource.Play();
    }

    public static void PlayLandingAudio()
    {
        if (current == null)
        {
            return;
        }

        current.playerSource.clip = current.landingClip;
        current.playerSource.Play();

    }

    public static void PlayTakeoffAudio()
    {
        if (current == null)
        {
            return;
        }

        current.playerSource.clip = current.takeoffClip;
        current.playerSource.Play();
    }

    public static void PlayVoiceSaruAudio()
    {
        if (current == null)
        {
            return;
        }

        current.playerSource.clip = current.voiceClip;
        current.playerSource.Play();
    }

    public static void PlayShootAudio()
    {
        if (current == null)
        {
            return;
        }

        current.playerSource.clip = current.shootClip;
        current.playerSource.Play();
    }

    public static void PlayAttackSaruAudio()
    {
        if (current == null)
        {
            return;
        }

        current.playerSource.clip = current.attackClip;
        current.playerSource.Play();
    }

    public static void PlayAttackSpiderAudio()
    {
        if (current == null)
        {
            return;
        }

        current.enemiesSource.clip = current.attackSpiderClip;
        current.enemiesSource.Play();
    }

    public static void PlayAttackWormAudio()
    {
        if (current == null)
        {
            return;
        }

        current.enemiesSource.clip = current.attackWormClip;
        current.enemiesSource.Play();
    }

    public static void PlayWalkWormAudio()
    {
        if (current == null)
        {
            return;
        }

        current.enemiesSource.clip = current.walkWormClip;
        current.enemiesSource.Play();
    }

    public static void PlayWalkSpiderAudio()
    {
        if (current == null)
        {
            return;
        }

        current.enemiesSource.clip = current.walkSpiderClip;
        current.enemiesSource.Play();
    }

    public static void PlayWalkErizoAudio()
    {
        if (current == null)
        {
            return;
        }

        current.enemiesSource.clip = current.walkErizoClip;
        current.enemiesSource.Play();
    }

    public static void PlayDeathEnemiesAudio()
    {
        if (current == null)
        {
            return;
        }

        current.enemiesSource.clip = current.deathEnemiesClip;
        current.enemiesSource.Play();
    }

    public static void PlayHitEnemiesAudio()
    {
        if (current == null)
        {
            return;
        }

        current.enemiesSource.clip = current.hitEnemiesClip;
        current.enemiesSource.Play();
    }

    public static void PlayBackButtonAudio()
    {
        if (current == null)
        {
            return;
        }

        current.uiSource.clip = current.backClip;
        current.uiSource.Play();
    }

    public static void PlayButtonAudio()
    {
        if (current == null)
        {
            return;
        }

        current.uiSource.clip = current.buttonClip;
        current.uiSource.Play();
    }
}
