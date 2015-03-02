using UnityEngine;
using System.Collections;

public class chestControls : MonoBehaviour {

	public Sprite openedSprite;
	bool opened = false;
	public string[] items;
	public int itemIndex;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnTriggerStay2D(Collider2D coll) {
		if (coll.transform.tag == "player" && Input.GetButtonDown("Interact")){
			if (!opened){
				coll.GetComponent<PlayerInventory>().AddItem(items[itemIndex]); //"high_jump"
				opened = true;
			}
			OpenChest();
		}
	}
	
	void OpenChest(){
			GetComponent<SpriteRenderer>().sprite = openedSprite;
			transform.GetChild(0).gameObject.SetActive(false);
	}
}
