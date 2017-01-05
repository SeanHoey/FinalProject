/*
*Author Sean Hoey X11000759
*Date: 10/11/2014
*/
using UnityEngine;
using System.Collections;

public class PlayersStat : MonoBehaviour {
	public bool Alive=true;
    public float Level = 0;
    int Exp = 0;
    int ExpRequired = 0;
    int Damage = 0;
    public int Coins = 0;
	public bool Blocking=false;
	public float MaxHealth=0;
	public float Health=1;
    public AudioClip DeathScream;
    DataManager DataSource;
    CombatScript Combat;
    public GUIStyle StatStyle;
    public Texture2D HealthBar;
    public Texture2D ExpBar;
    public Texture2D InfoBar;
	
	// Use this for initialization
	void Start () {
        DataSource = GameObject.FindGameObjectWithTag("Data").GetComponent<DataManager>();
        Combat = gameObject.transform.GetChild(0).gameObject.GetComponent<CombatScript>();
        Level = DataSource.GetLevel();
        Exp = DataSource.GetExp();
        Coins = DataSource.GetCoins();
        SetStats();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LevelUp()
    {
        Exp -= ExpRequired;
        Level += 1;
        DataSource.Level = Level;
        DataSource.Exp = Exp;
        DataSource.Change = true;
        SetStats();
    }

	void OnGUI(){
        GUI.DrawTexture(new Rect(Screen.width * 0.8f, Screen.height*0.72f, 120, 30), HealthBar);
        GUI.DrawTexture(new Rect(Screen.width * 0.8f, Screen.height * 0.8f, 120, 30), ExpBar);
        GUI.DrawTexture(new Rect(Screen.width * 0.8f, Screen.height * 0.88f, 120, 30), InfoBar);
        GUI.TextField(new Rect(Screen.width * 0.81f, Screen.height * 0.73f, 100, 20), "health: " + Health, StatStyle);
        GUI.TextField(new Rect(Screen.width * 0.81f, Screen.height * 0.81f, 100, 20), "EXP: " + Exp + "/" + ExpRequired , StatStyle);
        GUI.TextField(new Rect(Screen.width * 0.81f, Screen.height * 0.89f, 100, 20), "Coins: " + Coins, StatStyle);
	}

    void SetStats()
    {
        ExpRequired = 1000 * (int)(Mathf.Pow(Level, 2f));
        Damage = 25 + (25 * ((int)Level));
        Health = 100 +(50*(Level+1));
        if (DataSource.version > 0 && DataSource.Health < Health)
        {
            Health = DataSource.CurrentHealth;
        }
        MaxHealth = 100 + (50 * (Level + 1));
        Combat.Damage = Damage;
    }

	void TakenDamage(float Damage){
		if(Alive){
			audio.Play();
			if(Blocking){
				Damage=Damage*0.25f;
			}
            Combat.AddAttackTime();
            Combat.combo = 0;
			Health-=Damage;
            DataSource.CurrentHealth = Health;
			if(Health<=0){
                Animator anim = gameObject.GetComponent<Animator>();
                anim.SetBool("EnemyNear", false);
				Alive=false;
				Dead();
			}
		}
	}

	public void HealthRestored(){
		if (Health < MaxHealth) {
			Health=MaxHealth;
            DataSource.CurrentHealth = Health;
		}
	}
    void GainEXP(int exp)
    {
        exp += Combat.highcombo * 10;
        Combat.highcombo = 0;
        Exp += exp;
        DataSource.Exp = Exp;
        if (exp > ExpRequired)
        {
            LevelUp();
        }
        Combat.EnemyNotNear();
    }

    void GainCoins(int x)
    {
        Coins += x * 10;
        DataSource.Coins = Coins;
    }
    
	void Dead(){
        audio.clip = DeathScream;
        audio.Play();
		Respawn RespawnMenu=gameObject.transform.parent.gameObject.GetComponent<Respawn>();
        RespawnMenu.PlayerDied();
		Debug.Log("player Died");
	}

	public void ChangeBlock(bool block){
		Blocking=block;
	}

    public void TakeCoins(int x)
    {
        Coins -= x;
        DataSource.Coins = Coins;
    }
}
