using UnityEngine;
using System.Collections;

public class GunControlsNonAiming : MonoBehaviour {
	
	public float baseLeftOffset = .2f;
	public Sprite[] armAngles;
	public float fireRate = .2f;
	public float lowerArmWaitTime = 1f;
	public bool haveGun = false;
	public bool haveScanner = false;
	bool armUp, nonAimDirectionality;
	public Vector2[] barrelLocations;
	public Vector3[] barrelRotations;
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
			CancelInvoke("Fire");
			InvokeRepeating("Fire", 0, fireRate);
		}
		if (aiming && Input.GetButtonUp("Fire")){
			CancelInvoke("Fire");
		}
		if (aiming){ //other directions
			float verticalInput = Input.GetAxisRaw("Vertical");
			if (Input.GetButton("Diagonal")){
				DiagonalAim(1);
				if (verticalInput > 0){
					DiagonalAim(1);
				}else if (verticalInput < 0){
					DiagonalAim(2);
				}
			}else if (verticalInput!=0){
				if (verticalInput>0){
					DiagonalAim(3); //straight up
				}else if (verticalInput<0){
					DiagonalAim(4); //straight down
				}
			}else{
				DiagonalAim(0); //forward
			}
		}
		if (haveGun && !Input.GetButton("Aim") && Input.GetButtonDown("Fire")){
			CancelInvoke("LowerArm");
			nonAimDirectionality = true;
			armUp = true;
			CancelInvoke("Fire");
			InvokeRepeating("Fire", 0, fireRate);
		}
		if (Input.GetButtonUp("Fire")){
			CancelInvoke("Fire");
			nonAimDirectionality = false;
			Invoke("LowerArm", lowerArmWaitTime);
		}
	}
	
	void DiagonalAim(int direction){
		/*
			0=straight ahead
			1=up diagonal
			2=down diagonal
			3=straight up
			4=straight down
		*/
		float leftBarrelOffset = baseLeftOffset;
		int leftDirection = 0;
		if (!controlScript.facingRight){
			
			if (direction == 0){
				leftDirection = 0;
			}else if (direction == 1){
				leftDirection=2;
			}else if (direction == 2){
				leftDirection=1;
			}else if (direction == 3){
				leftDirection=4;
			}else if (direction == 4){
				leftDirection=3;
			}
			gunBarrel.rotation = Quaternion.Euler(barrelRotations[leftDirection]);
		}else{
			leftBarrelOffset = 0;
			gunBarrel.rotation = Quaternion.Euler(barrelRotations[direction]);
		}
		
		Vector2 newBarrelLocation = new Vector2 ((barrelLocations[direction].x * gunArm.localScale.y) - leftBarrelOffset, barrelLocations[direction].y);
		gunBarrel.position = (Vector2)transform.position + newBarrelLocation;
		armSprite.sprite = armAngles[direction];
		
	}
	
	void Fire(){

		Transform newBullet = (Transform)Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity);
		newBullet.rigidbody2D.velocity = gunBarrel.up * bulletSpeed * gunArm.localScale.y;
		//newBullet.rigidbody2D.AddForce(Vector2.up * Random.Range(-1f,1f) * 100f);
	}
	
	void LowerArm(){
		armUp = false;
	}
	
	void Animate(){
		if (aiming ||armUp) {
			armSprite.enabled = true;
		}else{
			armSprite.enabled = false;	
		}if (nonAimDirectionality){
			float verticalInput = Input.GetAxisRaw("Vertical");
			if (Input.GetButton("Diagonal")){
				DiagonalAim(1);
				if (verticalInput > 0){
					DiagonalAim(1);
				}else if (verticalInput < 0){
					DiagonalAim(2);
				}
			}else if (verticalInput!=0){
				if (verticalInput>0){
					DiagonalAim(3); //straight up
				}else if (verticalInput<0){
					DiagonalAim(4); //straight down
				}
			}else{
				DiagonalAim(0); //forward
			}
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
