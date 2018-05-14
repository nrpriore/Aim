using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorNavDetailedView : MonoBehaviour {

	// Constant vars
	private Image _BG;
	private RectTransform _menu;
	private Text _name;
	private Image _preview;
	private GameObject _nopreview;

	private Image _nameIcon;

	private GameObject _submittedUI;
	private Text _starText;
	private StarRating _starRating;
	private Text _played;
	private Text _users;

	private GameObject _nonsubmittedUI;
	private Text _boundary;
	private Text _size;


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

	public void EditLevel() {
		Static.CurrentLevel = EditorNavController.SelectedLevel.GameLevel.Info;
 		SceneManager.LoadSceneAsync("Editor", LoadSceneMode.Single);
	}

	public void OpenDetailedView(EditorNavLevelUI levelui) {
		_target = Vector2.one;
		_BG.color = Functions.UpdateColor(_BG.color, a: .6f);
		_BG.raycastTarget = true;

		_name.text = levelui.GameLevel.Info.Name;

		string filePath = Application.persistentDataPath + "/Resources/Screenshots/" + levelui.GameLevel.Info.LevelID + ".jpg";
		if (System.IO.File.Exists(filePath)) {
			var bytes = System.IO.File.ReadAllBytes(filePath);
     		Texture2D tex = new Texture2D(1, 1);
     		tex.LoadImage(bytes);
     		_preview.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
     		_nopreview.SetActive(false);
		}
		if(_preview.sprite == null) {
			_nopreview.SetActive(true);
		}

		if(levelui.GameLevel.Info.Submitted) {
			_submittedUI.SetActive(true);
			_nonsubmittedUI.SetActive(false);
		}else {
			_nonsubmittedUI.SetActive(true);
			_submittedUI.SetActive(false);
			_boundary.text = (levelui.GameLevel.Info.Boundary)? "Boundary: On" : "Boundary: Off";
			_size.text = "Size: " + ((LevelUtil.SizeEnum)levelui.GameLevel.Info.SizeID);
		}

	}

	public void CloseDetailedView() {
		_target = Vector2.zero;
		_BG.color = Functions.UpdateColor(_BG.color, a: 0f);
		_BG.raycastTarget = false;

		EditorNavController.SelectedLevel.ToggleSelected();
		EditorNavController.SelectedLevel = null;
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_menu = gameObject.GetComponent<RectTransform>();
		_BG = GameObject.Find("MyLevelsBG").GetComponent<Image>();
		_name = transform.Find("Name").gameObject.GetComponent<Text>();

		_submittedUI = transform.Find("Submitted").gameObject;
		_starText = _submittedUI.transform.Find("StarsText").gameObject.GetComponent<Text>();
		_starRating = _submittedUI.transform.Find("DetailedStars").gameObject.GetComponent<StarRating>();
		_preview = _submittedUI.transform.Find("Preview").gameObject.GetComponent<Image>();
		_played = _submittedUI.transform.Find("Stats").Find("PlayedText").gameObject.GetComponent<Text>();
		_users = _submittedUI.transform.Find("Stats").Find("UsersText").gameObject.GetComponent<Text>();

		_nonsubmittedUI = transform.Find("NonSubmitted").gameObject;
		_preview = _nonsubmittedUI.transform.Find("Preview").gameObject.GetComponent<Image>();
		_nopreview = _nonsubmittedUI.transform.Find("NoPreview").gameObject;
		_boundary = _nonsubmittedUI.transform.Find("Boundary").gameObject.GetComponent<Text>();
		_size = _nonsubmittedUI.transform.Find("Size").gameObject.GetComponent<Text>();

		_target = Vector2.zero;
	}
	
}
