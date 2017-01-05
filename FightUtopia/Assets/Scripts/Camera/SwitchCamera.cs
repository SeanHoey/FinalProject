/*
*Author Sean Hoey X11000759
*Date: 16/11/2014
*/
using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {
	public GameObject OculusRift,Cameara;
    
	// Use this for initialization
	void Start () {
		OculusRift=gameObject.transform.GetChild(1).gameObject;
		Cameara=gameObject.transform.GetChild(0).gameObject;
		if(Settings.OculusRift){
			Cameara.SetActive(false);
		}
		else{
			OculusRift.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
