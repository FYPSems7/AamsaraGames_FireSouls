using UnityEngine;
using System.Collections;

public class NPCScript : MonoBehaviour {


	bool NearbyPlayer = false;
	public GameObject interect;
	public GameObject Player;
	public GameObject InterectSymbol;
	public bool isInterectable = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float distance = 0.0f;
		if (NearbyPlayer) {
			isInterectable = true;
			//Quaternion rotation =new Quaternion (0.0f, 0.0f, 90.0f, 0.0f);
			Vector3 position = new Vector3 (transform.position.x, 3.0f, transform.position.z);
			if (interect == null) {
				interect = Instantiate (InterectSymbol, position, Quaternion.identity) as GameObject;
			}
			Vector3 LookAtPos = new Vector3 (Player.transform.position.x, 0.5f, Player.transform.position.z);
			transform.LookAt (LookAtPos);
			distance = Vector3.Distance (transform.position, Player.transform.position);
		}

		if (distance >= 5.0f) {
			isInterectable = false;
			Destroy (interect);
			interect = null;
		}
		if (distance >= 6.0f) {
			NearbyPlayer = false;
			Player = null;

		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) {
			Player = other.gameObject;
			NearbyPlayer = true;
		}
	}
}
