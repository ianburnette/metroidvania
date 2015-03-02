using UnityEngine;
using System.Collections;

public class bulletHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("DestroySelf", 5f);
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D col) {
		if (col.transform.tag == "destroyable"){
			col.SendMessage("BulletHit");
		}
		DestroySelf();
	}
	
	void DestroySelf(){
		Destroy(gameObject);
	}
}


//PondPeters9008