using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

	public AudioSource bgm;

	// Use this for initialization
	void Start () {
		AudioManager.instance.PlayBGM(bgm);
	}
}
