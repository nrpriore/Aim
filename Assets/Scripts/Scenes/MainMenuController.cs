using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.SceneManagement;		// To change scenes
using System.IO;						// For DirectoryInfo

public class MainMenuController : MonoBehaviour {

	// Constant vars

	// Dynamic vars

	// Use this for initialization
	void Start () {
		// Testing
		//PlayerPrefs.DeleteAll();
		//PlayerPrefs.SetString("username", "NRP");
		//Dev.InsertObjectTypes();
		//Dev.ClearLevelRecords("NRP");
		// End testing

		InitVars();
		SetUpDirectories();

		if(NewAccount()) {
			OpenAccountUI();
		}
		else {
			if(!Server.VerifyAccount(PlayerPrefs.GetString("username"))) {
				PlayerPrefs.DeleteAll();
				OpenAccountUI();
			}
		}
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
		Static.LoadPrefabs();
	}

	private void SetUpDirectories() {
		DirectoryInfo resources = new DirectoryInfo(Application.persistentDataPath + "/Resources");
		if(!resources.Exists) {
			resources.Create();
		}
		DirectoryInfo screenshots = new DirectoryInfo(Application.persistentDataPath + "/Resources/Screenshots");
		if(!screenshots.Exists) {
			screenshots.Create();
		}
	}

	// Checks if account exists or not. Returns true if account doesn't exist
	private bool NewAccount() {
		return !PlayerPrefs.HasKey("username");
	}

	// Opens account creation UI
	private void OpenAccountUI() {
		GameObject.Find("AccountMenu").GetComponent<AccountMenu>().OpenUI();
	}

}
