/*
*Author Sean Hoey X11000759
*Date: 16/11/2014
*/
using UnityEngine;
using System.Collections;
using System;

public class PickCamera : MonoBehaviour {
	public bool ButtonCamera =false;
	bool message=false;
	public GUIStyle Style;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if(message==true){
			if(ButtonCamera==true){
				GUI.Label(new Rect((Screen.width-550)*0.5f,(Screen.height-75)*0.5f,50,100),"Normal camera selected.\nNow please select new game.",Style);
			}
			else{
				GUI.Label(new Rect((Screen.width-550)*0.5f,(Screen.height-75)*0.5f,50,100),"Oculus camera selected.\nNow please select new game.",Style);
			}
		}
	}
	
	public void SelectCamera(){
		if(ButtonCamera){
			Settings.OculusRift=false;
		}
		else{
			Settings.OculusRift=true;
		}
		print(Settings.OculusRift);
		StartCoroutine(ShowMessage());
	}
	void OnMouseDown(){
		SelectCamera();
	}
	
	IEnumerator ShowMessage(){
		message=true;
		yield return new WaitForSeconds(2);
		message=false;
	}
}
