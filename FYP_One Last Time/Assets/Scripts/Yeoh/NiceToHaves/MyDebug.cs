using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDebug : MonoBehaviour
{
    public void Log(string msg) => Debug.Log(msg);
    public void LogWarning(string msg) => Debug.LogWarning(msg);
    public void LogError(string msg) => Debug.LogError(msg);
    public void Print(string msg) => print(msg);
}
