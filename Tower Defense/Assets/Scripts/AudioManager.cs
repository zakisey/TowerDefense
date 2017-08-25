using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null;

    public AudioSource cannonAudio;
    public AudioSource missileAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string audioType)
    {
        if (audioType.ToLower() == "cannon")
            StartCoroutine(CoPlaySound(cannonAudio));
        else if (audioType.ToLower() == "missile")
            StartCoroutine(CoPlaySound(missileAudio));
    }

    private IEnumerator CoPlaySound(AudioSource audioSource)
    {
        AudioSource audio = Instantiate(audioSource, this.transform);
        audio.Play();

        yield return new WaitWhile(() => audio.isPlaying);
        Destroy(audio.gameObject);
    }
}
