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
	public static bool LoadLevel(int? levelID) {
		// Later, load level from database via levelID
		// For now, use hardcoded level
		Dictionary<string, object> level = HardcodedTestLevel();

		// Map dictionary to Level instance and create on screen
		Static.CurrentLevel = new Level(level);

		bool success = true;
		return success;
	}

/// -----------------------------------------------------------------------------------------------
/// Private methods -------------------------------------------------------------------------------

	// Creates a hardcoded level for testing
	private static Dictionary<string, object> HardcodedTestLevel() {
		// Set level properties
		Dictionary<string, object> level = new Dictionary<string, object>();
		level.Add("ID", 1);
		level.Add("Name", "HardcodedTestLevel");
		level.Add("Size", 1);
		level.Add("Boundary", true);
		level.Add("Gravity", 1);
		
		// Add in-game objects
		List<Dictionary<string, object>> objects = new List<Dictionary<string, object>>();
		objects.Add(ObjToDict("01-Ball", Vector2.one, new Vector2(6, 5), 0));
		objects.Add(ObjToDict("03-Platform", Vector2.one, new Vector2(13.96f, 5.46f), 118.8f));
		objects.Add(ObjToDict("04-Bounce", Vector2.one, new Vector2(17.29f, 7.73f), 28.93f));
		objects.Add(ObjToDict("02-Goal", Vector2.one, new Vector2(16.53f, 1.25f), 0));
		level.Add("Objects", objects);

		return level;
	}

	// Wrapper for a game-object's dictionary
	private static Dictionary<string, object> ObjToDict(string objID, Vector2 scale, Vector2 pos, float rotZ) {
		Dictionary<string, object> obj = new Dictionary<string, object>();
		obj.Add("Name", objID);
		obj.Add("Pos", pos);
		obj.Add("Scale", scale);
		obj.Add("Rot", rotZ);

		return obj;
	}
	
}
