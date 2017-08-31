using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 他のクラスからAudioSourceをもらって再生を管理する
/// </summary>
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance = null;

    private AudioSource currentBGM = null;
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
        DontDestroyOnLoad(gameObject);
    }
    
    public void PlayBGM(AudioSource bgm)
    {
        if (bgm == null) // BGMなしのシーン
        {
            Destroy(currentBGM.gameObject);
            currentBGM = null;
        }
        else if (currentBGM == null || currentBGM.GetComponent<AudioSource>().clip != bgm.clip) // 何も流していないか、違うBGMがインプットされた
        {
            if (currentBGM != null) // すでに何か流しているなら消す
                Destroy(currentBGM.gameObject);

            currentBGM = Instantiate(bgm, this.transform);
            currentBGM.Play();
        }
    }

    public void PlaySound(AudioSource audioSource)
    {
        StartCoroutine(CoPlaySound(audioSource));
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
