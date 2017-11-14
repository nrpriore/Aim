using UnityEngine;						// To inherit from Monobehaviour

 
public class EditorObject : MonoBehaviour {

	// Constant vars
	

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

	// Called after instantiation
	public void SetObjProperties() {
		// Update starting size to match in-game sprite
		gameObject.name = gameObject.name.Substring(0, gameObject.name.Length - 7);
		Sprite sprite = Resources.Load<GameObject>("Prefabs/Game/" + gameObject.name).GetComponent<SpriteRenderer>().sprite;

		Vector2 worldSize = sprite.rect.size / sprite.pixelsPerUnit;
		Vector2 screenSize = 0.5f * worldSize / Camera.main.orthographicSize;
		screenSize.y *= Camera.main.aspect;
		Vector2 pixelSize = new Vector2(screenSize.x * Camera.main.pixelWidth, screenSize.y * Camera.main.pixelHeight);
		pixelSize /= Camera.main.aspect;

		gameObject.GetComponent<RectTransform>().sizeDelta = pixelSize;
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		
	}
	
}
