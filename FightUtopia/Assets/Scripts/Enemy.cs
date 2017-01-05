/*
*Author Sean Hoey X11000759
*Date: 10/11/2014
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
    public int Level;
    int Exp;
	public int Health = 100;
	bool Alive=true;
	float Distance;
	public GameObject Target;
	float LookDistance=50.0f;
	float CharacterRange=30f;
    float MessageRange = 15f;
	float AttackRange=8f;
	public float MoveSpeed=10.0f;
	float Damping=4.0f;
	float Gravity=20.0f;
	Vector3 MoveDirection=Vector3.zero;
	CharacterController Controller;	
	float Damage=40;
	float AttacRepeatkTime=1.5f;
	float AttackTime;
    Animation Anim;
    List<string> Clips = new List<string>();
    bool Animbool = false;
	public AudioClip DeathAudio;
	public AudioClip HitAudio;
	
	// Use this for initialization
	void Start () {
		Controller=gameObject.GetComponent<CharacterController>();
        if (gameObject.GetComponent<Animation>()!=null)
        {
            Anim = gameObject.GetComponent<Animation>();
            Animbool = true;
            foreach (AnimationState state in Anim)
            {
                Clips.Add(state.name);
            }
        }
        GameObject[] List = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in List)
        {
            if (g.name.Contains("Clone"))
            {

            }
            else
            {
                Target = g;
            }
        }
		AttackTime=Time.time;
        SetStats();
	}
    void SetStats()
    {
        Exp = 100 + (100 * (Level-1));
        Health = 100 +(50* Level);
        Damage = 25 + (25* (Level-1));
    }
	// Update is called once per frame
	void Update () {
		if(Alive&&Target.activeSelf==true){
			transform.eulerAngles=new Vector3(0,transform.eulerAngles.y,0);
			Distance=Vector3.Distance(Target.transform.position,transform.position);
            if(Distance<LookDistance){
				LookAt();
                Target.SendMessage("EnemyNotNear", SendMessageOptions.DontRequireReceiver);
			}
			if(Distance<AttackRange){
				Attack();
			}
			if(Distance<CharacterRange){
				Chase();
                Target.SendMessage("EnemyIsNear", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void ApplyDamage(int Damage){
		if(Alive==true){
			audio.Play();
			Health-= Damage;
            AttackTime = Time.time + 0.5f;
            transform.position += (transform.forward * -8);
			if(Health<=0){
				Alive=false;
				StartCoroutine(Dead());
			}
		}
	}
	
	IEnumerator Dead(){
		audio.clip=DeathAudio;
		audio.Play();
        particleSystem.Play();
        Target.SendMessage("GainEXP", Exp, SendMessageOptions.DontRequireReceiver);
        Target.SendMessage("GainCoins", Level, SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds(1);
		DestroyObject();	
	}
	void DestroyObject(){
		Destroy(gameObject);
	}
	void LookAt(){
		Quaternion rotation=Quaternion.LookRotation(Target.transform.position-transform.position);
		transform.rotation=Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime*Damping);
	}
	
	void Chase(){
            MoveDirection = transform.forward;
            MoveDirection *= MoveSpeed;
            MoveDirection.y = 0;
            Controller.Move(MoveDirection * Time.deltaTime);
	}
	
	void Attack(){
		if(Time.time>AttackTime){
			Target.SendMessage("TakenDamage",Damage,SendMessageOptions.DontRequireReceiver);
			AttackTime=Time.time+AttacRepeatkTime;
			print("attack");
            int random = UnityEngine.Random.Range(0, Clips.Count);
            Anim.Play(Clips[random]);
		}
	}
}
