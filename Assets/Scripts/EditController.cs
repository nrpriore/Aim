using UnityEngine;						// To inherit from Monobehaviour

 
public class EditController : MonoBehaviour {

	// Constant vars
	private int _objLayer;				// Reference to the layer mask holding objects for collision ("Objects")

	// Dynamic vars
	private GameObject _currObject;		// Object currently being moved
	private int _prevTouchCount;		// TouchCount of previous frame
	private Touch _aimTouch;			// The touch used as an input for aiming
	private int _aimTouchID;			// The fingerID associated with _aimTouch

	private Touch activeTouch;


	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {

		Vector2 inputPosition = Vector2.zero;

	// Editor
		if(Application.isEditor) {
			if(Input.GetMouseButtonDown(0)) {
				Vector2 mPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Collider2D hit = Physics2D.OverlapPoint(mPos, _objLayer);
				Debug.Log(mPos);
				Debug.Log(hit);

				if(hit && _currObject == null) {
					Debug.Log(hit.gameObject.name);
					_currObject = hit.gameObject;
				}
			}

			if(Input.GetMouseButtonUp(0) && _currObject != null) {
				_currObject = null;
			}

			if(_currObject != null) {
				inputPosition = Input.mousePosition;
			}

		}
	// Mobile
		else if(Application.isMobilePlatform) {
			if(Input.touchCount != _prevTouchCount) {
				if(Input.touchCount > _prevTouchCount) {
					Touch newTouch = Input.GetTouch(Input.touchCount - 1);
					Vector2 tPos = (Vector2)Camera.main.ScreenToWorldPoint(newTouch.position);
					Collider2D hit = Physics2D.OverlapPoint(tPos, _objLayer);

					if(hit && _currObject == null) {
						_currObject = hit.gameObject;
						_aimTouchID = newTouch.fingerId;
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

			if(_aimTouch.phase == TouchPhase.Ended) {
				_currObject = null;
			}

			if(_currObject != null) {
				inputPosition = _aimTouch.position;
			}
		}

		// Update position
		if(_currObject != null) {
			_currObject.transform.localPosition = inputPosition;
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_objLayer = LayerMask.GetMask("EditorObject");

		_prevTouchCount = 0;
		_currObject = null;
	}
	
}
