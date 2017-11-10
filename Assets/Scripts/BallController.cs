using UnityEngine;						// To inherit from Monobehaviour

/// Handles ball mechanics in game
public class BallController : MonoBehaviour {

	// Constant vars
	private GameController _gc;			// Reference to GameController script
	private Rigidbody2D _rb;			// Reference to rigidbody2d of ball
	private int _successLayer;			// Reference to the layer mask holding Success collider ("Success")
	private int _objLayer;				// Reference to the layer mask holding objects for collision ("Objects")
	private float _ballRadius;			// Reference to radius of ball for faster collision detection
	private float _velThreshold;		// Value at which ball is considered at rest

	// Dynamic vars


	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		// Success if ball is sleeping while colliding with success object
		if(_rb.IsSleeping()) {
			if(IsSuccess()) {
				_gc.Success();
			}else {
				if(GameObject.Find("GameController").GetComponent<GameController>().Shot) {
					_gc.Fail();
				}
			}
		}else if(OnGround()) {
			// Speed improvement to stop ball if magnitude of speed is below threshold
			if(_rb.velocity.magnitude < _velThreshold) {
				_rb.velocity = Vector2.zero;
			}
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Shoots ball with given power at given angle
	public void Shoot(float power, float angle) {
		_rb.velocity = Functions.GetVelocity(power, angle);
		_rb.angularVelocity = (Mathf.Abs(angle) >= 90f)? 300f : -300f;
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_gc = GameObject.Find("GameController").GetComponent<GameController>();
		_rb = gameObject.GetComponent<Rigidbody2D>();
		_successLayer = LayerMask.GetMask("Success");
		_objLayer = LayerMask.GetMask("Objects");
		_ballRadius = gameObject.GetComponent<CircleCollider2D>().radius;

		_velThreshold = 0.2f;
	}

	// Checks if ball is colliding with Success layer
	private bool IsSuccess() {
		return Physics2D.OverlapCircle((Vector2)gameObject.transform.localPosition, _ballRadius, _successLayer);
	}

	// Checks if ball is colliding with boundary (ground in all cases for now)
	private bool OnGround() {
		Collider2D hit = Physics2D.OverlapCircle((Vector2)gameObject.transform.localPosition, _ballRadius, _objLayer);
		if(hit) {
			return hit.tag == "Boundary";
		}
		return false;
	}

}
