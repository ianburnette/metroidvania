using UnityEngine;
using System.Collections;

public class bulletHit : MonoBehaviour {

	public float gravAmt = 2f;
	public float timeToGrav = 3;
	public int timeToDie = 5;

	// Use this for initialization
	void Start () {
		Invoke("TurnOnGravity", timeToGrav);
		Invoke("DestroySelf", timeToDie);
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D col) {
		if (col.transform.tag == "destroyable"){
			col.SendMessage("BulletHit");
		}
		DestroySelf();
	}
	
	void TurnOnGravity(){
		GetComponent<Rigidbody2D>().gravityScale=gravAmt;
	}
	
	void DestroySelf(){
		Destroy(gameObject);
	}
}


//PondPeters9008