using UnityEngine;
using System.Collections;

public class snapToPixelGrid : MonoBehaviour {

	public bool isBullet;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float adjustedX;
		float adjustedY;
		if (!isBullet){
			adjustedX = Round(transform.position.x);
			adjustedY = Round(transform.position.y);
		}else{
			adjustedX = transform.parent.position.x;
			adjustedY = transform.parent.position.y;
		}
		SetNewPos(adjustedX, adjustedY);
	}
	
	float Round(float rawPos){
		float adjusted = Mathf.Round(rawPos * 10f)/ 10f;
		return adjusted;
	}
	
	void SetNewPos(float x, float y){
		transform.position = new Vector2(x,y);
	}
}
