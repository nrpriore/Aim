﻿using UnityEngine;						// To inherit from Monobehaviour

/// Handles cross-platform slingshot input during game
/// Supported: editor, mobile
public class InputController : MonoBehaviour {

	// Constant vars
	private GameController _gc;			// Reference to GameController script
	private Transform _ballTR;			// Reference to the transform of the ball
	private Transform _arrowTR;			// Reference to transform of aiming arrow
	private SpriteRenderer _arrowSR;	// Reference to SpriteRenderer of aiming arrow
	private BallController _ball;		// Reference to current BallController script
	private GameObject _ballBoundary;	// Reference to clickable boundary for ball

	private float _offset;				// Distance from center of ball to tip of arrow (for UI_)
	private float _minPower;			// Min scale of arrow (determines power)
	private float _maxPower;			// Max scale of arrow (determines power)
	private float _minAim;				// Min magintude for a valid aim (dragged far enough from the ball?)

	// Dynamic vars
	private bool _aiming;				// This returns true when mouse or touch is down
	private Projection _projection;		// The Projection instance for this aim
	private Touch _aimTouch;			// The touch used as an input for aiming
	private int _aimTouchID;			// The fingerID associated with _aimTouch
	private int _prevTouchCount;		// TouchCount of previous frame


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

	// Editor
		if(Application.isEditor) {
			// If mouseclick hits ball boundary start aiming
			if(Input.GetMouseButtonDown(0)) {
				Vector2 mPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Collider2D hit = Physics2D.OverlapPoint(mPos);

				if(hit) {
					if(hit.transform.gameObject == _ballBoundary) {
						StartAiming();
					}
				}
			}

			// If mouse up while arrow is active (meaning a valid aim), shoot the ball. Otherwise, just disable arrow
			if(Input.GetMouseButtonUp(0) && _aiming) {
				if(_arrowSR.enabled) {
					_gc.Shoot();
					_ball.Shoot(_arrowTR.localScale.x,_arrowTR.localEulerAngles.z);
				}
				DisableArrow();
				StopAiming();
				return;
			}

			if(_aiming) {
				// mPos here is the position of the mouse RELATIVE to the ball
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

					// Update projection
					_projection.Update(
						_ball.gameObject.GetComponent<Rigidbody2D>(), 
						(Vector2)_ball.gameObject.transform.localPosition, 
						Functions.GetVelocity(_arrowTR.localScale.x, _arrowTR.localEulerAngles.z)
					);

				// If too close to ball, disable arrow
				}else if(_arrowSR.enabled) {
					DisableArrow();
				}
			}
		}



	// Phone
		if(Application.isMobilePlatform) {
			// Check for new touch. If touch hits ball boundary and not already aiming, assign and start
			if(Input.touchCount != _prevTouchCount) {
				if(Input.touchCount > _prevTouchCount && !_aiming) {
					Touch newTouch = Input.GetTouch(Input.touchCount - 1);
					Vector2 tPos = (Vector2)Camera.main.ScreenToWorldPoint(newTouch.position);
					Collider2D hit = Physics2D.OverlapPoint(tPos);

					if(hit) {
						if(hit.transform.gameObject == _ballBoundary) {
							_aimTouchID = newTouch.fingerId;
							StartAiming();
						}
					}
				}
				_prevTouchCount = Input.touchCount;
			}

			// Since Touch is a struct (stupid Unity), assign _aimTouch each frame... (stupid Unity)...
			foreach(Touch touch in Input.touches) {
				if(touch.fingerId == _aimTouchID) {
					_aimTouch = touch;
				}
			}

			// If touch ends while arrow is active (meaning a valid aim), shoot the ball. Otherwise, just disable arrow
			if(_aimTouch.phase == TouchPhase.Ended) {
				if(_arrowSR.enabled) {
					_gc.Shoot();
					_ball.Shoot(_arrowTR.localScale.x,_arrowTR.localEulerAngles.z);
				}
				DisableArrow();
				StopAiming();
				return;
			}

			if(_aiming) {
				// tPos here is the position of the touch RELATIVE to the ball
				Vector2 tPos = (Vector2)Camera.main.ScreenToWorldPoint(_aimTouch.position) - (Vector2)_ballTR.localPosition;
				// If a valid aim, enable and update arrow transform
				if(tPos.magnitude >= _minAim) {
					if(!_arrowSR.enabled) {
						_arrowSR.enabled = true;
					}
					float zRot = (tPos.x >= 0)? Mathf.Atan(tPos.y/tPos.x) + Mathf.PI : Mathf.Atan(tPos.y/tPos.x);

					_arrowTR.localRotation  = Quaternion.Euler(0, 0, 180 * zRot / Mathf.PI);
					_arrowTR.localPosition  = new Vector2(Mathf.Cos(zRot),Mathf.Sin(zRot)) * -_offset;
					_arrowTR.localScale  	= Vector2.one * Mathf.Clamp(tPos.magnitude, _minPower, _maxPower);

				// If too close to ball, disable arrow
				}else if(_arrowSR.enabled) {
					DisableArrow();
				}
			}
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------



/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_gc 			= GameObject.Find("GameController").GetComponent<GameController>();
		_ballTR			= GameObject.Find("Ball").transform;
		_ball 			= _ballTR.gameObject.GetComponent<BallController>();
		_ballBoundary 	= _ball.gameObject.transform.Find("ClickBoundary").gameObject;
		_arrowTR 		= GameObject.Find("Arrow").transform;
		_arrowSR 		= _arrowTR.gameObject.GetComponent<SpriteRenderer>();

		_offset 	= 0.75f;
		_minPower 	= 2.5f;
		_maxPower 	= 5f;
		_minAim	 	= 2f;

		_prevTouchCount = 0;
	}

	// Runs when arrow is disabled
	private void DisableArrow() {
		if(_projection != null) {
			_projection.HideProjection();
		}
		_arrowSR.enabled = false;
	}

	// Runs when aiming started
	private void StartAiming() {
		_aiming = true;
		_projection = new Projection();
	}

	// Runs when aiming stopped
	private void StopAiming() {
		_aiming = false;
		if(_projection != null) {
			_projection.Destroy();
		}
		_projection = null;
	}
	
}
