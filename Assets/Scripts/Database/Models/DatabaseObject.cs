using System.Collections.Generic;

public abstract class DatabaseObject {

	public string ModelName;

	public abstract Dictionary<string, object> ToDict();

}
