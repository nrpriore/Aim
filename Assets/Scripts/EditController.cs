using UnityEngine;						// To inherit from Monobehaviour

 
public class EditController : MonoBehaviour {

	// Constant vars
	private int _objLayer;				// Reference to the layer mask holding objects for collision ("Objects")

	// Dynamic vars
	private GameObject _currObject;		// Object currently being moved
	private int _prevTouchCount;		// TouchCount of previous frame
	private Touch _aimTouch;			// The touch used as an input for aiming
	private int _aimTouchID;			// The fingerID associated with _aimTouch
	private Vector2 _lastPos;			// The position of touch/click last frame
	private Vector2 _deltaPos;			// Difference between _lastPos and current position

	private Touch activeTouch;



	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void FixedUpdate() {

	// Editor
		if(Application.isEditor) {
			if(Input.GetMouseButtonDown(0)) {
				Vector2 mPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(mPos, Vector2.zero, 0, _objLayer);

				if(hit && _currObject == null) {
					SetObject(hit);
					_lastPos = (Vector2)Input.mousePosition;
				}
			}

			if(Input.GetMouseButtonUp(0) && _currObject != null) {
				_currObject = null;
			}

			if(_currObject != null) {
				_deltaPos = new Vector2(Input.mousePosition.x - _lastPos.x, Input.mousePosition.y - _lastPos.y);
				_lastPos = (Vector2)Input.mousePosition;
			}

		}
	// Mobile
		else if(Application.isMobilePlatform) {
			if(Input.touchCount != _prevTouchCount) {
				if(Input.touchCount > _prevTouchCount) {
					Touch newTouch = Input.GetTouch(Input.touchCount - 1);
					Vector2 tPos = (Vector2)Camera.main.ScreenToWorldPoint(newTouch.position);
					RaycastHit2D hit = Physics2D.Raycast(tPos, Vector2.zero, 0, _objLayer);

					if(hit && _currObject == null) {
						SetObject(hit);
						_aimTouchID = newTouch.fingerId;
						_lastPos = (Vector2)newTouch.position;
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
				_deltaPos = new Vector2(_aimTouch.position.x - _lastPos.x, _aimTouch.position.y - _lastPos.y);
				_lastPos = _aimTouch.position;
			}
		}

		// Update position
		if(_currObject != null) {
			RectTransform rt = _currObject.GetComponent<RectTransform>();
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + _deltaPos.x, rt.anchoredPosition.y + _deltaPos.y);
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
		_lastPos = Vector2.zero;
		_deltaPos = Vector2.zero;
	}

	// Sets _currentObject and properties
	private void SetObject(RaycastHit2D hit) {
		_currObject = Instantiate<GameObject>(hit.transform.gameObject, hit.transform.parent);
		_currObject.GetComponent<EditorObject>().SetPlacementProperties(hit.transform.gameObject);
	}
	
}
