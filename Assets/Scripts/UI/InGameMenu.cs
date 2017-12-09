using UnityEngine;						// To inherit from Monobehaviour

 
public class InGameMenu : MonoBehaviour {

	// Constant vars
	private RectTransform _carot;		// Reference to Carot transform
	private RectTransform _menu;		// Reference to expandable piece of menu
	private float _lerpThreshold;		// Don't continue to lerp if within this range

	// Dynamic vars
	private bool _expanded;				// Is the menu expanded or not
	private float _targetRot;			// Target rotation of carot
	private float _targetPos;			// Target position of menu


	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		if(Mathf.Abs(_targetRot - _carot.localEulerAngles.z) > _lerpThreshold) {
			float rot = Mathf.Lerp(_carot.localEulerAngles.z, _targetRot, Time.deltaTime * 10f);
			_carot.localEulerAngles = new Vector3(0, 0, rot);
		}

		if(Mathf.Abs(_targetPos - _menu.localPosition.x) > _lerpThreshold) {
			float pos = Mathf.Lerp(_menu.localPosition.x, _targetPos, Time.deltaTime * 10f);
			_menu.localPosition = new Vector3(pos, -200, 0);
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	public void ToggleMenu() {
		_expanded = !_expanded;
		_targetRot = (_expanded)? 90 : 270;
		_targetPos = (_expanded)? -130 : 0;
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_carot = GameObject.Find("Carot").GetComponent<RectTransform>();
		_menu = gameObject.transform.Find("Menu").gameObject.GetComponent<RectTransform>();

		_expanded = false;
		_targetRot = _carot.localEulerAngles.z;

		_lerpThreshold = 0.1f;
	}
	
}
