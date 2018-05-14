using System.Collections.Generic;


// Update Models ------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------

public class UpdateUser : Update {

	public UpdateUser(DB.User user) {
		filters.Add(new QFilter(DB.Models.GetModelInfo(GetType().Name, "PrimaryKey"), "=", user.Username));
		changes = user.ToDict();
	}
}

public class UpdateLevel : Update {

	public UpdateLevel(DB.Level level) {
		filters.Add(new QFilter(DB.Models.GetModelInfo(GetType().Name, "PrimaryKey"), "=", level.LevelID));
		changes = level.ToDict();
	}
}


// Update Components --------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------

public abstract class Update : DatabaseObject
{
	public string model_name;
	public List<QFilter> filters;
	public Dictionary<string, object> changes;

	protected Update() {
		model_name = DB.Models.GetModelInfo(GetType().Name, "Name");
		filters = new List<QFilter>();
		changes = new Dictionary<string, object>();
	}

	public bool SendUpdate() {
		return Parse.Int(Comm.SendUpdate(ToDict())["changed"]) > 0;
	}

	public override Dictionary<string, object> ToDict() {
		Dictionary<string, object> dict = new Dictionary<string, object>();
		dict["model_name"] = model_name;

		dict["filters"] = new Dictionary<string, object>[filters.Count];
		for(int i = 0; i < filters.Count; i++) {
			((Dictionary<string, object>[]) dict["filters"])[i] = filters[i].ToDict();
		}

		dict["changes"] = changes;

		return dict;
	}
}
