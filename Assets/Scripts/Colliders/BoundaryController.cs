using UnityEngine;						// To inherit from Monobehaviour

/// Handles boundary mechanics in game
public class BoundaryController : MonoBehaviour {

	// Constant vars
	private Transform _background;
	private Transform _mask;

	private EdgeCollider2D _topCol;
	private EdgeCollider2D _bottomCol;
	private EdgeCollider2D _leftCol;
	private EdgeCollider2D _rightCol;

	private LineRenderer _topLR;
	private LineRenderer _bottomLR;
	private LineRenderer _leftLR;
	private LineRenderer _rightLR;

	private float _lineRadius;

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

	// Sets boundaries based on level size
	public void SetBoundaries(Vector2 size, bool walled) {
		float unitHeight 	= size.y;
		float unitWidth 	= size.x;

		// Set camera to center point of level
		Camera.main.transform.localPosition = new Vector3(unitWidth/2f, unitHeight/2f, -10);

		float levelAspectRatio 	= unitHeight/unitWidth;
		float camAspectRatio 	= (float)Screen.height/Screen.width;
		float diffAspectRatio 	= camAspectRatio/levelAspectRatio;

		// Set camera size
		// If diffaspectratio < 1, the level is taller than the camera so match height (there will be blank space left & right)
		// if > 1, the level is wider than the camera, so match width (there will be blank space top and bottom)
		float targetOrtho;
		if(diffAspectRatio <= 1f) {
			targetOrtho = unitHeight/2f;
		}else {
			targetOrtho = unitWidth * camAspectRatio / 2f;
		}
		Camera.main.orthographicSize = targetOrtho;

		// Set up walls and colliders if walled is set to true
		if(walled) {
			// Set up background
			_background.localPosition = new Vector3(unitWidth/2f, unitHeight/2f,1);
			Vector2 localsize = _background.gameObject.GetComponent<SpriteRenderer>().bounds.size;
			float scaleY = targetOrtho*2f / localsize.y;
			float scaleX = (targetOrtho*2f / camAspectRatio) / localsize.x;
			_background.localScale = new Vector3(scaleX, scaleY, 1);

			// Set up mask
			localsize = _mask.gameObject.GetComponent<SpriteRenderer>().bounds.size;
			scaleY = unitHeight / localsize.y;
			scaleX = unitWidth / localsize.x;
			_mask.localScale = new Vector3(scaleX, scaleY, 1);

			// Set up boundary colliders and lines
			_topCol.points 		= new [] {new Vector2(0, unitHeight), 	new Vector2(unitWidth, unitHeight)};
			_bottomCol.points 	= new [] {new Vector2(0, 0), 			new Vector2(unitWidth, 0)};
			_leftCol.points 	= new [] {new Vector2(0, 0), 			new Vector2(0, unitHeight)};
			_rightCol.points 	= new [] {new Vector2(unitWidth, 0), 	new Vector2(unitWidth, unitHeight)};
			_topCol.edgeRadius = _bottomCol.edgeRadius = _leftCol.edgeRadius = _rightCol.edgeRadius = _lineRadius;

			_topLR.SetPositions(	new [] {new Vector3(0, unitHeight, -1), new Vector3(unitWidth, unitHeight, -1)});
			_bottomLR.SetPositions(	new [] {new Vector3(0, 0, -1), 			new Vector3(unitWidth, 0, -1)});
			_leftLR.SetPositions(	new [] {new Vector3(0, 0, -1), 			new Vector3(0, unitHeight, -1)});
			_rightLR.SetPositions(	new [] {new Vector3(unitWidth, 0, -1), 	new Vector3(unitWidth, unitHeight, -1)});
		}else {
			// Else, disable all background components
			_background.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			_topCol.enabled = _bottomCol.enabled = _leftCol.enabled = _rightCol.enabled = false;
			_topLR.enabled = _bottomLR.enabled = _leftLR.enabled = _rightLR.enabled = false;
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_background = gameObject.transform.Find("Background");
		_mask = _background.Find("Mask");

		_topCol 	= gameObject.transform.Find("Top").GetComponent<EdgeCollider2D>();
		_bottomCol 	= gameObject.transform.Find("Bottom").GetComponent<EdgeCollider2D>();
		_leftCol 	= gameObject.transform.Find("Left").GetComponent<EdgeCollider2D>();
		_rightCol 	= gameObject.transform.Find("Right").GetComponent<EdgeCollider2D>();

		_topLR	 	= _topCol.gameObject.GetComponent<LineRenderer>();
		_bottomLR 	= _bottomCol.gameObject.GetComponent<LineRenderer>();
		_leftLR 	= _leftCol.gameObject.GetComponent<LineRenderer>();
		_rightLR 	= _rightCol.gameObject.GetComponent<LineRenderer>();

		_lineRadius = _topLR.startWidth / 2f;
	}
	
}
