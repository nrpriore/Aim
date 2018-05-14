using UnityEngine;						
using System.Collections.Generic;


public static class Server {

	// Constant vars
	

	// Dynamic vars

/// -----------------------------------------------------------------------------------------------
/// Generic Methods -------------------------------------------------------------------------------

	public static bool VerifyAccount(string username) {
		List<QFilter> filters = new List<QFilter> {new QFilter("username", "=", username)};
		QueryUser query = new QueryUser(filters, "verify");
		List<Dictionary<string, object>> response = query.SendQuery();

		return response.Count == 1;
	}
	public static bool VerifyLevel(string levelID) {
		List<QFilter> filters = new List<QFilter> {new QFilter("level_id", "=", levelID)};
		QueryLevel query = new QueryLevel(filters, "verify");
		List<Dictionary<string, object>> response = query.SendQuery();

		return response.Count == 1;
	}
	public static bool VerifyBeatable() {
		List<QFilter> filters = new List<QFilter> {new QFilter("level_id", "=", Static.CurrentLevel.LevelID)};
		QueryLevel query = new QueryLevel(filters, "beatable");
		List<Dictionary<string, object>> response = query.SendQuery();

		Static.CurrentLevel.Beatable = false;
		if(response.Count > 0) {
			Static.CurrentLevel.Beatable = Parse.Bool(response[0]["beatable"]);
		}
		return Static.CurrentLevel.Beatable;
	}

	public static DB.Level LoadLevel(string levelID) {
		List<QFilter> filters = new List<QFilter> {new QFilter("level_id", "=", levelID)};
		List<DB.Level> levels = Server.QueryLevels(filters);
		
		return (levels.Count == 0)? null : levels[0];
	}

	public static List<Level> LoadMyLevels() {
		List<QFilter> filters = new List<QFilter> {new QFilter("submitted_by", "=", PlayerPrefs.GetString("username"))};
		List<DB.Level> dblevels = Server.QueryLevels(filters);

		List<Level> levels = new List<Level>();
		for(int i = 0; i < dblevels.Count; i++) {
			levels.Add(new Level(dblevels[i]));
		}

		return levels;
	}

	public static bool SubmitCurrentLevel() {
		Static.CurrentLevel.Submitted = true;
		return Server.SaveCurrentLevel();
	}

	public static bool SaveCurrentLevel() {
		if(Server.UpdateCurrentLevelObjects()) {
			return Static.CurrentLevel.Update();
		}
		return false;
	}

	public static bool UpdateCurrentLevelObjects() {
		if(Static.CurrentLevel.DeleteLevelObjects()) {
			return Static.CurrentLevel.InsertLevelObjects();
		}
		return false;
	}

	public static List<DB.LevelObject> GetLevelObjects(string levelID) {
		List<QFilter> filters = new List<QFilter> {new QFilter("level_id", "=", levelID)};
		List<DB.LevelObject> objects = QueryLevelObjects(filters);
		return objects;
	}

	public static List<DB.ObjectType> GetObjectTypes() {
		List<QFilter> filters = new List<QFilter>();
		List<DB.ObjectType> types = QueryObjectTypes(filters);
		return types;
	}

/// -----------------------------------------------------------------------------------------------
/// Create methods --------------------------------------------------------------------------------

	public static bool CreateUser(string username, string email) {
		// Create the user
		DB.User user = new DB.User(username);
		user.Email = email;

		return user.Create();
	}

	public static bool CreateLevel(bool boundary, int sizeID, bool loadNow = false) {
		// Create the level
		string id = PlayerPrefs.GetString("username") + (PlayerPrefs.GetInt("num_levels", 0) + 1);
		DB.Level level = new DB.Level(id);
		level.Name = id;
		level.Boundary = boundary;
		level.SizeID = sizeID;
		level.Submitted = false;
		level.SubmittedBy = PlayerPrefs.GetString("username");

		// Call insert
		bool success = level.Create();

		if(success) {
			if(loadNow) {
				Static.CurrentLevel = level;
			}
			PlayerPrefs.SetInt("num_levels", PlayerPrefs.GetInt("num_levels", 0) + 1);
		}

		return success;
	}

/// -----------------------------------------------------------------------------------------------
/// Update methods --------------------------------------------------------------------------------



/// -----------------------------------------------------------------------------------------------
/// Delete methods --------------------------------------------------------------------------------

	public static bool DeleteUser(DB.User user) {
		return user.Delete();
	}

	public static bool DeleteLevel() {
		if(Static.CurrentLevel.DeleteLevelObjects()) {
			if(Static.CurrentLevel.Delete()) {
				Functions.DeleteScreenshot(Static.CurrentLevel.LevelID);
				return true;
			}
		}
		return false;
	}

/// -----------------------------------------------------------------------------------------------
/// Query methods (private) -----------------------------------------------------------------------

	private static List<DB.User> QueryUsers(List<QFilter> filters) {
		QueryUser query = new QueryUser(filters);

		List<Dictionary<string, object>> response = query.SendQuery();

		List<DB.User> users = new List<DB.User>();
		foreach(Dictionary<string, object> dict in response) {
			users.Add(new DB.User(dict));
		}

		return users;
	}

	private static List<DB.Level> QueryLevels(List<QFilter> filters) {
		QueryLevel query = new QueryLevel(filters);

		List<Dictionary<string, object>> response = query.SendQuery();

		List<DB.Level> levels = new List<DB.Level>();
		foreach(Dictionary<string, object> dict in response) {
			levels.Add(new DB.Level(dict));
		}

		return levels;
	}

	private static List<DB.LevelObject> QueryLevelObjects(List<QFilter> filters) {
		QueryLevelObject query = new QueryLevelObject(filters);

		List<Dictionary<string, object>> response = query.SendQuery();

		List<DB.LevelObject> objects = new List<DB.LevelObject>();
		foreach(Dictionary<string, object> dict in response) {
			objects.Add(new DB.LevelObject(dict));
		}

		return objects;
	}

	private static List<DB.ObjectType> QueryObjectTypes(List<QFilter> filters) {
		QueryObjectType query = new QueryObjectType(filters);

		List<Dictionary<string, object>> response = query.SendQuery();

		List<DB.ObjectType> objects = new List<DB.ObjectType>();
		foreach(Dictionary<string, object> dict in response) {
			objects.Add(new DB.ObjectType(dict));
		}

		return objects;
	}

}
