using UnityEngine;
using System.Collections;

public class npcDialogue : MonoBehaviour {

	public int dialogueIndex;
	public bool inDialogue = false;

	// Use this for initialization
	void Start () {
		Dialoguer.Initialize();
	}
	
	// Update is called once per frame
	void OnTriggerStay2D (Collider2D col) {
		if (col.transform.tag == "player"){
			if (Input.GetButtonUp("Interact") && !inDialogue){
				Dialoguer.StartDialogue(0);
				col.SendMessage("DialogueStart", transform);
				inDialogue = true;
			}
		}
	}
	
	public void DialogueEnded(){
		inDialogue = false;
	}
}
