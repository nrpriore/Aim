using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.UI;
 
public class AccountMenu : MonoBehaviour {

	// Constant vars
	private RectTransform _info;
	private RectTransform _menu;

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
		_target = Vector2.one;
	}

	public void CloseInfo() {
		_info.localScale = Vector2.zero;
	}

	public void CreateAccount() {
		string username = gameObject.transform.Find("MenuScreen").Find("Username").Find("Text").gameObject.GetComponent<Text>().text;
		string email = gameObject.transform.Find("MenuScreen").Find("Email").Find("Text").gameObject.GetComponent<Text>().text;
		
		if(ValidInfo(username, email)) {
			if(Server.CreateUser(username, email)) {
				PlayerPrefs.SetString("username", username);
				PlayerPrefs.Save();
				CloseUI();
			}
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_menu = gameObject.GetComponent<RectTransform>();
		_info = gameObject.transform.Find("MenuScreen").Find("Info").gameObject.GetComponent<RectTransform>();

		_target = Vector2.zero;

		gameObject.transform.Find("MenuScreen").Find("Username").gameObject.GetComponent<InputField>().characterLimit = 24;
	}

	private bool ValidInfo(string username, string email) {
		int numErrors = 0;
		if(username.Contains(" ")) {
			Debug.Log("Username can't contain spaces");
			numErrors++;
		}
		if(username.Length < 3 ) {
			Debug.Log("Username must be at least 3 characters");
			numErrors++;
		}
		if(!email.Contains("@") || !email.Contains(".com")) {
			Debug.Log("Invalid email");
			numErrors++;
		}
		return numErrors == 0;
	}

	private void CloseUI() {
		_target = Vector2.zero;
	}

/// -----------------------------------------------------------------------------------------------
/// Database Queries ------------------------------------------------------------------------------
	


}
