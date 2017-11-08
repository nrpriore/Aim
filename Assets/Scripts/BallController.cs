﻿using UnityEngine;						// To inherit from Monobehaviour

 
public class BallController : MonoBehaviour {

	// Constant vars
	private Rigidbody2D _rb;			// Reference to rigidbody2d of ball

	// Dynamic vars



	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Shoots ball with given power at given angle
	public void Shoot(float power, float angle) {
		float angleRad = Mathf.PI * angle / 180f;

		_rb.simulated = true;
		_rb.AddForce(new Vector2(Mathf.Cos(angleRad),Mathf.Sin(angleRad)) * power * 200);
		_rb.angularVelocity = (Mathf.Abs(angle) >= 90f)? 30f : -30f;
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
}