using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.UI;
using UnityEngine.SceneManagement;

 
public class NewLevelMenu : MonoBehaviour {

	// Constant vars
	private Image _BG;
	private RectTransform _menu;
	private Toggle _boundary;
	private Slider _sizeSlider;
	private Text _sizeText;
	

	// Dynamic vars
	private Vector2 _target;


	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		_menu.localScale = Vector2.Lerp(_menu.localScale, _target, Time.deltaTime * 20f);
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	public void OpenUI() {
		_boundary.isOn = true;
		_sizeSlider.value = 1;
		_target = Vector2.one;

		_BG.color = Functions.UpdateColor(_BG.color, a: .6f);
		_BG.raycastTarget = true;
	}

	public void CloseUI() {
		_target = Vector2.zero;

		_BG.color = Functions.UpdateColor(_BG.color, a: 0f);
		_BG.raycastTarget = false;
	}

	public void UpdateSizeText() {
		switch((int)_sizeSlider.value) {
			case 0:
				_sizeText.text = "Small";
				break;
			case 1:
				_sizeText.text = "Medium";
				break;
			case 2:
				_sizeText.text = "Large";
				break;
			default:
				_sizeText.text = "";
				break;
		}
	}

	public void CreateLevel() {
		bool hasBoundary = _boundary.isOn;

		if(Server.CreateLevel(hasBoundary, (int)_sizeSlider.value, true)) {
			SceneManager.LoadSceneAsync("Editor", LoadSceneMode.Single);
		}else {
			Debug.Log("Failed to create level");
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_BG = GameObject.Find("NewMenuBG").GetComponent<Image>();
		_menu = gameObject.GetComponent<RectTransform>();
		_boundary = gameObject.transform.Find("MenuScreen").Find("Boundary").gameObject.GetComponent<Toggle>();
		_sizeSlider = gameObject.transform.Find("MenuScreen").Find("Size").Find("Slider").gameObject.GetComponent<Slider>();
		_sizeText = _sizeSlider.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();

		_target = Vector2.zero;
		_sizeSlider.value = 1;
		UpdateSizeText();
	}
	
}
