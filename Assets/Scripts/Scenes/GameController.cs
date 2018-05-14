using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.SceneManagement;		// To access scene classes

/// Handles administrative game mechanics
/// I.E. scene loading, stats, game phase, etc...
public class GameController : MonoBehaviour {

	// Constant vars

	// Dynamic vars
	private float _gravityScale;

	private bool _shot;
	private bool? _success;


	// On instantiation
	void Start() {
		InitVars();

		if(Static.CurrentLevel != null) { 
			LoadLevel();
		}
	}

	// Runs every frame
	void Update() {
		
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Runs on success
	public void Success() {
		if(_success != true) {
			_success = true;
			Debug.Log("Success");
		}
	}

	// Runs on fail
	public void Fail() {
		if(_success != false) {
			_success = false;
			Debug.Log("Fail");
		}
	}

	// Returns _shot
	public bool Shot {
		get{return _shot;}
	}

	// Returns gravity scale
	public float GravityScale {
		set{_gravityScale = value;}
		get{return _gravityScale;}
	}

	// Runs when ball is shot
	public void Shoot() {
		_shot = true;
	}

	// Restarts game
	public void Restart() {
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_shot = false;
		_gravityScale = 1f;
	}

	// Loads level
	private void LoadLevel() {
		LevelUtil.LoadCurrentLevel();
		gameObject.AddComponent<InputController>();
	}
	
}
