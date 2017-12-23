using UnityEngine;						// For Unity classes
using System.Collections.Generic;		// For dictionaries

// Class handling methods related to saving & loading levels using dictionaries
public class Level{

	private int _id;
	private string _name;
	private int _sizeID;
	private bool _walled;
	private float _gravity;

	private List<GameObject> _objects;
	private SizeStruct _size;


	// Constructor - takes dictionary loaded from database
	public Level(Dictionary<string, object> levelDict) {
		// Check for nulls
		if(levelDict.ContainsKey("ID")) {
			// Set level variables
			_id 	 = Parse.Int(levelDict["ID"]);
			_name 	 = Parse.String(levelDict["Name"]);
			_sizeID  = Parse.Int(levelDict["Size"]);
			_walled  = Parse.Bool(levelDict["Boundary"]);
			_gravity = Parse.Float(levelDict["Gravity"]);

			// Create size struct and set game boundary
			GameObject boundary = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Game/Helper/Boundary"));
			if(System.Enum.IsDefined(typeof(SizeEnum), _sizeID)) {
				_size = new SizeStruct((SizeEnum)_sizeID);
				Vector2 size = new Vector2(_size.x, _size.y);
				boundary.GetComponent<BoundaryController>().SetBoundaries(size, _walled);
			}else{
				Debug.Log("Invalid level size");
			}

			// Create game objects
			List<Dictionary<string, object>> objDicts = (List<Dictionary<string,object>>)levelDict["Objects"];
			_objects = new List<GameObject>();
			foreach(Dictionary<string, object> objDict in objDicts) {
				// Check for nulls
				if(objDict.ContainsKey("Name")) {
					GameObject obj 	= Prefabs.Get(Parse.String(objDict["Name"]));
					obj = Object.Instantiate<GameObject>(obj);
					obj.name = obj.name.Substring(0, obj.name.Length-7);

					obj.transform.localPosition =  Parse.VectorTwo(objDict["Pos"]);
					obj.transform.localScale = Parse.VectorTwo(objDict["Scale"]);
					obj.transform.localEulerAngles = new Vector3(0, 0, Parse.Float(objDict["Rot"]));

					_objects.Add(obj);
				}
			}

		}
	} 


/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Returns level name
	public string Name {
		get{return _name;}
	}

	// Returns objects list
	public List<GameObject> Objects {
		get{return _objects;}
	}

	// Returns Gravity
	public float Gravity {
		get{return _gravity;}
	}

	// Returns Size
	public SizeStruct Size {
		get{return _size;}
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------




/// -----------------------------------------------------------------------------------------------
/// Helpers ---------------------------------------------------------------------------------------

	// Defines potential level sizes
	public enum SizeEnum {
		S = 0,
		M = 1,
		L = 2
	}

	// Builds struct containin size variables
	public struct SizeStruct {
		public string Name;
		public float x, y;

		public SizeStruct(SizeEnum strSize) {
			switch(strSize) {
				case SizeEnum.S:
					Name = "Small";
					x = 12.44445f;
					y = 7f;
					break;
				case SizeEnum.M:
					Name = "Medium";
					x = //25
						//20.55556f
						17.77778f
						//12
						;
					y = 10f;
					break;
				case SizeEnum.L:
					Name = "Large";
					x = 23.11112f;
					y = 13f;
					break;
				default:
					Name = "Undefined";
					x = 1f;
					y = 1f;
					break;
			}
		}
	}
	
}

