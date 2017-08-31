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
        OptionPanel.SetActive(false);
        // TODO:AudioManagerから音量をもらってSliderのvalueを初期化する


        // AudioManagerの音量に変更を加える
        SESlider.onValueChanged.AddListener(delegate { ChangeSE(); });
        BGMSlider.onValueChanged.AddListener(delegate { ChangeBGM(); });

    }

    //Sliderの値が変化したときのみ呼び出される
    private void ChangeSE()
    {
        print(SESlider.value + "に変化しました");

    }
    private void ChangeBGM()
    {
        print(BGMSlider.value + "に変化しました");

    }



    public void ShowOption()
    {
        OptionPanel.SetActive(true);
    }

    public void CloseOption()
    {
        // TODO:音量のデータをAudioManagerを使ってPlayerPrefsに保存する
        OptionPanel.SetActive(false);
    }
}
