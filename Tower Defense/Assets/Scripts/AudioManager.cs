using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null;

    public AudioSource BGM;

    public AudioSource defaultCannonAudio;
    public AudioSource defaultMissileAudio;
    public AudioSource extraCannonAudio;
    public AudioSource extraMissileAudio;

    private float pitch = 1f;

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
        StartCoroutine(CoPlaySound(BGM));
    }

    /// <summary>
    /// 音声タイプで流す効果音を決める
    /// </summary>
    /// <param name="audioType">CannonかMissile</param>
    public void PlaySound(string audioType)
    {
        // 特殊の効果音がある場合、確率でそれを流す
        switch(audioType.ToLower())
        {
            case "cannon":
                if (extraCannonAudio == null || Random.Range(0.0f, 4.0f) < 3.0f) // 4分の1
                    StartCoroutine(CoPlaySound(defaultCannonAudio));
                else
                    StartCoroutine(CoPlaySound(extraCannonAudio));
                break;
            case "missile":
                if (extraMissileAudio == null || Random.Range(0.0f, 3.0f) < 1.0f) // 3分の2
                    StartCoroutine(CoPlaySound(defaultMissileAudio));
                else
                    StartCoroutine(CoPlaySound(extraMissileAudio));
                break;
            default:
                break;
        }
    }

    private IEnumerator CoPlaySound(AudioSource audioSource)
    {
        AudioSource audio = Instantiate(audioSource, this.transform);
        audio.pitch = this.pitch;
        audio.Play();

        yield return new WaitWhile(() => audio.isPlaying);
        Destroy(audio.gameObject);
    }

    public void SetPitchResume()
    {
        pitch = 1f;
    }

    public void SetPitchFastForward()
    {
        pitch = 2f;
    }
}
