/*
*Author Sean Hoey X11000759
*Date: 10/11/2014
*/
using UnityEngine;
using System.Collections;
public class Respawn : MonoBehaviour {
    PlayersStat StatOfPlayer;
    GameObject PlayerObject;
    public bool PlayerIsDead= false;
    Vector3 StartPos;
    Camera PlayerCamera;
    public Texture2D BG;
    public GUIStyle Textfont;
    public GUIStyle Selectfont;
    public Texture2D Icon;
    int ListIndex = 0;
    float RepeatTime = 0.4f;
    float ButtonTime;
    
	// Use this for initialization
	void Start () {
        PlayerObject = gameObject.transform.GetChild(0).gameObject;
        StatOfPlayer = PlayerObject.GetComponent<PlayersStat>();
        StartPos = gameObject.transform.position;
        ButtonTime = Time.unscaledTime;
	}
	
	// Update is called once per frame
	void Update () {
        
		if(PlayerIsDead){
			//lockCamera
            PlayerObject.SetActive(false);
            if (Input.GetAxis("DPadLeftRight") < -0.5f && Time.unscaledTime > ButtonTime && ListIndex > 0)
            {
                ListIndex -= 1;
                ButtonTime = Time.unscaledTime + RepeatTime;
            }
            if (Input.GetAxis("DPadLeftRight") > 0.5f && Time.unscaledTime > ButtonTime && ListIndex < 3)
            {
                ListIndex += 1;
                ButtonTime = Time.unscaledTime + RepeatTime;
            }
            if (Input.GetButton("X") && Time.unscaledTime > ButtonTime)
            {
                Select();
                ButtonTime = Time.unscaledTime + RepeatTime;
            }
		}
	}
	
	void OnGUI(){
		if(PlayerIsDead){
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BG);
            GUI.DrawTexture(new Rect(Screen.width * 0.7f, Screen.height * 0.1f, 150f, 150f), Icon);
		 GUI.TextField(new Rect(Screen.width * 0.3f, Screen.height * 0.4f, 100f, 100f), "Respawn", GetFont(0));
         GUI.TextField(new Rect(Screen.width * 0.6f, Screen.height * 0.4f, 100f, 100f), "Quit", GetFont(1));
		}
		
	}
	
	void RespawnPlayer(){
        DataManager data=GameObject.FindGameObjectWithTag("Data").GetComponent<DataManager>();
        data.CurrentHealth=StatOfPlayer.MaxHealth;
        Application.LoadLevel(1);
	}
    public void PlayerDied()
    {
        PlayerIsDead = true;
    }

    GUIStyle GetFont(int index)
    {
        if (index == ListIndex)
        {
            return Selectfont;
        }
        else
        {
            return Textfont;
        }
    }
    void Select()
    {
        if (ListIndex == 0)
        {
            RespawnPlayer();
        }
        else if (ListIndex == 1)
        {
            Application.LoadLevel(0);
        }
    }
}
