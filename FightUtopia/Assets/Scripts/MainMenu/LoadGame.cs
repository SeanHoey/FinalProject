/*
*Author Sean Hoey X11000759
*Date: 16/11/2014
*/
using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {
    DataManager Data;
    // Use this for initialization
    void Start()
    {
        Data = GameObject.FindGameObjectWithTag("Data").GetComponent<DataManager>();
        Data.ReadStats_FromDisk();
        if (Data.version == 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        Application.LoadLevel("Controller");
    }
    void OnMouseDown()
    {
        StartGame();
    }
}
