using UnityEngine;						// To inherit from Monobehaviour
using System.Collections.Generic;		// For dictionarties

 
public static class Prefabs {

	// Constant vars
	private static Dictionary<string, GameObject> _objects;

	// Dynamic vars


/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Initialize prefab objects
	public static void LoadPrefabs() {
		GameObject[] resources = Resources.LoadAll<GameObject>("Prefabs/Game/Objects");

		_objects = new Dictionary<string, GameObject>();
		foreach(GameObject obj in resources) {
			_objects.Add(obj.name, obj);
		}
	}

	// Get prefab
	public static GameObject Get(string name) {
		return _objects[name];
	}
	

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	
}
