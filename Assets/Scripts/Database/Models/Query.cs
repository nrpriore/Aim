using System.Collections.Generic;


// Query Models -------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------

public class QueryUser : Query {

	public QueryUser(List<QFilter> _filters, string operation = "queryAll") {
		switch(operation) {
			case "queryAll":
				fields = new List<string> {"username", "email"};
				break;
			case "verify":
				fields = new List<string> {"username"};
				break;
		}
		filters = _filters;
	}
}

public class QueryLevel : Query {

	public QueryLevel(List<QFilter> _filters, string operation = "queryAll") {
		switch(operation) {
			case "queryAll":
				fields = new List<string> {"level_id", "name", "submitted", "submitted_by", "boundary", "size_id", "beatable"};
				break;
			case "verify":
				fields = new List<string> {"level_id"};
				break;
			case "beatable":
				fields = new List<string> {"beatable"};
				break;
		}
		filters = _filters;
	}
}

public class QueryObjectType : Query {

	public QueryObjectType(List<QFilter> _filters, string operation = "queryAll") {
		switch(operation) {
			case "queryAll":
				fields = new List<string> {"display_id", "name"};
				break;
		}
		filters = _filters;
	}
}

public class QueryLevelObject : Query {

	public QueryLevelObject(List<QFilter> _filters, string operation = "queryAll") {
		switch(operation) {
			case "queryAll":
				fields = new List<string> {"level_id", "object_type", "pos_x", "pos_y", "scale_x", "scale_y", "rot"};
				break;
		}
		filters = _filters;
	}
}


// Query Components ---------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------

public abstract class Query : DatabaseObject {
	public string model_name;
	public List<string> fields;
	public List<QFilter> filters;

	protected Query(){
		model_name = DB.Models.GetModelInfo(GetType().Name, "Name");
		fields = new List<string>();
		filters = new List<QFilter>();
	}

	public List<Dictionary<string, object>> SendQuery() {
		return Comm.SendQuery(ToDict());
	}

	public override Dictionary<string, object> ToDict(){
		Dictionary<string, object> dict = new Dictionary<string, object>();
		dict["model_name"] = model_name;
		dict["fields"] = fields.ToArray();
		dict["filters"] = new Dictionary<string, object>[filters.Count];

		for (int i = 0; i < filters.Count; i++){
			((Dictionary<string, object>[]) dict["filters"])[i] = filters[i].ToDict();
		}

		return dict;
	}
}


public class QFilter : DatabaseObject {
	public string field;
	public string operand;
	public object value;

	public QFilter(string f, string o, object v){
		field = f;
		operand = o;
		value = v;
	}

	public override Dictionary<string, object> ToDict(){
		Dictionary<string, object> dict = new Dictionary<string, object>();
		dict["field"] = field;
		dict["operand"] = operand;
		dict["value"] = value;

		return dict;
	}
}
