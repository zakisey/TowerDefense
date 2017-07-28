using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButton : MonoBehaviour {

    public GameObject unit;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnClick()
    {
        BoardManager.instance.SetUnitOnSocket(unit);
    }
}
