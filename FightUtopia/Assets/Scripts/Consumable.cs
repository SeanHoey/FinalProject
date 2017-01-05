using UnityEngine;
using System.Collections;
using System;

public class Consumable : MonoBehaviour {
    public enum ItemType
    {
        HealthPotion,
        Smoke,
    };
    public ItemType TypeofItem;
    Inventory InventoryScript;
    ParticleSystem SmokeEffect;
    public bool ItemBeingUsed = false;
	// Use this for initialization
	void Start () {
	    InventoryScript=GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();;
	}
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player"&&ItemBeingUsed==false)
        {
            int itemId = (int)TypeofItem;
            InventoryScript.AddItem(itemId);
            Destroy(gameObject);
        }

    }
	// Update is called once per frame
	void Update () {
	
	}
    public void ItemAffect(){
        if (TypeofItem == ItemType.HealthPotion)
        {
            PlayersStat Stats;
            Stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayersStat>();
            Stats.HealthRestored();
            Destroy(gameObject);
        }
        if(TypeofItem == ItemType.Smoke){
            SmokeEffect = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
            MeshRenderer Rend = gameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
            Rend.enabled=false;
            SmokeEffect.Play();
            StartCoroutine(DestroySmokeBomb());
        }
    }

    IEnumerator DestroySmokeBomb()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
