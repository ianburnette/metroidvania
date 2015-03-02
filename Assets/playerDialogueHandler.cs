using UnityEngine;
using System.Collections;

public class playerDialogueHandler : MonoBehaviour {

	Animator anim;
	playerControls controls;
	gunControls gunConts;
	Transform npcTalkingTo;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		controls = GetComponent<playerControls>();
		gunConts = GetComponent<gunControls>();
	}
	
	// Update is called once per frame
	public void DialogueStart(Transform npcTalking) {
		print (npcTalking);
		npcTalkingTo = npcTalking;
		anim.SetBool("moving", false);
		controls.enabled = false;
		gunConts.enabled = false;
	}
	
	public void EndDialogue(){
		print ("ended");
		controls.enabled = true;
		gunConts.enabled = true;
		npcTalkingTo.SendMessage("DialogueEnded");
	}
}
