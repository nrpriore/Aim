using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.SceneManagement;		// To change scenes
using System.Collections.Generic;		// For lists

 
public class EditorNavController : MonoBehaviour {

	// Constant vars
	

	// Dynamic vars
	private List<EditorNavLevelUI> _stagedLevels;
	private List<EditorNavLevelUI> _submittedLevels;


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

	public void OpenNewEditor() {
		Static.LevelID = null;
		SceneManager.LoadSceneAsync("Editor", LoadSceneMode.Single);
	}

	public void OpenEditor(int levelID) {
		Static.LevelID = levelID;
		SceneManager.LoadSceneAsync("Editor", LoadSceneMode.Single);
	}

	public void RateNewLevel() {

	}

	public void ReRateLevel() {

	}

	public void PlayTopLevels() {

	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		
	}

	// Loads levels that you've submitted
	private void LoadMyLevels() {

	}
	
}
