using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;

public abstract class DatabaseObject
{
    public abstract Dictionary<string, object> ToDict();
}