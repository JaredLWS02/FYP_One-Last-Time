using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Current;

    void Awake()
    {
        if(!Current)
        {
            Current=this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        Invoke(nameof(UnlockFPS), .1f);
    }

    // ============================================================================

    void UnlockFPS()
    {
        //Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 0;
    }

    // ============================================================================

    public bool IsWindows()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor;
    }

    // ============================================================================

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent)
    {
        GameObject spawned = Instantiate(prefab, pos, rot);

        if(parent) spawned.transform.parent = parent;

        EventManager.Current.OnSpawned(spawned);

        return spawned;
    }

}