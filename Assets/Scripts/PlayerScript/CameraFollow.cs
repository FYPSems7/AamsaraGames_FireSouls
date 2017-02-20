using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	private Camera cam;
	private Transform camTransform;
	public Transform target;
	public float distance = 10.0f;
	public float height = 10.0f;
	public float heightDamping = 2.0f;
	public float scrollSpeed;

	public List<GameObject> transparent = new List<GameObject> ();

	void Start()
	{
		camTransform = transform;
		cam = Camera.main;
	}

	void Update()
	{
		scrollSpeed = Input.GetAxis ("Mouse ScrollWheel");
		if (scrollSpeed < 0.0f) {
			distance += 0.5f;
			height += 0.5f;
		} else if (scrollSpeed > 0.0f) {
			distance -= 0.5f;
			height -= 0.5f;
		}

		if(distance > 20.0f){	
			distance = 20.0f;
		}
		else if (distance < 5.0f) {
			distance = 5.0f;
		}
		if(height > 20.0f){	
			height = 20.0f;
		}
		else if (height < 5.0f) {
			height = 5.0f;
		}
		/*RaycastHit hit;
		Vector3 fwd = camTransform.transform.TransformDirection (Vector3.forward);
		Physics.Raycast (camTransform.position, fwd, out hit, 10.0f);
		Debug.DrawRay (camTransform.position, fwd, Color.blue);
		if (hit.transform.tag == "Player") {
			DeTransparent ();
		} else if(hit.transform.tag != "Player"){
			transparent.Add (hit.transform.gameObject);
			Transparent (hit.transform.gameObject);
		}*/
			
	}

	void LateUpdate () 
	{
		float wantedHeight = target.position.y + height;
		float currentHeight = transform.position.y;

		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping + Time.deltaTime);

		transform.position = target.position;
		transform.position -= Vector3.forward * distance;
		transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
		transform.LookAt (target);
	}

	/*void Transparent(GameObject go)
	{
		MeshRenderer renderer = go.GetComponent<MeshRenderer> ();
		Color trans = renderer.material.color;
		trans.a = 0.03f;
		renderer.material.color = trans;
	}
	void DeTransparent()
	{
		for (int i = 0; i < transparent.Count; i++) {
			MeshRenderer renderer = transparent [i].gameObject.GetComponent<MeshRenderer> ();
			Color color = renderer.material.color;
			color.a = 1.0f;
			renderer.material.color = color;
		}
		//transparent.Clear ();
	}*/
}
