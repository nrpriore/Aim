using UnityEngine;						// To inherit from Monobehaviour

 
public class EditController : MonoBehaviour {

	// Constant vars
	private int _objLayer;				// Reference to the layer mask holding objects for collision ("Objects")
	private int _editLayer;
	private EditorMenu _menu;
	private EditorDeleteObject _delete;	

	// Dynamic vars
	public static Transform CurrObject;		// Object currently being moved
	private int _prevTouchCount;		// TouchCount of previous frame
	private Touch _aimTouch;			// The touch used as an input for aiming
	private int _aimTouchID;			// The fingerID associated with _aimTouch
	private Vector2 _deltaPos;			// Difference between _lastPos and current position

	private Touch activeTouch;


	// On instantiation
	void Start() {
		InitVars();

		if(Static.CurrentLevel != null) { 
			LevelUtil.LoadCurrentLevel();
			UpdateMenus();
		}
	}

	// Runs every frame
	void Update() {
		if(_menu.Open()) {
			return;
		}
	// Editor
		if(Application.isEditor) {
			if(Input.GetMouseButtonDown(0)) {
				Vector2 mPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				
				RaycastHit2D hit = Physics2D.Raycast(mPos, Vector2.zero, 0, _objLayer);
				RaycastHit2D hitedit = Physics2D.Raycast(mPos, Vector2.zero, 0, _editLayer);

				if(hitedit) {

				}else if(hit) {
					if(!Equals(hit.transform, CurrObject)) {
						UnselectObject();
						SelectObject(hit.transform);
					}
				}else {
					UnselectObject();
				}
			}
		}
	// Mobile
		else if(Application.isMobilePlatform) {
			if(Input.touchCount != _prevTouchCount) {
				if(Input.touchCount > _prevTouchCount) {
					Touch newTouch = Input.GetTouch(Input.touchCount - 1);
					Vector2 tPos = (Vector2)Camera.main.ScreenToWorldPoint(newTouch.position);
					RaycastHit2D hit = Physics2D.Raycast(tPos, Vector2.zero, 0, _objLayer);

					if(hit && CurrObject == null) {
						//_menu.FadeOut();
						//_delete.FadeIn();
						CurrObject = hit.transform;
						CurrObject.Find("EditorSelect").gameObject.SetActive(true);
						//_aimTouchID = newTouch.fingerId;
						//_deltaPos = (Vector2)CurrObject.localPosition - _aimTouch.position;
					}
				}
				_prevTouchCount = Input.touchCount;
			}

			/*// Since Touch is a struct (stupid Unity), assign _aimTouch each frame... (stupid Unity)...
			foreach(Touch touch in Input.touches) {
				if(touch.fingerId == _aimTouchID) {
					_aimTouch = touch;
				}
			}

			if(_aimTouch.phase == TouchPhase.Ended) {
				_menu.FadeIn();
				PlaceObject();
				_delete.FadeOut();
			}

			if(CurrObject != null) {
				CurrObject.localPosition = _aimTouch.position + _deltaPos;
			}*/
		}

	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	public void TakeScreenShot() {
		ScreenShot();
	}

	public void SelectAddedObject(GameObject obj, int pointerID = -1) {
		SelectObject(obj.transform, pointerID);
	}

	public static void UpdateObject(GameObject obj) {
		Functions.SetLayerRecursively(obj, 12);

		GameObject select = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Editor/EditorSelect"), obj.transform);
		select.name = select.name.Substring(0, select.name.Length - 7);
		Vector2 size = obj.GetComponent<SpriteRenderer>().sprite.bounds.size;
		select.GetComponent<Transform>().localScale = Vector2.one * Mathf.Max(((Mathf.Max(size.x, size.y) + .9f) / (select.GetComponent<SpriteRenderer>().sprite.bounds.size.x - 1.1f)), 0.6f);
		select.SetActive(false);

		switch(obj.name) {
			case "Ball":
				obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
				obj.GetComponent<BallController>().enabled = false;
				Destroy(obj.transform.Find("ClickBoundary").gameObject);
				Destroy(obj.transform.Find("Arrow").gameObject);
				obj.transform.Find("EditorSelect").GetComponent<SpriteRenderer>().enabled = false;
				obj.transform.Find("EditorSelect").Find("Rotate").GetComponent<Collider2D>().enabled = false;
			break;
			default:
				if(obj.GetComponent<Rigidbody2D>()) {
					obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
				}
			break;
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_objLayer = LayerMask.GetMask("EditorObject");
		_editLayer = LayerMask.GetMask("EditorSelect");
		_menu = GameObject.Find("EditorMenu").GetComponent<EditorMenu>();
		_delete = GameObject.Find("DeleteObject").GetComponent<EditorDeleteObject>();

		_prevTouchCount = 0;
		CurrObject = null;
		_deltaPos = Vector2.zero;
	}

	private void UpdateMenus() {
		// Move menu based on boundary/phone resolutions
		if(Static.CurrentLevel.Boundary) {
			Vector2 size = LevelUtil.GetSize(Static.CurrentLevel.SizeID);
			float xOffset = Mathf.Max(ScreenUnitsWidth() - size.x, 0) / 2f;
			float yOffset = Mathf.Max(ScreenUnitsHeight() - size.y, 0) / 2f;
			if(Mathf.Abs(xOffset) > 0.01f || Mathf.Abs(yOffset) > 0.1f) {
				// Update Menu Button
				RectTransform button = GameObject.Find("MenuButton").GetComponent<RectTransform>();
				button.anchoredPosition = new Vector2(button.anchoredPosition.x + (xOffset * PixelsPerUnit()), button.anchoredPosition.y + (yOffset * PixelsPerUnit()));
				// Update Menu Screen
				RectTransform screen = GameObject.Find("MenuScreen").GetComponent<RectTransform>();
				screen.offsetMin = new Vector2(screen.offsetMin.x + (xOffset * PixelsPerUnit()), screen.offsetMin.y + (yOffset * PixelsPerUnit()));
				screen.offsetMax = new Vector2(screen.offsetMax.x - (xOffset * PixelsPerUnit()), screen.offsetMax.y - (yOffset * PixelsPerUnit()));
				// Update Trash Icon
				RectTransform delete = GameObject.Find("DeleteObject").GetComponent<RectTransform>();
				delete.anchoredPosition = new Vector2(-xOffset * PixelsPerUnit(), -yOffset * PixelsPerUnit());
			}
		}

		// Load objects into menu
		Transform parent = GameObject.Find("EditorObjects").transform;
		for(int i = 0; i < Static.EditMenuObjects.Length; i++) {
			RectTransform obj = Instantiate<GameObject>(Static.EditMenuObjects[i], parent).GetComponent<RectTransform>();
			obj.sizeDelta *= PixelsPerUnit();
			float padding = 70f;
			float x = parent.gameObject.GetComponent<RectTransform>().sizeDelta.x + padding + (obj.sizeDelta.x / 2f);
			obj.anchoredPosition = new Vector2(x, 0f);
			x += (obj.sizeDelta.x / 2f) + padding;
			parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(x, 0);

			obj.gameObject.GetComponent<EditorObject>().SetProperties();
		}
	}

	private void SelectObject(Transform hitTR, int pointerID = -1) {
		CurrObject = hitTR;
		CurrObject.Find("EditorSelect").gameObject.SetActive(true);
		CurrObject.Find("EditorSelect").GetComponent<EditorSelect>().MoveOnSelect();
		CurrObject.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
	}
	private void UnselectObject() {
		if(CurrObject != null) {
			CurrObject.Find("EditorSelect").gameObject.SetActive(false);
			CurrObject.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
			CurrObject = null;
		}
	}
	private void PlaceObject() {
		_delete.CheckForDelete(CurrObject);
		CurrObject = null;
	}

	private float ScreenUnitsHeight() {
		return Camera.main.orthographicSize * 2f;
	}
	private float ScreenUnitsWidth() {
		return ScreenUnitsHeight() * ((float)Screen.width/Screen.height);
	}
	private float PixelsPerUnit() {
		return Screen.height / (Camera.main.orthographicSize * 2f);
	}

	private void ScreenShot() {
		Camera _camera = Camera.main;
		Texture2D _screenShot;

		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
		_camera.targetTexture = rt;
		_screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		_camera.Render();
		RenderTexture.active = rt;
		_screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		_screenShot.Apply();

		_camera.targetTexture = null;
		RenderTexture.active = null;
		Destroy(rt);

		System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Resources/Screenshots/" + Static.CurrentLevel.LevelID + ".jpg", _screenShot.EncodeToJPG(10));
	}
	
}


