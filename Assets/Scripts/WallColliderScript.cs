using UnityEngine;
using System.Collections;

public class WallColliderScript : MonoBehaviour {

	public XboxPlayerControlScript xboxControl;

	public bool ishitwall = false;
	public bool isColliding = true;
	public float distance = 1.5f;
	Ray ray;
	Vector3 fwd;
	RaycastHit hit;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0.0f));
		fwd = GetComponent<Rigidbody>().transform.forward;
		Vector3 pos = transform.position;
		pos.y += 1.0f;
		//pos.z += 1.0f;
		Debug.DrawRay (pos, fwd * distance, Color.green);
		if (Physics.Raycast (pos, fwd, out hit, distance)) {
			if (hit.transform.gameObject.CompareTag ("Wall")) {
				Debug.Log ("isHit");
				ishitwall = true;
			} 
		} else
			ishitwall = false;
	}

	/*void OnCollisionEnter(Collision collision)
	{
		//Debug.Log (collision);
		if (collision.transform.tag == "Wall") {
			ishitwall = true;
		}
	}*/

	/*void OnCollisionExit(Collision collision)'
	{
		ishitwall = true;
	}*/
}
