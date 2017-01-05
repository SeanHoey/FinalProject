using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
    GameObject Player;
    public GameObject InvObj;
    float Distance;
    float ShopDistance = 20f;
    bool ShopOpen = false;
    float RepeatTime = 0.5f;
    float ButtonPressed;
    Inventory inv;
    PlayersStat Stat;
    public Texture2D BG;
    public GUIStyle Textfont;
    public GUIStyle Selectfont;
    int Healthnum=0;
    int Smokenum=0;
    int Price=0;
    int ListIndex;

	// Use this for initialization
	void Start () {
        ButtonPressed = Time.unscaledTime;
        inv = InvObj.GetComponent<Inventory>();
       
        GameObject[] List = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in List)
        {
            if (g.name.Contains("Clone"))
            {

            }
            else
            {
                Player = g;
            }
        }
        Stat = Player.GetComponent<PlayersStat>();
	}
	
	// Update is called once per frame
	void Update () {
        Distance = Vector3.Distance(Player.transform.position, transform.position);
        Price = 10 * ((1 * Healthnum) + (1 * Smokenum));
        if (Distance <= ShopDistance)
        {
            if (Input.GetButton("Square"))
            {
                ShopOpen = true;
                Time.timeScale = 0;
            }
        }
        if (ShopOpen)
        {
            if (Input.GetAxis("DPadUpDown") > 0.5f && Time.unscaledTime > ButtonPressed && ListIndex > 0)
            {
                ListIndex -= 1;
                Healthnum=0;
                Smokenum=0;
                ButtonPressed = Time.unscaledTime + RepeatTime;
            }
            if (Input.GetAxis("DPadUpDown") < -0.5f && Time.unscaledTime > ButtonPressed && ListIndex < 3)
            {
                Healthnum = 0;
                Smokenum = 0;
                ListIndex += 1;
                ButtonPressed = Time.unscaledTime + RepeatTime;
            }
            if (Input.GetButton("X") && Time.unscaledTime > ButtonPressed)
            {
                Select();
                ButtonPressed = Time.unscaledTime + RepeatTime;
            }
            if (ListIndex == 0)
            {
                if (Input.GetAxis("DPadLeftRight") < -0.5f && Time.unscaledTime > ButtonPressed && Healthnum > 0)
                {
                    Healthnum -= 1;
                    ButtonPressed = Time.unscaledTime + RepeatTime;
                }
                if (Input.GetAxis("DPadLeftRight") > 0.5f && Time.unscaledTime > ButtonPressed)
                {
                    Healthnum += 1;
                    ButtonPressed = Time.unscaledTime + RepeatTime;
                }
            }
            else if (ListIndex == 1)
            {
                if (Input.GetAxis("DPadLeftRight") < -0.5f && Time.unscaledTime > ButtonPressed && Smokenum > 0)
                {
                    Smokenum -= 1;
                    ButtonPressed = Time.unscaledTime + RepeatTime;
                }
                if (Input.GetAxis("DPadLeftRight") > 0.5f && Time.unscaledTime > ButtonPressed)
                {
                    Smokenum += 1;
                    ButtonPressed = Time.unscaledTime + RepeatTime;
                }
            }
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
    void OnGUI()
    {
        if (ShopOpen)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BG);
            GUI.TextField(new Rect(Screen.width * 0.2f, Screen.height * 0.2f, 100f, 100f), "Prize: "+Price.ToString(), Textfont);
            GUI.DrawTexture(new Rect(Screen.width * 0.2f, Screen.height * 0.4f, 100f, 100f), inv.Box);
            GUI.DrawTexture(new Rect(Screen.width * 0.2f, Screen.height * 0.4f, 100f, 100f), inv.ConsumablesTexture[0]);
            GUI.TextField(new Rect(Screen.width * 0.2f, Screen.height * 0.4f, 100f, 100f), inv.Consumables[0].ToString());
            GUI.TextField(new Rect(Screen.width * 0.6f, Screen.height * 0.4f, 100f, 100f), Healthnum.ToString(),GetFont(0));
            GUI.DrawTexture(new Rect(Screen.width * 0.2f, Screen.height * 0.6f, 100f, 100f), inv.Box);
            GUI.DrawTexture(new Rect(Screen.width * 0.2f, Screen.height * 0.6f, 100f, 100f), inv.ConsumablesTexture[1]);
            GUI.TextField(new Rect(Screen.width * 0.2f, Screen.height * 0.6f, 100f, 100f), inv.Consumables[1].ToString());
            GUI.TextField(new Rect(Screen.width * 0.6f, Screen.height * 0.6f, 100f, 100f), Smokenum.ToString(),GetFont(1));
            GUI.TextField(new Rect(Screen.width * 0.6f, Screen.height * 0.8f, 100f, 100f), "Back", GetFont(2));
        }
    }

    void Select()
    {
        if (ListIndex == 0)
        {
            if (Stat.Coins >= Price)
            {
                inv.BoughtHeath(Healthnum);
                Stat.TakeCoins(Price);
            }
        }
        else if (ListIndex == 1)
        {
            if (Stat.Coins >= Price)
            {
                inv.BoughtSmoke(Smokenum);
                Stat.TakeCoins(Price);
            }
        }
        else if (ListIndex == 2)
        {
            Time.timeScale = 1;
            ShopOpen = false;
        }
    }

}
