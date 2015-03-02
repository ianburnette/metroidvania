using UnityEngine;
using System.Collections;

public class MoveCursor : MonoBehaviour {

	public float moveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis("HorizontalRS");
		float y = Input.GetAxis("VerticalRS");
		transform.Translate(new Vector3(x*moveSpeed,-y*moveSpeed,0)*Time.deltaTime);
	}
}
