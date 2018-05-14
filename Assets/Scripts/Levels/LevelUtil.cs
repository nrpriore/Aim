using UnityEngine;						// For Unity classes
using System.Collections.Generic;		// For dictionaries

// Class handling methods related to saving & loading levels using dictionaries
public static class LevelUtil {




/// -----------------------------------------------------------------------------------------------
/// Public methods --------------------------------------------------------------------------------

	// Packs all level information into a dictionary
	public static bool SaveLevel() {


		bool success = true;
		return success;
	}

	// Loads level data from levelID and creates Level instance
	public static bool LoadCurrentLevel() {
		// Verify level exists in database via levelID, or HardCoded level if that's null
		bool success = true;
		if(!Server.VerifyLevel(Static.CurrentLevel.LevelID)) {
			success = false;
			Static.CurrentLevel = HardcodedTestLevel();
		}

		// Map dictionary to Level instance and create on screen
		Level level = new Level(Static.CurrentLevel);
		level.Build();

		return success;
	}

	public static Vector2 GetSize(int _sizeID) {
		if(System.Enum.IsDefined(typeof(SizeEnum), _sizeID)) {
			SizeStruct tempsize = new SizeStruct((SizeEnum)_sizeID);
			Vector2 size = new Vector2(tempsize.x, tempsize.y);
			return size;
		}
		Debug.Log("Invalid size ID");
		return Vector2.zero;
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Creates a hardcoded level for testing
	private static DB.Level HardcodedTestLevel() {
		// Set level properties
		DB.Level level = new DB.Level("HardCodedTestLevel");
		level.Name = "HardcodedTestLevel";
		level.Boundary = true;
		level.SizeID = 1;
		level.Beatable = false;
		
		// Add in-game objects
		level.Objects = new List<DB.LevelObject>();
		level.Objects.Add(new DB.LevelObject(ObjToDict("Ball", Vector2.one, new Vector2(6, 5), 0)));
		level.Objects.Add(new DB.LevelObject(ObjToDict("Platform", Vector2.one, new Vector2(13.96f, 5.46f), 118.8f)));
		level.Objects.Add(new DB.LevelObject(ObjToDict("Bounce", Vector2.one, new Vector2(17.29f, 7.73f), 28.93f)));
		level.Objects.Add(new DB.LevelObject(ObjToDict("Goal", Vector2.one, new Vector2(16.53f, 1.25f), 0)));		

		return level;
	}

	// Wrapper for a game-object's dictionary
	private static Dictionary<string, object> ObjToDict(string objID, Vector2 scale, Vector2 pos, float rotZ) {
		Dictionary<string, object> obj = new Dictionary<string, object>();
		obj.Add("level_id", Static.CurrentLevel.LevelID);
		obj.Add("object_type", objID);
		obj.Add("pos_x", pos.x);
		obj.Add("pos_y", pos.y);
		obj.Add("scale_x", scale.x);
		obj.Add("scale_y", scale.y);
		obj.Add("rot", rotZ);

		return obj;
	}

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
					x = 14.22224f;
					y = 8f;
					break;
				case SizeEnum.M:
					Name = "Medium";
					x = 17.77778f;
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
