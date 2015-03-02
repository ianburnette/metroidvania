using UnityEngine;
using System.Collections;

public class GunControlsNonAiming : MonoBehaviour {
	
	public float lowerArmWaitTime = 1f;
	public bool haveGun = false;
	public bool haveScanner = false;
	bool armUp;
	public Transform gunArm, gunBarrel, bulletPrefab;
	public Vector2 rightPos, leftPos;
	public float bulletSpeed;
	SpriteRenderer armSprite;
	public bool aiming, aimingScanner;
	playerControls controlScript;
	
	void Start () {
		controlScript = GetComponent<playerControls>();
		armSprite = gunArm.GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		GetInput();
		Animate();
		MaintainArmPosition();
	}
	
	void GetInput(){
		if (haveGun && Input.GetButton("Aim")){
			aiming=true;
		}else{
			aiming=false;
		}
		if (aiming && Input.GetButtonDown("Fire")){
			Fire();
		}
		if (haveGun && !aiming && Input.GetButtonDown("Fire")){
			CancelInvoke("LowerArm");
			armUp = true;
			Fire();
			Invoke("LowerArm", lowerArmWaitTime);
		}
	}
	
	void Fire(){
		Transform newBullet = (Transform)Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity);
		newBullet.rigidbody2D.velocity = gunArm.up * bulletSpeed * gunArm.localScale.y;
	}
	
	void LowerArm(){
		armUp = false;
	}
	
	void Animate(){
		if (aiming ||armUp) {
			armSprite.enabled = true;
		}else{
			armSprite.enabled = false;	
		}
	}
	
	void MaintainArmPosition(){
		if (controlScript.facingRight){
			gunArm.transform.position = (Vector2)transform.position + rightPos;
			gunArm.localScale = new Vector3(1,1,1);
		}else{
			gunArm.transform.position = (Vector2)transform.position + leftPos;
			gunArm.localScale = new Vector3(1,-1,1);
		}
	}
}
