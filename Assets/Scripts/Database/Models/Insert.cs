using System.Collections.Generic;


// Insert Models ------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------

public class InsertUser : Insert {

	public InsertUser(DB.User user) {
		objects = new Dictionary<string, object>[1];
		objects[0] = user.ToDict();
	}
}

public class InsertLevel : Insert {

	public InsertLevel(DB.Level level) {
		objects = new Dictionary<string, object>[1];
		objects[0] = level.ToDict();
	}
}

public class InsertLevelObjects : Insert {

	public InsertLevelObjects(DB.Level level) {
		objects = new Dictionary<string, object>[level.Objects.Count];
		for(int i = 0; i < level.Objects.Count; i++) {
			objects[i] = level.Objects[i].ToDict();
		}
		numRows = level.Objects.Count;
	}
}


// Insert Components --------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------

public abstract class Insert : DatabaseObject {
	public string model_name;
	public Dictionary<string, object>[] objects;
	public int numRows;

	protected Insert(){
		model_name = DB.Models.GetModelInfo(GetType().Name, "Name");
		numRows = 1;
	}

	public bool SendInsert() {
		return Parse.Int(Comm.SendInsert(ToDict())["changed"]) == numRows;
	}

	public override Dictionary<string, object> ToDict(){
		Dictionary<string, object> dict = new Dictionary<string, object>();
		dict["model_name"] = model_name;

		dict["objects"] = objects;

		return dict;
	}
}
