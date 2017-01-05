/*
*Author Sean Hoey X11000759
*Date: 22/11/2014
*/
using UnityEngine;
using System.Collections;

public class HeatlBarrels : MonoBehaviour {
	public GameObject Player;
	PlayersStat Stats;
	public int RestorePoints=50;
	MeshRenderer Render;

	// Use this for initialization
	void Start () {
		Stats=Player.GetComponent<PlayersStat>();
		Render = gameObject.GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void ApplyDamage(int Damage){
		Stats.HealthRestored();
		Render.enabled = false;
//		particleSystem.Play ();
		StartCoroutine(DestroyBarrel());
	}

	IEnumerator DestroyBarrel(){
		particleSystem.Play ();
		yield return new WaitForSeconds (1.4f);
		Destroy(gameObject);
	}
}
