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
	private Animator _anim;				// Reference to sball animator
	private float _fixedUpdateTime;		// Reference to fixedupdate interval
	private float _decrementInterval;	// Amount to reduce decrement

	// Dynamic vars


	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void FixedUpdate() {
		if(_gc.Shot) {
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
			}else {
				// Modify collision to reduce overlap (check where ball will be next frame)
				Vector2 pos = DecrementPos(1);
				Collider2D hit = Physics2D.OverlapCircle(pos, _ballRadius, _objLayer);
				if(hit) {
					float decrement = 1f;
					while(hit && decrement > 0) {
						decrement -= _decrementInterval;
						pos = DecrementPos(decrement);
						hit = Physics2D.OverlapCircle(pos, _ballRadius, _objLayer);
					}
					decrement += _decrementInterval;
					pos = DecrementPos(decrement);
					gameObject.transform.localPosition = pos;
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D hit) {
		switch(hit.collider.sharedMaterial.name) {
			case "Spikes":
				gameObject.GetComponent<CircleCollider2D>().enabled = false;
				_anim.Play("Spikes");
				break;
			default: return;
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Shoots ball with given power at given angle
	public void Shoot(float power, float angle) {
		_rb.velocity = Functions.GetVelocity(power, angle);
		//_rb.angularVelocity = (Mathf.Abs(angle) >= 90f)? 300f : -300f;
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
		_anim = gameObject.GetComponent<Animator>();
		_fixedUpdateTime = Time.fixedDeltaTime;

		_decrementInterval = 0.001f;
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

	// Gets next decrement position
	private Vector2 DecrementPos(float decrement) {
		return (Vector2)gameObject.transform.localPosition + (_rb.velocity * _fixedUpdateTime * decrement);
	}

}
