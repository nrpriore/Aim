using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorMenu : MonoBehaviour {

	public static bool ObjSelected;

	// Constant vars
	private EditController _ec;
	private CanvasGroup _group;
	private RectTransform _menu;
	private GameObject _open;
	private GameObject _close;
	private Button _submit;
	private Button _test;
	private Text _testText;
	private RectTransform _editorObjects;
	private ScrollRect _scroll;
	private Transform _objects;

	// Dynamic vars
	private int _targetSize;
	private int _targetAlpha;

	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		_menu.localScale = Vector2.Lerp(_menu.localScale, Vector2.one * _targetSize, Time.deltaTime * 20f);
		_group.alpha = Mathf.Lerp(_group.alpha, (float)_targetAlpha, Time.deltaTime * 10f);
		if(_group.alpha <= .05f && _targetSize == 1) {
			ToggleMenu();
		}
		if(_targetSize == 0) {
			_editorObjects.anchoredPosition = Vector2.zero;
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	public void ToggleMenu() {
		if(_targetSize == 0) {
			_ec.TakeScreenShot();
			_targetSize = 1;
			_open.SetActive(false);
			_close.SetActive(true);
			_scroll.horizontal = true;
			UpdateTestButton();
		} else {
			_targetSize = 0;
			_open.SetActive(true);
			_close.SetActive(false);
			_editorObjects.anchoredPosition = Vector2.zero;
			_scroll.horizontal = false;
		}
	}

	public void FadeOut() {
		_targetAlpha = 0;
	}
	public void FadeIn() {
		_targetAlpha = 1;
	}
	public bool Open() {
		return _targetSize == 1;
	}

	public void DeleteLevel() {
		if(!Static.CurrentLevel.Submitted) {
			if(Server.DeleteLevel()) {
				Static.CurrentLevel = null;
				SceneManager.LoadSceneAsync("EditorNav", LoadSceneMode.Single);
			}
		}
	}

	public void TestLevel() {
		if(ValidLevel()) {
			UpdateObjects();
			if(Server.SaveCurrentLevel()) {
				SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
			}
		}
	}

	public void SaveAndExit() {
		UpdateObjects();
		if(Server.SaveCurrentLevel()) {
			Static.CurrentLevel = null;
			SceneManager.LoadSceneAsync("EditorNav", LoadSceneMode.Single);
		}
	}

	public void SubmitLevel() {
		if(Server.VerifyBeatable()) {
			if(Server.SubmitCurrentLevel()) {
				Static.CurrentLevel = null;
				SceneManager.LoadSceneAsync("EditorNav", LoadSceneMode.Single);
			}
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_ec = GameObject.Find("EditController").GetComponent<EditController>();
		_group = gameObject.GetComponent<CanvasGroup>();
		_menu = transform.Find("MenuScreen").gameObject.GetComponent<RectTransform>();
		_open = transform.Find("MenuButton").Find("Open").gameObject;
		_close = transform.Find("MenuButton").Find("Close").gameObject;
		_submit = _menu.gameObject.transform.Find("Submit").gameObject.GetComponent<Button>();
		_test = _menu.gameObject.transform.Find("Test").gameObject.GetComponent<Button>();
		_testText = _menu.gameObject.transform.Find("Test").Find("Text").gameObject.GetComponent<Text>();
		_editorObjects = GameObject.Find("EditorObjects").GetComponent<RectTransform>();
		_scroll = GameObject.Find("ScrollView").GetComponent<ScrollRect>();
		_objects = GameObject.Find("Objects").transform;

		if(!Static.CurrentLevel.Beatable) {
			_submit.interactable = false;
			_submit.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = "Must 'Test Level' and win before submitting";
		}

		if(Static.CurrentLevel.Submitted) {
			_menu.gameObject.transform.Find("Delete").gameObject.SetActive(false);
		}

		_targetSize = 0;
		_targetAlpha = 1;
		_editorObjects.anchoredPosition = Vector2.zero;
		_scroll.horizontal = false;

		ObjSelected = false;
	}

	private void UpdateTestButton() {
		if(!ValidLevel()) {
			_test.interactable = false;
			_testText.text = "Requires Ball and Goal to test";
		}else {
			_test.interactable = true;
			_testText.text = "Test Level";
		}
	}

	private bool ValidLevel() {
		bool a = _objects.Find("Ball");
		bool b = _objects.Find("Goal");

		return a && b;
	}

	private void UpdateObjects() {
		Static.CurrentLevel.Objects.Clear();
		foreach(Transform child in _objects) {
			Static.CurrentLevel.Objects.Add(new DB.LevelObject(child.gameObject));
		}
	}
	
}