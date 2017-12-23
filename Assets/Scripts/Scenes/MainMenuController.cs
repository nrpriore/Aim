using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.SceneManagement;		// To change scenes

 
public class MainMenuController : MonoBehaviour {

	// Constant vars
	

	// Dynamic vars



	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	public void OpenEditorNav() {
		SceneManager.LoadSceneAsync("EditorNav", LoadSceneMode.Single);
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		
	}
	
}
