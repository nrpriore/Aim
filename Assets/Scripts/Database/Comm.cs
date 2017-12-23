using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System.Net;
using System.IO;
using MiniJSON;

/*
Example usage of this class

void Start () {
		Debug.Log("Sending request");

		//About Example - likely won't actually use
		//Comm.SendAbout();

		//Insert levels example (insert takes a list of database objects to insert)
		string modelName = "levels";

		Level level1 = new Level();
		level1.name = "Level 1";
		level1.version = "1.0";
		level1.numObjects = 20;

		Level level2 = new Level();
		level2.name = "Level 2";
		level2.version = "2.0";
		level2.numObjects = 10;

		Level[] levels = new Level[]{level1, level2};

		//Comm.SendInsert(modelName, levels);

		//Construct a basic query
		Query query = new Query();
		query.model_name = "levels";
		query.fields = new List<string> {"name"};
		query.filters = new List<QFilter> {new QFilter("name", "=", "Level 2")};

		//Comm.SendQuery(query);
	}

*/


public static class Comm{

	private static string APIKey = "***";
	private static string BaseURL = "https://centralcontrol.herokuapp.com/";

	private static string _SendAndGetResponse(HttpWebRequest request, string payload){
		request.ContentType = "application/json";
		request.Headers["Accepts"] = "application/json";
		request.Headers["Authorization"] = "Token " + Comm.APIKey;

		if (payload != null){
			using (StreamWriter sw = new StreamWriter(request.GetRequestStream())){
		        sw.Write(payload);
		        sw.Flush();
		        sw.Close();
		    }
		}

		HttpWebResponse response = null;

		try{
			response = request.GetResponse() as HttpWebResponse;
		}
		catch (WebException e)
        {
        	response = (HttpWebResponse) e.Response;
        }
		
	    string msg = "";
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
        {
            msg = streamReader.ReadToEnd();
        }

		return msg;
	}

	public static Dictionary<string, object> SendAbout(){
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Comm.BaseURL + "/api/v1/about");

		string response = _SendAndGetResponse(request, null);

		Debug.Log("Response: " + response);

		var aboutResponse = Json.Deserialize(response) as Dictionary<string,object>;

		Debug.Log("Company: " + (string) aboutResponse["company"]);
		Debug.Log("Version: " + (string) aboutResponse["version"]);

		return aboutResponse;
	}

	public static Dictionary<string, object> SendInsert(string modelName, DatabaseObject[] objects){
		//Create the insert JSON request
		var jsonRequest = new Dictionary<string, object>();
		jsonRequest["model_name"] = modelName;
		jsonRequest["objects"] = new object[objects.Length];

		for (int i = 0; i < objects.Length; i++){
			((object[])jsonRequest["objects"])[i] = objects[i].ToDict();
		}

		//Convert the insert request to JSON
		string payload = Json.Serialize(jsonRequest);
		Debug.Log(payload);

		//Construct the POST request
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Comm.BaseURL + "/api/v1/data");
		request.Method = "POST";
		
    	//Send the request and receive the response
		string response = _SendAndGetResponse(request, payload);

		Debug.Log("Response: " + response);

		// Serialize the response to a dictionary
		var changedResponse = Json.Deserialize(response) as Dictionary<string, object>;

		Debug.Log("Changed: " + (bool) changedResponse["changed"]);

		if (changedResponse.ContainsKey("error_code")){
			Debug.Log("error_code: " + (string) changedResponse["error_code"]);
		}

		if (changedResponse.ContainsKey("message")){
			Debug.Log("message: " + (string) changedResponse["message"]);
		}

		return changedResponse;
	}

	public static Dictionary<string, object> SendQuery(Query query){
		//Create the query JSON request
		var jsonRequest = query.ToDict();

		//Convert the query request to JSON
		string payload = Json.Serialize(jsonRequest);
		Debug.Log(payload);

		//Construct the PUT request
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Comm.BaseURL + "/api/v1/data");
		request.Method = "PUT";

		//Send the request and receive the response
		string response = _SendAndGetResponse(request, payload);

		Debug.Log("Response: " + response);

		var queryResponse = Json.Deserialize(response) as Dictionary<string, object>;

		return queryResponse;
	}

}