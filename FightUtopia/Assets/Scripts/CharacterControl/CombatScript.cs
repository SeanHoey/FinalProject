/*
*Author Sean Hoey X11000759
*Date: 10/11/2014
*/
using UnityEngine;
using System.Collections;

public class CombatScript : MonoBehaviour {
	public int Damage=0;
	float Distance;
	float KickDistance=5.5f;
	float PunchDistance=5f;
	float AttacRepeatkTime=0.5f;
	float AttackTime;
	bool Blocking =false;
	bool RightPunch=false;
	bool RightKick=false; 
	public GameObject Player;
    Animator CharAnim;
	PlayersStat Stats;
    bool EnemyNear = false;
    public int combo;
    public int highcombo;
    int LPunch = Animator.StringToHash("LPunch");
    int LKick = Animator.StringToHash("LKick");
    int RPunch = Animator.StringToHash("RPunch");
    int RKick = Animator.StringToHash("RKick");
	
	void Start(){
		AttackTime=Time.time;
        CharAnim = gameObject.transform.parent.gameObject.GetComponent<Animator>();
		Stats=Player.GetComponent<PlayersStat>();
	}

    public void AddAttackTime()
    {
        AttackTime = Time.time + AttacRepeatkTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Circle"))
        {
			RightPunch=true;
			Punch();
		}
        if (Input.GetButton("Triangle"))
        {
			RightPunch=false;
			Punch();
		}
		if(Input.GetButton("X")){
			RightKick=true;
			Kick();
		}
		if(Input.GetButton("Square")){
			RightKick=false;
			Kick();
		}
		if(Input.GetButtonDown("R2")&&Blocking==false){
			ChangeBlock();
		}
        if (Input.GetButtonUp("R2") && Blocking == true)
        {
            ChangeBlock();
		}
	}
	
	void Punch(){
		if(Time.time>AttackTime){
			if(RightPunch){
                CharAnim.SetTrigger(RPunch);
			}
			else{
                CharAnim.SetTrigger(LPunch);
			}
			audio.Play();
            AddAttackTime();
			RaycastHit hit;
			if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hit)){
				Distance=hit.distance;
				if(Distance<PunchDistance){
					hit.transform.SendMessage("ApplyDamage",Damage,SendMessageOptions.DontRequireReceiver);
                    combo++;
                    if (combo > highcombo)
                    {
                        highcombo = combo;
                    }
				}
			}
		}
	}
	
	void Kick(){
		if(Time.time>AttackTime){
			if(RightKick){
				 CharAnim.SetTrigger(RKick);
			}
			else{
				 CharAnim.SetTrigger(LKick);
			}
			audio.Play();
            AddAttackTime();
			RaycastHit hit;
			if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hit)){
				Distance=hit.distance;
				if(Distance<KickDistance){
					hit.transform.SendMessage("ApplyDamage",Damage,SendMessageOptions.DontRequireReceiver);
                    combo++;
                    if (combo > highcombo)
                    {
                        highcombo = combo;
                    }
				}
			}
		}
	}


    public void EnemyNotNear()
    {
        CharAnim.SetBool("EnemyNear", false);
    }
    void ChangeBlock()
    {
        Blocking = !Blocking;
        Stats.ChangeBlock(Blocking);
        CharAnim.SetBool("Blocking", Blocking);
    }
}
