using UnityEngine;
using System.Collections;

public class hitBlockWithBullet : MonoBehaviour {

	public Sprite damagedSprite;
	int health = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void BulletHit () {
		health--;
		if (health == 1){
			GetComponent<SpriteRenderer>().sprite = damagedSprite;
		}else if (health == 0){
			Destroy (gameObject);
		}
	}
}
