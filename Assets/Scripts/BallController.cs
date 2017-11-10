using UnityEngine;						// To inherit from Monobehaviour

/// Handles ball mechanics in game
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
		_rb.velocity = Functions.GetVelocity(power, angle);
		_rb.angularVelocity = (Mathf.Abs(angle) >= 90f)? 30f : -30f;
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
}
