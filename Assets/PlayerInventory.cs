using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour {

	public bool highJump, gun, ledgeGrab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddItem(string itemToAdd){
		if (itemToAdd == "high_jump"){
			highJump = true;
		}else if (itemToAdd == "gun"){
			gun = true;
			GetComponent<gunControls>().haveGun = true;
		}else if (itemToAdd == "ledge_grab"){
			ledgeGrab = true;
			GetComponent<playerControls>().canGrab = true;
		}
	}
}
