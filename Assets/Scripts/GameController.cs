using UnityEngine;						// To inherit from Monobehaviour
using UnityEngine.SceneManagement;		// To change scenes

 
public class GameController : MonoBehaviour {

	// Constant vars

	// Dynamic vars
	private bool _shot;


	// On instantiation
	void Start() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		
	}

/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Returns _shot
	public bool Shot {
		get{return _shot;}
	}

	// Runs when ball is shot
	public void Shoot() {
		_shot = true;
	}

	// Restarts game
	public void Restart() {
		SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Initialize game variables
	private void InitVars() {
		_shot = false;
	}
	
}
