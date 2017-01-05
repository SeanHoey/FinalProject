/*
*Author Sean Hoey X11000759
*Date: 30/01/2015
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Dictionary<int,int> Consumables=new Dictionary<int,int>();
    public List<Texture2D> ConsumablesTexture = new List<Texture2D>();
    public List<GameObject> Items = new List<GameObject>();
    public GameObject ItemsPa;
    public Texture2D Box;
    public GameObject ItemUsed;
    public GameObject Player;
    DataManager Data;
    int ConsumableIndex=0;
    float RepeatTime = 0.5f;
    float ButtonPressed;
    Consumable ConsumableScript;

    void Awake()
    {
        Data = GameObject.FindGameObjectWithTag("Data").GetComponent<DataManager>();
       
        for (int i = 0; i < Items.Count;i++ )
        {
            Consumables.Add(i, Data.GetInventory(i));
           
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        ButtonPressed = Time.time;
        for (int i = 0; i < 10; i++)
        {
            Createitem(0);
            Createitem(1);
        }
    }
    

    void Update()
    {
        if (Time.timeScale > 0) { 
            if (Input.GetAxis("DPadUpDown") > 0.5f&&Time.time>ButtonPressed)
            {
                ConsumableIndex = 0;
                ButtonPressed = Time.time + RepeatTime;
            }
            if (Input.GetAxis("DPadUpDown") < -0.5f && Time.time > ButtonPressed)
            {
                ConsumableIndex = 1;
                ButtonPressed = Time.time + RepeatTime;
            }
            if (Input.GetButton("L1") && Time.time > ButtonPressed)
            {
                ButtonPressed = Time.time + RepeatTime;
                UseItem(ConsumableIndex);
            }
        }
    }
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width * 0.05f,Screen.height*0.7f , 100f, 100f), Box);
        GUI.DrawTexture(new Rect(Screen.width * 0.05f, Screen.height * 0.7f, 100f, 100f), ConsumablesTexture[ConsumableIndex]);
        GUI.TextField(new Rect(Screen.width * 0.05f, Screen.height * 0.7f, 100f, 100f), Consumables[ConsumableIndex].ToString());
    }

    public void AddItem(int itemId)
    {
        Consumables[itemId] += 1;
        Data.Health = Consumables[0];
        Data.Smoke = Consumables[1];
    }

    public void UseItem(int itemId)
    {
        if (Consumables[ConsumableIndex] > 0)
        {
            Consumables[ConsumableIndex] -= 1;
            ItemUsed = (GameObject)Instantiate(Items[ConsumableIndex], new Vector3(Player.transform.position.x, 0, Player.transform.position.z), Quaternion.identity);
            ItemUsed.transform.eulerAngles = new Vector3(270, 0, 0);
            ConsumableScript = ItemUsed.GetComponent<Consumable>();
            ConsumableScript.ItemBeingUsed = true;
            ConsumableScript.ItemAffect();
        }
    }

    public void BoughtHeath(int x)
    {
        Consumables[0] += x;
        Data.Health = Consumables[0];
    }
    public void BoughtSmoke(int x)
    {
        Consumables[1] += x;
        Data.Smoke = Consumables[1];
    }

    void Createitem(int obj)
    {
        float x = UnityEngine.Random.Range(-240, 240);
        float z = UnityEngine.Random.Range(-240, 240);
        GameObject it;
        if(obj==0){
            it = (GameObject)Instantiate(Items[obj], new Vector3(x, 0, z), Quaternion.Euler(new Vector3(-90, 0, 0)));
            it.transform.parent = ItemsPa.transform;
        }
        else{
            it = (GameObject)Instantiate(Items[obj], new Vector3(x, 1, z), Quaternion.identity);
            it.transform.parent = ItemsPa.transform;
        }
    }
}
