using UnityEngine;
using System.Collections;

public class PlayerControlsNoRigidbody : MonoBehaviour {

	public float walkSpeed, runSpeed, jumpSpeed, highJumpSpeed, jumpHorSpeed, maxVerticalSpeed;
	public LayerMask groundMask;
	public LayerMask slopeMask;
	public Transform bodySprite;
	public SpriteRenderer crosshair;
	public float diagRayDist = .75f;
	public float horRayDist = .3f;
	public float vertRayDist = .7f;
	float h, v;
	public bool running, grounded;
	public bool facingRight = false;
	PlayerInventory inventoryScript;
	public float slopeModifier;
	public float baseSlopeModifier;

	void Start () {
		inventoryScript = GetComponent<PlayerInventory>();
	}
	
	void Update () {
	
	}
	
	void FixedUpdate(){
		CheckBelow();
		GetInput();
		Gravity();
		UpdatePosition();
		LimitVerticalSpeed();
		Animate();
	}
	
	void Gravity(){
		if (!grounded){
			transform.Translate(Vector2.up * -2 * Time.deltaTime);
		}
	}
	
	void GetInput(){
		h = Input.GetAxisRaw("Horizontal");
		if (!crosshair.enabled){
			if (h>0){
				facingRight=true;
			} else if (h<0){
				facingRight=false;
			}
		}
		if (Input.GetButton("Run") && grounded){ running = true; }else if (!Input.GetButton("Run") && grounded){ running = false; }
		if (Input.GetButtonDown("Jump")){ CheckToJump(); }
		if (Input.GetButtonUp("Jump")){ CheckToStopJump(); }
	}
	
	void CheckBelow(){
		RaycastHit2D seHit = Physics2D.Raycast (transform.position, new Vector2(diagRayDist,-.75f), 1f, groundMask);
		RaycastHit2D swHit = Physics2D.Raycast (transform.position, new Vector2(-diagRayDist,-.75f), 1f, groundMask);
		
		RaycastHit2D seSlopeHit = Physics2D.Raycast (transform.position, new Vector2(diagRayDist,-.75f), diagRayDist, slopeMask);
		RaycastHit2D swSlopeHit = Physics2D.Raycast (transform.position, new Vector2(-diagRayDist,-.75f), diagRayDist, slopeMask);
		RaycastHit2D belowSlopeHit = Physics2D.Raycast (transform.position,  -Vector2.up*diagRayDist, diagRayDist, slopeMask);
		
		RaycastHit2D eHit = Physics2D.Raycast (transform.position, new Vector2(horRayDist,0f), horRayDist, groundMask);
		RaycastHit2D wHit = Physics2D.Raycast (transform.position, new Vector2(-horRayDist,0), horRayDist, groundMask);
		RaycastHit2D belowHit = Physics2D.Raycast (transform.position, -Vector2.up*vertRayDist, vertRayDist, groundMask);
		
		if (seHit.transform!=null){	Debug.DrawRay(transform.position, new Vector2(diagRayDist,-.75f), Color.blue);}
		if (swHit.transform!=null){	Debug.DrawRay(transform.position, new Vector2(-diagRayDist,-.75f), Color.blue);	}
		if (seSlopeHit.transform!=null){	Debug.DrawRay(transform.position, new Vector2(diagRayDist,-.75f), Color.red);		print ("normal is " + seHit.normal);}
		if (swSlopeHit.transform!=null){	Debug.DrawRay(transform.position, new Vector2(-diagRayDist,-.75f), Color.red);	}
		if (belowSlopeHit.transform!=null){	Debug.DrawRay(transform.position, -Vector2.up*vertRayDist, Color.red	);	}
		if (eHit.transform!=null){	Debug.DrawRay(transform.position, new Vector2(horRayDist,0), Color.blue);	}
		if (wHit.transform!=null){	Debug.DrawRay(transform.position, new Vector2(-horRayDist,0), Color.blue);	}
		if (belowHit.transform!=null){	Debug.DrawRay(transform.position, -Vector2.up*vertRayDist, Color.blue);	}
		
		
		if ((belowHit.transform!=null) || ((seHit.transform!=null || swHit.transform!=null) && (eHit.transform==null && wHit.transform==null ))){
			grounded = true;}
		else{ 
			grounded = false;
		}	
		CheckForSlope (seSlopeHit, swSlopeHit, belowSlopeHit);
	}
	
	void CheckForSlope(RaycastHit2D seSlopeHit, RaycastHit2D swSlopeHit, RaycastHit2D belowSlopeHit){
		if (seSlopeHit.transform!=null && belowSlopeHit.transform!=null && h==0){ //detecting slope to right
			//rigidbody2D.velocity = new Vector2(0, 0);
		}else if (swSlopeHit.transform!=null && belowSlopeHit.transform!=null && h == 0){ //slope to left
			//rigidbody2D.velocity = new Vector2(0, 0);
		}else if (seSlopeHit.transform!=null && belowSlopeHit.transform!=null && h!=0){
			slopeModifier = baseSlopeModifier;
		}else if (swSlopeHit.transform!=null && belowSlopeHit.transform!=null && h != 0){
			slopeModifier = baseSlopeModifier;
		}
		else{
			slopeModifier = 0f;
		}
		
	}
	
	void UpdatePosition(){
		//if (grounded){	
			slopeModifier *= h;
			if (!running){
				transform.Translate(Vector2.right * h * walkSpeed * Time.deltaTime);
			}else{
				//rigidbody2D.velocity = new Vector2(h * runSpeed, rigidbody2D.velocity.y + slopeModifier);
			}
	//	}
	}
	
	void CheckToJump(){
	print ("pressed jump");
		if (grounded){
			Jump();
		}
	}
	
	void CheckToStopJump(){

		//if (rigidbody2D.velocity.y > .01){
			//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, .01f);
		//	rigidbody2D.AddForce(Vector2.right * jumpHorSpeed * h);
		//}
	}
	
	void Jump(){
		if (inventoryScript.highJump){
		//	rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, highJumpSpeed);
		}else{
		//	rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeed);
		}
	}
	
	void LimitVerticalSpeed(){
		//if (Mathf.Abs(rigidbody2D.velocity.y) > maxVerticalSpeed){
			float velToSet = 0;
			//if (rigidbody2D.velocity.y > 0){ velToSet = maxVerticalSpeed;}
			//else if (rigidbody2D.velocity.y<0){ velToSet = -maxVerticalSpeed;}
		//	rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, velToSet);
		//}
	}
	
	void Animate(){
		if (!crosshair.enabled){
			if (facingRight){
				bodySprite.transform.localScale = new Vector3 (1,1,1);
			}else if (!facingRight){
				bodySprite.transform.localScale = new Vector3 (-1,1,1);
			}
		}else{
			if (crosshair.transform.position.x > transform.position.x){
				bodySprite.transform.localScale = new Vector3 (1,1,1);
				
				facingRight = true;
			}else{
				bodySprite.transform.localScale = new Vector3 (-1,1,1);
				facingRight = false;
			}
		}
	}
}
