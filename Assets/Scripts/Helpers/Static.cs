using UnityEngine;						// To inherit from Monobehaviour
using System.Collections.Generic;		// For dictionarties
using System;							// For Array class

// Holds static variables 
public static class Static {

	// Public static vars
	public static DB.Level CurrentLevel;
	public static GameObject[] EditMenuObjects;

	// Private static vars
	private static Dictionary<string, GameObject> _objects;


/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Load prefabs so that we can use the ID's in the database without having to hardcode the ID's in the app
	// Matches on name
	public static void LoadPrefabs() {
		// Load the gameobjects from Unity
		GameObject[] objects = Resources.LoadAll<GameObject>("Prefabs/Game/Objects");
		EditMenuObjects = Resources.LoadAll<GameObject>("Prefabs/EditMenu");

		// Load the IDs/Names from the database
		List<DB.ObjectType> types = Server.GetObjectTypes();

		// Sort EditMenuObjects based on display_id
		int[] menuorder = new int[types.Count];
		for(int i = 0; i < EditMenuObjects.Length; i++) {
			menuorder[i] = types.Find(x => x.Name == EditMenuObjects[i].name).DisplayID;
		}
		Array.Sort(menuorder, EditMenuObjects);

		// Match names from Unity to DB and set _objects dictionary to enable "Get" method
		_objects = new Dictionary<string, GameObject>();
		foreach(DB.ObjectType type in types) {
			_objects.Add(type.Name, Array.Find(objects, obj => obj.name == type.Name));
		}
	}

	// Get prefab by name
	public static GameObject Get(string name) {
		return _objects[name];
	}
	
}
