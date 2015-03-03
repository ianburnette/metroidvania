using UnityEngine;
using System.Collections;

public class playerControls : MonoBehaviour {

	public bool canGrab;
	bool hanging;
	public float walkSpeed, runSpeed, jumpSpeed, highJumpSpeed, jumpHorSpeed, maxVerticalSpeed, grabStunTime;
	public LayerMask groundMask;
	public LayerMask slopeMask;
	public Transform bodySprite;
	public float diagRayDist = .75f;
	public float horRayDist = .3f;
	public float vertRayDist = .7f;
	float h, v;
	public bool running, grounded, onSlope;
	public bool facingRight = false;
	PlayerInventory inventoryScript;
	GunControlsNonAiming gunControls;
	public float slopeModifier;
	public float baseSlopeModifier;
	public float hugGroundHeight;
	Animator anim;
	public float grabOffsetH, grabOffsetL, grabDistance;
	public float highGrav, baseGrav;

	void Start () {
		inventoryScript = GetComponent<PlayerInventory>();
		gunControls = GetComponent<GunControlsNonAiming>();
		anim = GetComponent<Animator>();
	}

	void FixedUpdate(){
		CheckBelow();
		if (canGrab){
			CheckToGrab();
		}if (!hanging){
			GetInput();
		}
		UpdatePosition();
		MoreGravity();
		LimitVerticalSpeed();
		Animate();
	}
	
	void MoreGravity(){
		if (grounded && !onSlope){
			GetComponent<Rigidbody2D>().gravityScale = highGrav;
		}else if (!onSlope){
			GetComponent<Rigidbody2D>().gravityScale = baseGrav;
		}
	}
	
	void CheckToGrab(){
		int facingInt = 1;
		if (!facingRight){facingInt=-1;}
		Debug.DrawRay(new Vector2(transform.position.x, transform.position.y+grabOffsetH), Vector2.right * facingInt * grabDistance, Color.red);
		Debug.DrawRay(new Vector2(transform.position.x, transform.position.y+grabOffsetL), Vector2.right * facingInt * grabDistance, Color.yellow);
		if (!grounded && !onSlope){
			RaycastHit2D upperGrabHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y+grabOffsetH), Vector2.right * facingInt * grabDistance, grabDistance, groundMask);
			RaycastHit2D lowerGrabHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y+grabOffsetL), Vector2.right * facingInt * grabDistance, grabDistance, groundMask);
			if (upperGrabHit.transform == null && lowerGrabHit.transform!=null){
				hanging = true;
			}
		}
		if (hanging){
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			HangControls();
		}
	}
	
	void HangControls(){
		if (Input.GetAxisRaw("Vertical")<0){
			hanging=false;
			canGrab=false;
			Invoke("ResumeGrab", grabStunTime);
		}
		if (Input.GetButtonDown("Jump")){
			hanging=false;
			canGrab=false;
			Invoke("ResumeGrab", grabStunTime);
			Jump();
		}
	}
	
	void ResumeGrab(){
		canGrab = true;
	}
	
	void GetInput(){
		h = Input.GetAxisRaw("Horizontal");
		if (!gunControls.aiming){
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
		
		
		if ((belowHit.transform!=null) /* || ((seHit.transform!=null || swHit.transform!=null) && (eHit.transform==null && wHit.transform==null */ ){
			grounded = true;
			//transform.position = new Vector2(transform.position.x, belowHit.point.y+hugGroundHeight);
		}else{ 
			grounded = false;
		}
		if (seSlopeHit.transform!=null || swSlopeHit.transform!=null){
			grounded = true;
			RaycastHit2D belowSlopeHitHug = Physics2D.Raycast (transform.position, -Vector2.up*vertRayDist*2, vertRayDist*2, groundMask);
			transform.position = new Vector2 (transform.position.x, belowSlopeHitHug.point.y + hugGroundHeight);
		}	
		CheckForSlope (seSlopeHit, swSlopeHit, belowSlopeHit);
		
		
	}
	
	void CheckForSlope(RaycastHit2D seSlopeHit, RaycastHit2D swSlopeHit, RaycastHit2D belowSlopeHit){
		if (seSlopeHit.transform!=null && belowSlopeHit.transform!=null && h==0){ //detecting slope to right
			GetComponent<Rigidbody2D>().gravityScale = 0;
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			onSlope = true;
		}else if (swSlopeHit.transform!=null && belowSlopeHit.transform!=null && h == 0){ //slope to left
			GetComponent<Rigidbody2D>().gravityScale = 0;
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			onSlope = true;
		}else if (seSlopeHit.transform!=null && belowSlopeHit.transform!=null && h!=0){
			slopeModifier = -baseSlopeModifier;
			onSlope = true;
		}else if (swSlopeHit.transform!=null && belowSlopeHit.transform!=null && h != 0){
			slopeModifier = baseSlopeModifier;
			onSlope = true;
		}
		else{
			slopeModifier = 0f;
			onSlope = false;
		}
		
	}
	
	void UpdatePosition(){
		//if (grounded){	
			slopeModifier *= h;
			if (!running){
				GetComponent<Rigidbody2D>().velocity = new Vector2(h * walkSpeed, GetComponent<Rigidbody2D>().velocity.y + slopeModifier);
			}else{
				GetComponent<Rigidbody2D>().velocity = new Vector2(h * runSpeed, GetComponent<Rigidbody2D>().velocity.y + slopeModifier);
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

		if (GetComponent<Rigidbody2D>().velocity.y > .01){
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, .01f);
		//	rigidbody2D.AddForce(Vector2.right * jumpHorSpeed * h);
		}
	}
	
	void Jump(){
		if (inventoryScript.highJump){
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, highJumpSpeed);
		}else{
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpSpeed);
		}
	}
	
	void LimitVerticalSpeed(){
		if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxVerticalSpeed){
			float velToSet = 0;
			if (GetComponent<Rigidbody2D>().velocity.y > 0){ velToSet = maxVerticalSpeed;}
			else if (GetComponent<Rigidbody2D>().velocity.y<0){ velToSet = -maxVerticalSpeed;}
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x, velToSet);
		}
	}
	
	void Animate(){
		
		
		if (!gunControls.aiming){
			if (facingRight){
				bodySprite.transform.localScale = new Vector3 (1,1,1);
			}else if (!facingRight){
				bodySprite.transform.localScale = new Vector3 (-1,1,1);
			}
		}else{
			/* if (crosshair.transform.position.x > transform.position.x){
				bodySprite.transform.localScale = new Vector3 (1,1,1);
				
				facingRight = true;
			}else{
				bodySprite.transform.localScale = new Vector3 (-1,1,1);
				facingRight = false;
			} */
		}
		
		
		if (h==0){
			anim.SetBool("moving", false);
		}else if (running){
			anim.SetBool("running", true);
			anim.SetBool("moving", true);
		}else if (!running){
			anim.SetBool("running", false);
			anim.SetBool("moving", true);
		}
		if (!grounded){
			if (GetComponent<Rigidbody2D>().velocity.y>=0){
				anim.SetBool("jumping", true);
				anim.SetBool("falling", false);
			}else if (GetComponent<Rigidbody2D>().velocity.y<0){
				anim.SetBool("jumping", false);
				anim.SetBool("falling", true);
			}
		}else{
			anim.SetBool("jumping", false);
			anim.SetBool("falling", false);
		}
		if (hanging){
			anim.SetBool("hanging", true);
		}else{
			anim.SetBool("hanging", false);
		}
	}
}
