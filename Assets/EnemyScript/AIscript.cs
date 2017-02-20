using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class AIscript : MonoBehaviour {

	public NavMeshAgent agent;
	//public EnemyScript enemy;
	//public EnemyShootScript enemyShootScipt;
 
	public enum State
	{
		PATROL,
		CHASE,
		SCOUT
	}

	public State state;
	private bool alive;

	//Patrolling
	public GameObject[] waypoints;
	private int waypointInd;
	public float patrolSpeed = 0.5f;

	//Chasing
	public float stopDistance = 4.0f;
	public float chaseSpeed = 1f;
	public GameObject target;

	//Scout
	private Vector3 scoutSpot;
	private float timer = 0;
	public float scoutWait = 2;

	//Sight
	public float heightMultiplier;
	public float sightDistance = 10;

	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		//enemy = GetComponent<EnemyScript> ();
		
		agent.updatePosition = true;
		agent.updateRotation = false;

		waypoints = GameObject.FindGameObjectsWithTag ("Waypoint");
		waypointInd = Random.Range (0, waypoints.Length);

		state = AIscript.State.PATROL;

		alive = true;

		heightMultiplier = 1.36f;

		StartCoroutine ("FSM");
	}

	IEnumerator FSM()
	{
		while (alive)
		{
			switch (state) 
			{
				case State.PATROL:
					Patrol();
					break;
				case State.CHASE:
					Chase();
					break;

				case State.SCOUT:
					Scout ();
					break;
			}
			yield return null;
		}
			
	}

	void Patrol()
	{
		agent.speed = patrolSpeed;
		if (Vector3.Distance (this.transform.position, waypoints [waypointInd].transform.position) >= 2)
		{
			agent.SetDestination (waypoints [waypointInd].transform.position);
		} 

		else if (Vector3.Distance (this.transform.position, waypoints [waypointInd].transform.position) <= 2) 
		{
			waypointInd = Random.Range (0, waypoints.Length);
		}
			
	}

	void Chase()
	{
		float distance = Vector3.Distance (target.transform.position, transform.position);
		if (distance < stopDistance) {
			agent.speed = 0.0f;
			//enemyShootScipt.GetComponent<EnemyShootScript> ().Shoot = true;
		} 
		else 
		{
			//enemyShootScipt.GetComponent<EnemyShootScript> ().Shoot = false;
			agent.speed = chaseSpeed;
			Vector3 targPos = new Vector3 (target.transform.position.x, 1.0f, target.transform.position.z);
			transform.LookAt (targPos);
		}
		agent.SetDestination (target.transform.position);
	}
		


	void Scout()
	{
		timer = +Time.deltaTime;

		agent.SetDestination (this.transform.position);
		transform.LookAt (scoutSpot);
		if (timer >= scoutWait)
		{
			timer = 0.0f;
			state = AIscript.State.PATROL;

		}
	}

	void OnTriggerEnter (Collider coll)
	{
		if (coll.tag == "Player") {
			state = AIscript.State.SCOUT;
			scoutSpot = coll.gameObject.transform.position;
		} 


	}


	


	// Update is called once per frame
	void FixedUpdate () 
	{
		RaycastHit hit;
		Debug.DrawRay (transform.position + Vector3.up * heightMultiplier, transform.forward * sightDistance, Color.red);
		Debug.DrawRay (transform.position + Vector3.up * heightMultiplier, transform.forward + transform.right * sightDistance, Color.red);
		Debug.DrawRay (transform.position + Vector3.up * heightMultiplier, transform.forward - transform.right * sightDistance, Color.red);

		if(Physics.Raycast (transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, sightDistance))
		{
			if(hit.collider.gameObject.tag == "Player")
			{
				state = AIscript.State.CHASE;
				target = hit.collider.gameObject;
			}
		}

		if(Physics.Raycast (transform.position + Vector3.up * heightMultiplier, transform.forward + transform.right, out hit, sightDistance))
		{
			if(hit.collider.gameObject.tag == "Player")
			{
				state = AIscript.State.CHASE;
				target = hit.collider.gameObject;
			}
		}

		if(Physics.Raycast (transform.position + Vector3.up * heightMultiplier, transform.forward - transform.right, out hit, sightDistance))
		{
			if(hit.collider.gameObject.tag == "Player")
			{
				state = AIscript.State.CHASE;
				target = hit.collider.gameObject;
			}
		}
	}
}
