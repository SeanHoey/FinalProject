/*
*Author Sean Hoey X11000759
*Date: 05/12/2014
*/
using UnityEngine;
using System.Collections;
using System;

public class Pause : MonoBehaviour {
    bool IsPaused = false;
    DataManager Data;
    public GUIStyle Textfont;
    public GUIStyle Selectfont;
    int ListIndex = 0;
    float RepeatTime = 0.4f;
    float ButtonTime;
    bool invetory;
    Inventory inv;
    public Texture2D Icon;
    public Texture2D BG;
    bool Savedbool = false;

	// Use this for initialization
	void Start () {
        ButtonTime=Time.unscaledTime;
        Data=GameObject.FindGameObjectWithTag("Data").GetComponent<DataManager>();
        inv = gameObject.transform.GetChild(2).gameObject.GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Start") && Time.unscaledTime > ButtonTime)
        {
            PauseGame();
            ButtonTime = Time.unscaledTime + RepeatTime;
		}
        if (IsPaused&&!invetory)
        {
            if (Input.GetAxis("DPadUpDown") > 0.5f && Time.unscaledTime > ButtonTime && ListIndex > 0)
            {
                ListIndex -=1;
                ButtonTime = Time.unscaledTime + RepeatTime;
            }
            if (Input.GetAxis("DPadUpDown") < -0.5f && Time.unscaledTime > ButtonTime && ListIndex < 3)
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
        if (invetory)
        {
            if (Input.GetButton("X") && Time.unscaledTime > ButtonTime)
            {
                invetory = false;
                ButtonTime = Time.unscaledTime + RepeatTime;
            }
        }
       
	}
    void OnGUI()
    {
        if (IsPaused)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width,Screen.height), BG);
            GUI.DrawTexture(new Rect(Screen.width * 0.7f, Screen.height * 0.1f, 150f, 150f), Icon);
        }
        if (IsPaused && !invetory)
        {
            GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.2f, 100f, 100f), "Resume", GetFont(0));
            GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, 100f, 100f), "Inventory", GetFont(1));
            GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.6f, 100f, 100f), "Save", GetFont(2));
            GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.8f, 100f, 100f), "Quit", GetFont(3));
        }
        if (invetory)
        {
            GUI.DrawTexture(new Rect(Screen.width * 0.2f, Screen.height * 0.4f, 100f, 100f), inv.Box);
            GUI.DrawTexture(new Rect(Screen.width * 0.2f, Screen.height * 0.4f, 100f, 100f), inv.ConsumablesTexture[0]);
            GUI.TextField(new Rect(Screen.width * 0.2f, Screen.height * 0.4f, 100f, 100f), inv.Consumables[0].ToString());
            GUI.DrawTexture(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, 100f, 100f), inv.Box);
            GUI.DrawTexture(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, 100f, 100f), inv.ConsumablesTexture[1]);
            GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, 100f, 100f), inv.Consumables[1].ToString());
            GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.8f, 100f, 100f), "Back", Selectfont);
        }
        if (Savedbool)
        {
            GUI.TextField(new Rect(Screen.width * 0.2f, Screen.height * 0.4f, 100f, 100f), "Your Progress has been saved",Selectfont);
        }
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
    void PauseGame()
    {
        if (!IsPaused)
        {
            Time.timeScale = 0;
            IsPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            IsPaused = false;
            
        }
    }

    void Select()
    {
        if (ListIndex == 0)
        {
            PauseGame();
        }
        else if (ListIndex == 1)
        {
            invetory = true;
        }
        else if (ListIndex == 2)
        {
            Data.Change = true;
            StartCoroutine(Saved());
        }
        else if (ListIndex == 3)
        {
            Application.LoadLevel(0);
        }
    }

    IEnumerator Saved()
    {
        Savedbool = true;
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + 2.5f)
        {
            yield return null;
        }
        Savedbool = false;
    }
}
