using UnityEngine;

public class EditorDeleteObject : MonoBehaviour {

	// Constant vars
	private CanvasGroup _group;
	private RectTransform _icon;
	private BoxCollider2D _col;

	// Dynamic vars
	private int _targetSize;
	private int _targetAlpha;

	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		_icon.localScale = Vector2.Lerp(_icon.localScale, Vector2.one + (new Vector2(0.3f, 0.3f) * _targetSize), Time.deltaTime * 20f);
		_group.alpha = Mathf.Lerp(_group.alpha, (float)_targetAlpha, Time.deltaTime * 10f);

		if(EditController.CurrObject != null) {
			if(_col.IsTouching(EditController.CurrObject.gameObject.GetComponent<Collider2D>())) {
				if(_targetSize == 0) {
					_targetSize = 1;
				}
			}else {
				if(_targetSize == 1) {
					_targetSize = 0;
				}
			}
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	public void FadeOut() {
		_targetAlpha = 0;
	}
	public void FadeIn() {
		_targetAlpha = 1;
	}

	public void CheckForDelete(Transform obj) {
		if(_targetSize == 1) {
			Destroy(obj.gameObject);
			_targetSize = 0;
			_targetAlpha = 0;
		}
	}


/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_group = gameObject.GetComponent<CanvasGroup>();
		_icon = transform.Find("Icon").gameObject.GetComponent<RectTransform>();
		_col = _icon.gameObject.GetComponent<BoxCollider2D>();

		_targetSize = 0;
		_targetAlpha = 0;
	}
	
}
