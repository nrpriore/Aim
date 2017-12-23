using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;

public class Level : DatabaseObject
{
    public string name;
    public string version;
    public int numObjects;

    public override Dictionary<string, object> ToDict(){
    	Dictionary<string, object> dict = new Dictionary<string, object>();
    	dict["name"] = name;
    	dict["version"] = version;
    	dict["numObjects"] = numObjects;

    	return dict;
    }
}