using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour {

    private GameObject OptionPanel;
    private Slider SESlider;
    private Slider BGMSlider;

    // Use this for initialization
    void Start () {
        OptionPanel = GameObject.Find("OptionPanel");
        SESlider = OptionPanel.transform.Find("SESlider").GetComponent<Slider>();
        BGMSlider = OptionPanel.transform.Find("BGMSlider").GetComponent<Slider>();
        // AudioManagerから音量をもらってSliderのvalueを初期化する
        SESlider.value = AudioManager.instance.GetSEVolumeRate();
        BGMSlider.value = AudioManager.instance.GetBGMVolumeRate();
        // AudioManagerの音量に変更を加える
        SESlider.onValueChanged.AddListener(delegate { AudioManager.instance.ChangeSEVolume(SESlider.value); });
        BGMSlider.onValueChanged.AddListener(delegate { AudioManager.instance.ChangeBGMVolume(BGMSlider.value); });
        OptionPanel.SetActive(false);
    }

    public void ShowOption()
    {
        OptionPanel.SetActive(true);
    }

    public void CloseOption()
    {
        // 音量のデータをAudioManagerを使ってPlayerPrefsに保存する
        AudioManager.instance.SaveVolumeData();
        OptionPanel.SetActive(false);
    }
}
