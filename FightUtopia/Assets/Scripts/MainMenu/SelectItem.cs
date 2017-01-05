/*
*Author Sean Hoey X11000759
*Date: 14/01/2015
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectItem : MonoBehaviour {
	public List<GameObject> MenuItem=new List<GameObject>();
	public int MenuIndex=0;
	TextMesh MenuMesh;
	Color StartColour;
	float SelectTime;

	// Use this for initialization
	void Start () {
		SelectTime=Time.time;
		GetChildren();
        DataManager data =GameObject.FindGameObjectWithTag("Data").GetComponent<DataManager>();
        if (data.version == 0)
        {
            MenuItem.RemoveAt(0);
        }
		MenuMesh=MenuItem[MenuIndex].GetComponent<TextMesh>();
		StartColour=MenuMesh.color;
		MenuMesh.color=Color.red;
	}
	
	void GetChildren(){
		foreach(Transform child in transform){
			if(child.gameObject.activeSelf){
			MenuItem.Add (child.gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
//	Debug.Log(SelectTime+" > "+Time.time);
		if(Input.GetAxis("LeftAnologUpDown")<-0.5f&&SelectTime<Time.time){
			SelectTime=Time.time+0.5f;
			ChangeColourDown();
		}
		if(Input.GetAxis("LeftAnologUpDown")>0.5f&&SelectTime<Time.time){
			SelectTime=Time.time+0.5f;
			ChangeColourUp();
		}
		if(Input.GetAxis("DPadUpDown")>0.5f&&SelectTime<Time.time){
            SelectTime = Time.time + 0.5f;
			ChangeColourUp();
		}
		if(Input.GetAxis("DPadUpDown")<-0.5f&&SelectTime<Time.time){
            SelectTime = Time.time + 0.5f;
			ChangeColourDown();
		}
		if(Input.GetButton("X")){
            SelectTime = Time.time + 0.5f;
			ItemSelected();
		}
	}
	
	void ChangeColourDown(){
		if(MenuIndex<MenuItem.Count-1){
			MenuMesh.color=StartColour;
			MenuIndex++;
			MenuMesh=MenuItem[MenuIndex].GetComponent<TextMesh>();
			MenuMesh.color=Color.red;
		}		
	}
	
	void ChangeColourUp(){
		if(MenuIndex>0){
			MenuMesh.color=StartColour;
			MenuIndex--;
			MenuMesh=MenuItem[MenuIndex].GetComponent<TextMesh>();
			MenuMesh.color=Color.red;
			
		}
	}
	
	void ItemSelected(){
        if (MenuItem.Count - MenuIndex == 5)
        {
            LoadGame Load = MenuItem[MenuIndex].GetComponent<LoadGame>();
            Load.StartGame();
        }
		else if(MenuItem.Count-MenuIndex==4){
			NewGame Game=MenuItem[MenuIndex].GetComponent<NewGame>();
			Game.StartGame();
		}
        else if (MenuItem.Count - MenuIndex == 3)
        {
			PickCamera pick=MenuItem[MenuIndex].GetComponent<PickCamera>();
			pick.SelectCamera();
		}
        else if (MenuItem.Count - MenuIndex == 2)
        {
			PickCamera pick=MenuItem[MenuIndex].GetComponent<PickCamera>();
			pick.SelectCamera();
		}
        else if (MenuItem.Count - MenuIndex == 1)
        {
            QuitGame Quit = MenuItem[MenuIndex].GetComponent<QuitGame>();
            Quit.Quit();
        }
	}
}
