using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.UI;

 
public class EditorNavLevelUI : MonoBehaviour {

	// Constant vars
	private EditorNavDetailedView _detailedView;
	private Image _selected;

	// Dynamic vars
	public Level GameLevel;


	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		
	}

	public int Index {get;set;}


/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	public void SetProperties(Level level) {
		GameLevel = level;
		transform.Find("Name").gameObject.GetComponent<Text>().text = GameLevel.Info.Name;

		// DEV - until rating is implemented
		if(GameLevel.Info.Submitted) {
			float rating = 0.5f + Random.value * 3.5f;
			ColorRating(rating);
		}
	}

	public void OpenDetailedView() {
		if(EditorNavController.SelectedLevel != null) {
			if(EditorNavController.SelectedLevel != this) {
				EditorNavController.SelectedLevel.ToggleSelected();
				ToggleSelected();
			}
		}else {
			ToggleSelected();
		}
		EditorNavController.SelectedLevel = this;

		_detailedView.OpenDetailedView(this);
	}

	public void ToggleSelected() {
		_selected.enabled = !_selected.enabled;
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_detailedView = GameObject.Find("MyLevels").transform.Find("DetailedView").gameObject.GetComponent<EditorNavDetailedView>();
		_selected = transform.Find("Selected").GetComponent<Image>();
	}

	private void ColorRating(float rating) {
		//Image stars = transform.Find("Stars").gameObject.GetComponent<Image>();
		
	}
	
}
