using UnityEngine;						// To inherit from Monobehaviour

 
public class InputController : MonoBehaviour {

	// Constant vars
	private GameController _gc;			// Reference to GameController script
	private Transform _ballTR;			// Reference to the transform of the ball
	private Transform _arrowTR;			// Reference to transform of aiming arrow
	private SpriteRenderer _arrowSR;	// Reference to SpriteRenderer of aiming arrow
	private BallController _ball;		// Reference to current BallController script

	private float _offset;				// Distance from center of ball to tip of arrow (for UI_)
	private float _minPower;			// Min scale of arrow (determines power)
	private float _maxPower;			// Max scale of arrow (determines power)
	private float _minAim;				// Min magintude for a valid aim (dragged far enough from the ball?)

	// Dynamic vars
	private bool _aiming;				// This returns true when mouse or touch is down


	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		// Stop input if ball is already shot
		if(_gc.Shot) {
			return;
		}

	// Computer
		_aiming = (!_aiming)? Input.GetMouseButtonDown(0) : !Input.GetMouseButtonUp(0);

		// If the screen is let go while the arrow is active (meaning a valid aim), shoot the ball
		if(Input.GetMouseButtonUp(0) && _arrowSR.enabled) {
			_arrowSR.enabled = false;
			_gc.Shoot();
			_ball.Shoot(_arrowTR.localScale.x,_arrowTR.localEulerAngles.z);
			return;
		}

		if(_aiming) {
			// mPos is the position of the mouse RELATIVE to the ball
			Vector2 mPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)_ballTR.localPosition;
			// If a valid aim, enable and update arrow transform
			if(mPos.magnitude >= _minAim) {
				if(!_arrowSR.enabled) {
					_arrowSR.enabled = true;
				}
				float zRot = (mPos.x >= 0)? Mathf.Atan(mPos.y/mPos.x) + Mathf.PI : Mathf.Atan(mPos.y/mPos.x);

				_arrowTR.localRotation  = Quaternion.Euler(0, 0, 180 * zRot / Mathf.PI);
				_arrowTR.localPosition  = new Vector2(Mathf.Cos(zRot),Mathf.Sin(zRot)) * -_offset;
				_arrowTR.localScale  	= Vector2.one * Mathf.Clamp(mPos.magnitude, _minPower, _maxPower);

			// If too close to ball, disable arrow
			}else if(_arrowSR.enabled) {
				_arrowSR.enabled = false;
			}
		// If not aiming, disable arrow
		}else if(_arrowSR.enabled) {
			_arrowSR.enabled = false;
		}



	// Phone
		foreach(Touch touch in Input.touches) {
			if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
				Debug.Log(touch.position.x + ", " + touch.position.y);
			}
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------



/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_gc 		= GameObject.Find("GameController").GetComponent<GameController>();
		_ballTR		= GameObject.Find("Ball").transform;
		_ball 		= _ballTR.gameObject.GetComponent<BallController>();
		_arrowTR 	= GameObject.Find("Arrow").transform;
		_arrowSR 	= _arrowTR.gameObject.GetComponent<SpriteRenderer>();

		_offset 	= 0.65f;
		_minPower 	= 1f;
		_maxPower 	= 4f;
		_minAim	 	= 0.6f;
	}
	
}
