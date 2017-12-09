using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.SceneManagement;		// To access scene classes

/// Handles boundary mechanics in game
public class BoundaryController : MonoBehaviour {

	// Constant vars
	private EdgeCollider2D _topCol;
	private EdgeCollider2D _bottomCol;
	private EdgeCollider2D _leftCol;
	private EdgeCollider2D _rightCol;

	private LineRenderer _topLR;
	private LineRenderer _bottomLR;
	private LineRenderer _leftLR;
	private LineRenderer _rightLR;

	private float _lineWidth;

	// Dynamic vars


	// On instantiation
	void Awake() {
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
		_topCol 	= gameObject.transform.Find("Top").GetComponent<EdgeCollider2D>();
		_bottomCol 	= gameObject.transform.Find("Bottom").GetComponent<EdgeCollider2D>();
		_leftCol 	= gameObject.transform.Find("Left").GetComponent<EdgeCollider2D>();
		_rightCol 	= gameObject.transform.Find("Right").GetComponent<EdgeCollider2D>();

		_topLR	 	= _topCol.gameObject.GetComponent<LineRenderer>();
		_bottomLR 	= _bottomCol.gameObject.GetComponent<LineRenderer>();
		_leftLR 	= _leftCol.gameObject.GetComponent<LineRenderer>();
		_rightLR 	= _rightCol.gameObject.GetComponent<LineRenderer>();

		_lineWidth = _topLR.startWidth / 2f;
	}

	// Sets boundaries based on platform resolution
	private void SetBoundaries() {
		float ratio 		= (float)Screen.width / Screen.height;
		float unitHeight 	= Camera.main.orthographicSize;
		float unitWidth 	= unitHeight * ratio;

		_topCol.points 		= new [] {new Vector2(-unitWidth, unitHeight), 	new Vector2(unitWidth, unitHeight)};
		_bottomCol.points 	= new [] {new Vector2(-unitWidth, -unitHeight), new Vector2(unitWidth, -unitHeight)};
		_leftCol.points 	= new [] {new Vector2(-unitWidth, unitHeight), 	new Vector2(-unitWidth, -unitHeight)};
		_rightCol.points 	= new [] {new Vector2(unitWidth, unitHeight), new Vector2(unitWidth, -unitHeight)};

		float posW = unitWidth + _lineWidth;
		float posH = unitHeight + _lineWidth;
		_topLR.SetPositions(	new [] {new Vector3(-posW, posH), 	new Vector3(posW, posH)});
		_bottomLR.SetPositions(	new [] {new Vector3(-posW, -posH), 	new Vector3(posW, -posH)});
		_leftLR.SetPositions(	new [] {new Vector3(-posW, posH), 	new Vector3(-posW, -posH)});
		_rightLR.SetPositions(	new [] {new Vector3(posW, posH), 	new Vector3(posW, -posH)});

		_topLR.enabled 		= true;
		_bottomLR.enabled 	= true;
		_leftLR.enabled 	= true;
		_rightLR.enabled 	= true;

		// If in edit scene, update screen
		if(SceneManager.GetActiveScene().name == "Edit") {
			float offset 		= Camera.main.orthographicSize * 0.5f;
			float marginRight 	= Camera.main.orthographicSize * 0.4f;
			float marginBot 	= Camera.main.orthographicSize * 0.32f;
			Camera.main.orthographicSize += offset;
			Camera.main.transform.localPosition = new Vector3(-((offset * ratio) - marginRight),offset - marginBot,-10);
		}
	}
	
}
