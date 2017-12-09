using UnityEngine;						// To inherit from Monobehaviour

 
public class EditorObject : MonoBehaviour {

	// Constant vars
	Collider2D _collider;				// Reference to 2D collider of object
	RectTransform _rt;					// Reference to recttransfrom of object
	private Transform _placedObjParent;	// Reference to parent transform of placed objects

	// Dynamic vars



	// On instantiation
	void Awake() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Called after instantiation into scrollview
	public void SetScrollViewProperties() {
		// Update starting size to match in-game sprite
		gameObject.name = gameObject.name.Substring(0, gameObject.name.Length - 7);
		Sprite sprite = Resources.Load<GameObject>("Prefabs/Game/" + gameObject.name).GetComponent<SpriteRenderer>().sprite;

		Vector2 worldSize = sprite.rect.size / sprite.pixelsPerUnit;
		Vector2 screenSize = 0.5f * worldSize / Camera.main.orthographicSize;
		screenSize.y *= Camera.main.aspect;
		Vector2 pixelSize = new Vector2(screenSize.x * Camera.main.pixelWidth, screenSize.y * Camera.main.pixelHeight);
		pixelSize /= Camera.main.aspect;

		_rt.sizeDelta = pixelSize;

		// Based on size, update click collider
		_collider = gameObject.GetComponent<Collider2D>();
		string type = _collider.GetType().ToString().Substring(12);
		switch(type) {
			case "CircleCollider2D":
				CircleCollider2D circle = (CircleCollider2D)_collider;
				circle.radius = _rt.sizeDelta.x * 0.8f;
				break;
			case "CapsuleCollider2D":
				CapsuleCollider2D capsule = (CapsuleCollider2D)_collider;
				capsule.size = _rt.sizeDelta * 1.2f;
				break;
			case "BoxCollider2D":
				BoxCollider2D box = (BoxCollider2D)_collider;
				box.size = _rt.sizeDelta * 1.2f;
				break;
		}
	}

	// Called after instantiation to be placed
	public void SetPlacementProperties(GameObject clone) {
		_placedObjParent = GameObject.Find("PlacedObjects").transform;

		gameObject.name = clone.name;

		RectTransform cloneRT = clone.GetComponent<RectTransform>();

		_rt.anchoredPosition = new Vector2(cloneRT.anchoredPosition.x, cloneRT.anchoredPosition.y);
		_rt.SetParent(_placedObjParent);
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_rt = gameObject.GetComponent<RectTransform>();
	}
	
}
