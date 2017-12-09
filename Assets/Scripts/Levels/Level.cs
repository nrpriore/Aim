using UnityEngine;						// For Unity classes
using System.Collections.Generic;		// For dictionaries

// Class handling methods related to saving & loading levels using dictionaries
public class Level {

	private int _id;
	private string _name;
	private Vector2 _size;

	private List<GameObject> _objects;


	// Constructor - takes dictionary loaded from database
	public Level(Dictionary<string, object> levelDict) {
		// Make sure dictionary isn't null
		if(levelDict.ContainsKey("ID")) {
			_id 	= Parse.Int(levelDict["ID"]);
			_name 	= Parse.String(levelDict["Name"]);
			_size 	= Parse.VectorTwo(levelDict["Size"]);

			List<Dictionary<string, object>> objDicts = (List<Dictionary<string,object>>)levelDict["Objects"];

			foreach(Dictionary<string, object> obj in objDicts) {
				//InGameObj gobj 
			}
		}
	} 


/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------



/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	private struct InGameObj {
		private GameObject _obj;
		private Vector2 _pos;

		private InGameObj(GameObject obj, Vector2 pos) {
			_obj = obj;
			_pos = pos;
		}
	}
	
}

