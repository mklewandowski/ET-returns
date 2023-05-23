using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    AudioClip MenuSound;

    [SerializeField]
    AudioClip ButtonSound;

    [SerializeField]
    AudioClip PlayerShootSound;

    [SerializeField]
    AudioClip PlayerShoot2Sound;

    [SerializeField]
    AudioClip PlayerBeeSound;

    [SerializeField]
    AudioClip PlayerLaserSound;

    [SerializeField]
    AudioClip ExplodeSound;

    [SerializeField]
    AudioClip EnemyHitSound;

    [SerializeField]
    AudioClip PlayerHitSound;

    [SerializeField]
    AudioClip PlayerDieSound;

    [SerializeField]
    AudioClip CollectCandySound;

    [SerializeField]
    AudioClip CollectPhoneSound;

    [SerializeField]
    AudioClip PhoneDialSound;

    [SerializeField]
    AudioClip LevelUpSound;

    [SerializeField]
    AudioClip BossLandSound;

    [SerializeField]
    AudioClip[] ClickSounds = new AudioClip[4];

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        audioSource = this.GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        audioSource.Play();
    }
    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PlayMenuSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(MenuSound, 1f);
    }

    public void PlayButtonSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(ButtonSound, 1f);
    }

    public void PlayPlayerShootSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PlayerShootSound, .75f);
    }

    public void PlayEnemyHitSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(EnemyHitSound, .5f);
    }

    public void PlayPlayerHitSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PlayerHitSound, .5f);
    }

    public void PlayPlayerDieSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PlayerDieSound, .5f);
    }

    public void PlayCollectCandySound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(CollectCandySound, .5f);
    }

    public void PlayCollectPhoneSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(CollectPhoneSound, .5f);
    }

    public void PlayLevelUpSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(LevelUpSound, .5f);
    }

    public void PlayPhoneDialSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PhoneDialSound, .5f);
    }

    public void PlayPlayerShoot2Sound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PlayerShoot2Sound, .75f);
    }

    public void PlayPlayerBeeSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PlayerBeeSound, .75f);
    }

    public void PlayPlayerLaserSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(PlayerLaserSound, .5f);
    }

    public void PlayExplodeSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(ExplodeSound, .75f);
    }

    public void PlayBossLandSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(BossLandSound, .75f);
    }

    public void PlayClickSound()
    {
        if (Globals.AudioOn)
        {
            int num = Random.Range(0, ClickSounds.Length - 1);
            audioSource.PlayOneShot(ClickSounds[num], 1f);
        }
    }
}
