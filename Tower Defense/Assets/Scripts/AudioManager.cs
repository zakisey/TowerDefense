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
    private float SEVolumeRate, BGMVolumeRate;
    private float pitch = 1f;
    
    private float BGMVolume = 0.3f;


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
    private void Start()
    {
        LoadVolume();
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
            BGMVolume = currentBGM.GetComponent<AudioSource>().volume;
            currentBGM.GetComponent<AudioSource>().volume = BGMVolume * BGMVolumeRate;
            currentBGM.Play();
        }
    }
       
    /// <summary>
    /// SESliderの値が変化したときのみ呼ばれる
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSEVolume(float value)
    {
        SEVolumeRate = value;
    }
    /// <summary>
    /// BGMSliderの値が変化したときのみ呼ばれる
    /// </summary>
    /// <param name="value"></param>
    public void ChangeBGMVolume(float value)
    {
        BGMVolumeRate = value;
        currentBGM.GetComponent<AudioSource>().volume = BGMVolume * value;
    }

    public void PlaySound(AudioSource audioSource)
    {
        StartCoroutine(CoPlaySound(audioSource));
    }

    private IEnumerator CoPlaySound(AudioSource audioSource)
    {
        AudioSource audio = Instantiate(audioSource, this.transform);
        audio.pitch = this.pitch;
        audio.volume *= SEVolumeRate; 
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

    private void LoadVolume()
    {

        if (PlayerPrefs.HasKey("SEVolumeRate"))
        {
            SEVolumeRate = PlayerPrefs.GetFloat("SEVolumeRate");
        }
        else
        {
            SEVolumeRate = 1.0f;
        }

        if (PlayerPrefs.HasKey("BGMVolumeRate"))
        {
            BGMVolumeRate = PlayerPrefs.GetFloat("BGMVolumeRate");
        }
        else
        {
            BGMVolumeRate = 1.0f;
        }

    }

    public void SaveVolumeData()
    {
        PlayerPrefs.SetFloat("SEVolumeRate", SEVolumeRate);
        PlayerPrefs.SetFloat("BGMVolumeRate", BGMVolumeRate);
    }

    public float GetSEVolumeRate() { return SEVolumeRate; }
    public float GetBGMVolumeRate() { return BGMVolumeRate; }
}
