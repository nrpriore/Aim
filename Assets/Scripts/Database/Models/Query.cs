using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;

public class Query : DatabaseObject
{
    public string model_name;
    public List<string> fields;
    public List<QFilter> filters;

    public Query(){
        
    }

    public Query(string modelName){
        model_name = modelName;
        fields = new List<string>();
        filters = new List<QFilter>();
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

public class QFilter : DatabaseObject
{
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