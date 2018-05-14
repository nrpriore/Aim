using System.Collections.Generic;


// Delete Models ------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------

public class DeleteUser : Delete {

	public DeleteUser(DB.User user) {
		filters.Add(new QFilter(DB.Models.GetModelInfo(GetType().Name, "PrimaryKey"), "=", user.Username));
		numRows = (Server.VerifyAccount(user.Username))? 1 : 0;
	}
}

public class DeleteLevel : Delete {

	public DeleteLevel(DB.Level level) {
		filters.Add(new QFilter(DB.Models.GetModelInfo(GetType().Name, "PrimaryKey"), "=", level.LevelID));
		numRows = (Server.VerifyLevel(level.LevelID))? 1 : 0;
	}
}

public class DeleteLevelObjects : Delete {

	public DeleteLevelObjects(DB.Level level) {
		filters.Add(new QFilter("level_id", "=", level.LevelID));
		numRows = Server.GetLevelObjects(level.LevelID).Count;
	}
}


// Delete Components --------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------

public abstract class Delete : DatabaseObject
{
	public string model_name;
	public List<QFilter> filters;
	public int numRows;

	protected Delete() {
		model_name = DB.Models.GetModelInfo(GetType().Name, "Name");
		filters = new List<QFilter>();
		numRows = 0;
	}

	public bool SendDelete() {

		return Parse.Int(Comm.SendDelete(ToDict())["changed"]) == numRows;
	}

	public override Dictionary<string, object> ToDict() {
		Dictionary<string, object> dict = new Dictionary<string, object>();
		dict["model_name"] = model_name;

		dict["filters"] = new Dictionary<string, object>[filters.Count];
		for(int i = 0; i < filters.Count; i++) {
			((Dictionary<string, object>[]) dict["filters"])[i] = filters[i].ToDict();
		}

		return dict;
	}
}
