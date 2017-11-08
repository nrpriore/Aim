using UnityEngine;						// To inherit from Monobehaviour

 
public class BoundaryController : MonoBehaviour {

	// Constant vars
	private EdgeCollider2D _top;
	private EdgeCollider2D _bottom;
	private EdgeCollider2D _left;
	private EdgeCollider2D _right;

	// Dynamic vars


	// On instantiation
	void Start() {
		InitVars();
		SetBoundaries();
	}

	// Runs every frame
	void Update() {
		
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_top 	= gameObject.transform.Find("Top").GetComponent<EdgeCollider2D>();
		_bottom = gameObject.transform.Find("Bottom").GetComponent<EdgeCollider2D>();
		_left 	= gameObject.transform.Find("Left").GetComponent<EdgeCollider2D>();
		_right 	= gameObject.transform.Find("Right").GetComponent<EdgeCollider2D>();
	}

	// Sets boundaries based on platform resolution
	private void SetBoundaries() {
		float ratio 		= (float)Screen.width / Screen.height;
		float unitHeight 	= Camera.main.orthographicSize;
		float unitWidth 	= unitHeight * ratio;

		_top.points 	= new [] {new Vector2(-unitWidth, unitHeight), 	new Vector2(unitWidth, unitHeight)};
		_bottom.points 	= new [] {new Vector2(-unitWidth, -unitHeight), new Vector2(unitWidth, -unitHeight)};
		_left.points 	= new [] {new Vector2(-unitWidth, unitHeight), 	new Vector2(-unitWidth, -unitHeight)};
		_right.points 	= new [] {new Vector2(unitWidth, unitHeight), 	new Vector2(unitWidth, -unitHeight)};
	}
	
}
