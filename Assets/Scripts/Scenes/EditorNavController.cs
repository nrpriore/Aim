using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.SceneManagement;		// To change scenes
using System.Collections.Generic;		// For lists

 
public class EditorNavController : MonoBehaviour {

	// Constant vars
	

	// Dynamic vars
	private List<EditorNavLevelUI> _myLevels;
	public static EditorNavLevelUI SelectedLevel;

	// On instantiation
	void Start() {
		InitVars();

		LoadMyLevels();
	}

	// Runs every frame
	void Update() {
		
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	public void RateNewLevel() {
		Static.CurrentLevel = new DB.Level("meeeep");
		SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
	}

	public void ReRateLevel() {

	}

	public void PlayTopLevels() {

	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_myLevels = new List<EditorNavLevelUI>();
		SelectedLevel = null;
	}


/// -----------------------------------------------------------------------------------------------
/// Database Queries ------------------------------------------------------------------------------

	// Loads levels that you've submitted
	private void LoadMyLevels() {
		GameObject nonsubmit = Resources.Load<GameObject>("Prefabs/UI/NonSubmittedLevel");
		GameObject submit = Resources.Load<GameObject>("Prefabs/UI/SubmittedLevel");
		Transform parent = GameObject.Find("MyLevelsContent").transform;

		List<Level> levels = Server.LoadMyLevels();
		levels.Sort((x, y) => string.Compare(x.Info.LevelID, y.Info.LevelID, System.StringComparison.Ordinal));

		GameObject level;
		float height = Mathf.Max(submit.GetComponent<RectTransform>().sizeDelta.y, nonsubmit.GetComponent<RectTransform>().sizeDelta.y);
		for(int i = 0; i < levels.Count; i++) {
			level = (!levels[i].Info.Submitted)? Instantiate(nonsubmit, parent) : Instantiate(submit, parent);
			level.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -height * i);

			_myLevels.Add(level.GetComponent<EditorNavLevelUI>());
			_myLevels[i].SetProperties(levels[i]);
		}
		parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height * levels.Count);
	}
	
}
