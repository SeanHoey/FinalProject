/*
*Author Sean Hoey X11000759
*Date: 16/11/2014
*/
using UnityEngine;
using System.Collections;


public class NewGame : MonoBehaviour {
    DataManager Data;
	// Use this for initialization
	void Start () {
        Data = GameObject.FindGameObjectWithTag("Data").GetComponent<DataManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void StartGame(){
        Data.reset();
		Application.LoadLevel("Controller");
	}
	void OnMouseDown(){
        StartGame();
	}
}
