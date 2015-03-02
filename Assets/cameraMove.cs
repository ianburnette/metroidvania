using UnityEngine;
using System.Collections;

public class cameraMove : MonoBehaviour {

	public float camXMoveDist, camYMoveDist;
	public float maxXDist, maxYDist;
	float curXDist, curYDist;
	public Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		curXDist = player.position.x - transform.position.x;
		if (Mathf.Abs(curXDist) >= maxXDist){
			if (curXDist>0){ MoveCamSideways(1);}
			else if (curXDist<0){ MoveCamSideways(-1);}
		}
		curYDist = player.position.y - transform.position.y;
		if (Mathf.Abs(curYDist) >= maxYDist){
			if (curYDist>0){ MoveCamVertically(1);}
			else if (curYDist<0){ MoveCamVertically(-1);}
		}
	}
	
	void MoveCamSideways(int dir){
		transform.position = new Vector3(transform.position.x+(camXMoveDist * dir), transform.position.y, -10);
	}
	
	void MoveCamVertically(int dir){
		transform.position = new Vector3(transform.position.x, transform.position.y+(camYMoveDist * dir), -10);
	}
}
