using UnityEngine;
using System.Collections;

public class Enemy_Charge : MonoBehaviour 
{

	public GameObject Player;
	public float Velocity = 0;

	// How many units per second it should move at Velocity 1
	public float speedFactor = 1f;

	public Vector3 movement;
	bool isCharging = false;
	bool ischargeLeft = false;
	bool ischargeRight = false;
	void Update ()
	{
		Charge();
	}
	public void Charge()
	{
		if(isCharging == false)
		{
			if(Velocity >= 1)
			{
				Velocity -= 1;
			}
			else
			{
				Velocity = 0;
			}
			Invoke("CalculateDirection",2);
		}
		else
		{
			if (ischargeLeft)
			{
				if (Player.transform.position.x > this.transform.position.x || Player.transform.position.x == this.transform.position.x)
				{
					isCharging = false;
				}
				else
				{
					Velocity += 1;
					movement.x = -0.5f * Velocity*Time.deltaTime*speedFactor;
					movement.y = this.transform.position.y;
					movement.z = this.transform.position.z;
					this.transform.position = movement;
				}
			}
			else if (ischargeRight)
			{
				if (Player.transform.position.x < this.transform.position.x || Player.transform.position.x == this.transform.position.x)
				{
					isCharging = false;
				}
				else
				{
					Velocity += 1;
					movement.x = 0.5f * Velocity*Time.deltaTime*speedFactor;
					movement.y = this.transform.position.y;
					movement.z = this.transform.position.z;
					this.transform.position = movement;
				}
			}
		}
	}
	public void CalculateDirection()
	{
		if (Player.transform.position.x < this.transform.position.x)
		{
			ischargeLeft = true;
			ischargeRight = false;
			isCharging = true;
		}
		else if (Player.transform.position.x > this.transform.position.x)
		{
			ischargeRight = false;
			ischargeLeft = true;
			isCharging = true;
		}
	}
}