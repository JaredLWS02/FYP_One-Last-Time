using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDebug : MonoBehaviour
{
    public string prefix;
    public string suffix;

    public void Log(string msg) => Debug.Log($"{prefix}{msg}{suffix}");
    public void LogWarning(string msg) => Debug.LogWarning($"{prefix}{msg}{suffix}");
    public void LogError(string msg) => Debug.LogError($"{prefix}{msg}{suffix}");
    public void Print(string msg) => print($"{prefix}{msg}{suffix}");
}
