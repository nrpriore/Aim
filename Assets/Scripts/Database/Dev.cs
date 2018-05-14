using UnityEngine;
using System.Collections.Generic;

public static class Dev {

	public static bool ClearLevelRecords(string username) {
		List<QFilter> filters = new List<QFilter>() {new QFilter("submitted_by", "=", username)};
		QueryLevel query = new QueryLevel(filters);

		List<Dictionary<string, object>> response = query.SendQuery();

		List<DB.Level> dblevels = new List<DB.Level>();
		foreach(Dictionary<string, object> dict in response) {
			dblevels.Add(new DB.Level(dict));
		}

		bool success = true;
		int deleted = 0;
		foreach(DB.Level dblevel in dblevels) {
			DeleteLevel delete = new DeleteLevel(dblevel);
			success = delete.SendDelete();
			if(!success) {
				break;
			}
			Functions.DeleteScreenshot(dblevel.LevelID);
			deleted++;
		}
		if(success && username == PlayerPrefs.GetString("username")) {
			PlayerPrefs.DeleteKey("num_levels");
		}
		Debug.Log("Deleted " + deleted + " of " + dblevels.Count + " levels");
		return success;
	}

	public static bool InsertObjectTypes() {
		List<DB.ObjectType> types = new List<DB.ObjectType>();
		types.Add(new DB.ObjectType("Goal", 0));
		types.Add(new DB.ObjectType("Ball", 1));
		types.Add(new DB.ObjectType("Platform", 2));
		types.Add(new DB.ObjectType("Bounce", 3));
		types.Add(new DB.ObjectType("Spikes", 4));
		
		InsertObjectTypes insert = new InsertObjectTypes(types);
		return insert.SendInsert();
	}
}

public class InsertObjectTypes : Insert {

	public InsertObjectTypes(List<DB.ObjectType> types) {
		objects = new Dictionary<string, object>[types.Count];
		for(int i = 0; i < types.Count; i++) {
			objects[i] = types[i].ToDict();
		}
	}
}

public class DeleteLevels : Delete {

	public DeleteLevels(DB.Level level) {
		filters.Add(new QFilter(DB.Models.GetModelInfo(GetType().Name, "PrimaryKey"), "=", level.LevelID));
	}
}