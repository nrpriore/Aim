using System.Collections.Generic;
using UnityEngine;


// Available Tables ------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------
// User
// Level
// LevelObjects
// Object
// LevelRating
// -----------------------------------------------------------------------------------------------


namespace DB {

	/// -----------------------------------------------------------------------------------------------
	/// User ------------------------------------------------------------------------------------------
	public class User : DatabaseObject {
		private string _username;
		public string Username {get{return _username;}}
		public string Email;

		// Used to insert new rows // specific columns set in Server class
		public User(string name) {
			_username = name;
		}

		// Used to query rows // maps the db columns to the data class
		public User(Dictionary<string, object> dict) {
			_username = Parse.String(dict["username"]);
			Email = Parse.String(dict["email"]);
		}

		// Used to save rows //  maps the data class to db columns
		public override Dictionary<string, object> ToDict(){
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict["username"] = _username;
			dict["email"] = Email;

			return dict;
		}

		// Database operations
		public bool Create() {
			InsertUser insert = new InsertUser(this);
			bool success = insert.SendInsert();
			return success;
		}
		public bool Update() {
			UpdateUser update = new UpdateUser(this);
			bool success = update.SendUpdate();
			return success;
		}
		public bool Delete() {
			DeleteUser delete = new DeleteUser(this);
			bool success = delete.SendDelete();
			return success;
		}
	}

	/// -----------------------------------------------------------------------------------------------
	/// Level -----------------------------------------------------------------------------------------
	public class Level : DatabaseObject {
		private string _levelID;
		public string LevelID {get{return _levelID;}}

		public bool Submitted;
		public string SubmittedBy;
		public string Name;
		public bool Boundary;
		public int SizeID;
		public bool Beatable;

		public List<LevelObject> Objects;

		// Used to insert new rows // specific columns set in Server class
		public Level(string levelID) {
			_levelID = levelID;
			Objects = new List<LevelObject>();
		}

		// Used to query rows // maps the db columns to the data class
		public Level(Dictionary<string, object> dict) {
			_levelID = Parse.String(dict["level_id"]);
			Submitted = Parse.Bool(dict["submitted"]);
			SubmittedBy = Parse.String(dict["submitted_by"]);

			Name = Parse.String(dict["name"]);
			Boundary = Parse.Bool(dict["boundary"]);
			SizeID = Parse.Int(dict["size_id"]);
			Beatable = Parse.Bool(dict["beatable"]);

			Objects = Server.GetLevelObjects(_levelID);
		}

		// Used to save rows // maps the data class to db columns
		public override Dictionary<string, object> ToDict(){
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict["level_id"] = _levelID;
			dict["submitted"] = Submitted;
			dict["submitted_by"] = SubmittedBy;

			dict["name"] = Name;
			dict["boundary"] = Boundary;
			dict["size_id"] = SizeID;
			dict["beatable"] = Beatable;

			return dict;
		}

		// Database operations
		public bool Create() {
			InsertLevel insert = new InsertLevel(this);
			bool success = insert.SendInsert();
			return success;
		}
		public bool Update() {
			UpdateLevel update = new UpdateLevel(this);
			bool success = update.SendUpdate();
			return success;
		}
		public bool Delete() {
			DeleteLevel delete = new DeleteLevel(this);
			bool success = delete.SendDelete();
			return success;
		}
		public bool InsertLevelObjects() {
			InsertLevelObjects insert = new InsertLevelObjects(this);
			bool success = insert.SendInsert();
			return success;
		}
		public bool DeleteLevelObjects() {
			DeleteLevelObjects delete = new DeleteLevelObjects(this);
			bool success = delete.SendDelete();
			return success;
		}
	}

	/// -----------------------------------------------------------------------------------------------
	/// LevelObjects ----------------------------------------------------------------------------------
	public class LevelObject : DatabaseObject {
		public string LevelID;
		public string ObjectType;
		public float PosX;
		public float PosY;
		public float ScaleX;
		public float ScaleY;
		public float Rot;

		public LevelObject(GameObject obj) {
			Transform tr = obj.transform;

			LevelID 	= Static.CurrentLevel.LevelID;
			ObjectType 	= obj.name;
			PosX 		= tr.localPosition.x;
			PosY 		= tr.localPosition.y;
			ScaleX 		= tr.localScale.x;
			ScaleY 		= tr.localScale.y;
			Rot 		= tr.localEulerAngles.z;
		}

		// Used to map the db columns to the data class
		public LevelObject(Dictionary<string, object> dict) {
			LevelID = Parse.String(dict["level_id"]);
			ObjectType = Parse.String(dict["object_type"]);
			PosX = Parse.Float(dict["pos_x"]);
			PosY = Parse.Float(dict["pos_y"]);
			ScaleX = Parse.Float(dict["scale_x"]);
			ScaleY = Parse.Float(dict["scale_y"]);
			Rot = Parse.Float(dict["rot"]);
		}

		// Used to map the data class to db columns
		public override Dictionary<string, object> ToDict(){
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict["level_id"] = LevelID;
			dict["object_type"] = ObjectType;
			dict["pos_x"] = PosX;
			dict["pos_y"] = PosY;
			dict["scale_x"] = ScaleX;
			dict["scale_y"] = ScaleY;
			dict["rot"] = Rot;

			return dict;
		}
	}

	/// -----------------------------------------------------------------------------------------------
	/// Object ----------------------------------------------------------------------------------------
	public class ObjectType : DatabaseObject {
		private string _name;
		public string Name {get{return _name;}}

		public int DisplayID;

		// Used to insert new rows // specific columns set in Server class
		public ObjectType(string name, int displayID) {
			_name = name;
			DisplayID = displayID;
		}

		// Used to map the db columns to the data class
		public ObjectType(Dictionary<string, object> dict) {
			DisplayID = Parse.Int(dict["display_id"]);
			_name = Parse.String(dict["name"]);
		}

		// Used to map the data class to db columns
		public override Dictionary<string, object> ToDict(){
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict["display_id"] = DisplayID;
			dict["name"] = Name;

			return dict;
		}
	}

	/// -----------------------------------------------------------------------------------------------
	/// LevelRating -----------------------------------------------------------------------------------
	/*public class LevelRating : DatabaseObject {
		public int UserID;
		public int LevelID;
		public int Rating;

		public override Dictionary<string, object> ToDict(){
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict["user_id"] = UserID;
			dict["level_id"] = LevelID;
			dict["rating"] = Rating;

			return dict;
		}
	}*/


	public static class Models {

		public static string GetModelInfo(string dataName, string infoType) {
			List<string> operations = new List<string> {"Insert", "Query", "Update", "Delete"};
			foreach(string operation in operations) {
				if(dataName.Contains(operation)) {
					dataName = dataName.Substring(operation.Length);
					break;
				}
			}

			Dictionary<string, string> info = new Dictionary<string, string>();
			switch(dataName) {
				case "User":
					info["Name"] 		= "User";
					info["PrimaryKey"] 	= "username";
					break;
				case "Level":
					info["Name"] 		= "Level3";
					info["PrimaryKey"] 	= "level_id";
					break;
				case "LevelObject": case "LevelObjects":
					info["Name"] 		= "LevelObject";
					break;
				case "ObjectType": case "ObjectTypes":
					info["Name"] 		= "ObjectType";
					info["PrimaryKey"] 	= "name";
					break;
				default:
					return null;
			}

			return (info.ContainsKey(infoType))? info[infoType] : null;
		}
	}

}