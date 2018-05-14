using UnityEngine;						// For Unity classes

// Class handling methods related to saving & loading levels using dictionaries
public class Level{

	private DB.Level _info;


	// Constructor - takes dictionary loaded from database
	public Level(DB.Level level) {
		_info = level;
	} 


/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Returns DB.Level info
	public DB.Level Info {
		get{return _info;}
	}

	public void Build() {
		// Create and set game boundary
		GameObject boundary = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Game/Helper/Boundary"));
		Vector2 size = LevelUtil.GetSize(Info.SizeID);
		boundary.GetComponent<BoundaryController>().SetBoundaries(new Vector2(size.x, size.y), _info.Boundary);

		// Create game objects
		foreach(DB.LevelObject levelobj in _info.Objects) {
			GameObject obj 	= Static.Get(levelobj.ObjectType);
			obj = Object.Instantiate<GameObject>(obj, GameObject.Find("Objects").transform);
			obj.name = obj.name.Substring(0, obj.name.Length-7);

			obj.transform.localPosition =  new Vector2(levelobj.PosX, levelobj.PosY);
			obj.transform.localScale = new Vector2(levelobj.ScaleX, levelobj.ScaleY);
			obj.transform.localEulerAngles = new Vector3(0, 0, levelobj.Rot);

			// If in editor view, set properties or disable components that have click actions
			if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Editor") {
				EditController.UpdateObject(obj);
			}
		}
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	
}

