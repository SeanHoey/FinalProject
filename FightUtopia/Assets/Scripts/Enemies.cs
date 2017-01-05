using UnityEngine;
using System.Collections;
using System;


public class Enemies : MonoBehaviour {
    PlayersStat Stat;
    int Numberenemies;
    public GameObject SChar,BigChar,MChar;
    bool Once=false;
    Enemy ScriptEnemy;


	// Use this for initialization
	void Start () {
        Stat= MChar.GetComponent<PlayersStat>();
        StartCoroutine(StartEnemies());
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.childCount < Numberenemies && Once == false)
        {
            Once = true;
            CheckNumber();
        }
	}

    void CheckNumber()
    {
        int NumberCreate = Numberenemies - gameObject.transform.childCount;
        for (int i = 0; i < NumberCreate; i++)
        {
            CreateEnemy();
        }
        Once = false;
    }

    void CreateEnemy()
    {
        float select = UnityEngine.Random.Range(0, 10);
        Debug.Log(select);
        GameObject enemy;
        float x =  UnityEngine.Random.Range(-240, 240);
        float z = UnityEngine.Random.Range(-240, 240);
        if (select > 5f)
        {
            enemy = (GameObject)Instantiate(BigChar, new Vector3(x, -1.712346f, z), Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            ScriptEnemy = enemy.GetComponent<Enemy>();
            ScriptEnemy.Level = (int)Stat.Level + UnityEngine.Random.Range(0, 5);
        }
        else
        {
            enemy = (GameObject)Instantiate(SChar, new Vector3(x, 4.863976f, z), Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            ScriptEnemy = enemy.GetComponent<Enemy>();
            ScriptEnemy.Level = (int)Stat.Level + UnityEngine.Random.Range(0, 5);
        }
    }
    IEnumerator StartEnemies()
    {
        yield return new WaitForEndOfFrame();
        Numberenemies = (int)(Stat.Level * 10);
        for (int i = 0; i < Numberenemies; i++)
        {
            CreateEnemy();
        }
    }
}
