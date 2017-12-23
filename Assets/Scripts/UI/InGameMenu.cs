using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.UI;

 
public class InGameMenu : MonoBehaviour {

	// Constant vars
	private const float LERP_THRESHOLD = 0.0001f;
	private RectTransform _menu;
	private Image _background;

	// Dynamic vars
	private bool _open;
	private Vector2 _target;


	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		_menu.localScale = Vector2.Lerp(_menu.localScale, _target, Time.deltaTime * 45f);
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	public void ToggleMenu() {
		_open = !_open;
		if(_open) {
			_target = Vector2.one;
			_background.enabled = true;
		}else {
			_target = Vector2.zero;
			_background.enabled = false;
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_menu = gameObject.transform.Find("MenuScreen").gameObject.GetComponent<RectTransform>();
		_background = gameObject.transform.Find("Background").gameObject.GetComponent<Image>();

		_open = false;
		_target = Vector2.zero;
	}
	
}
