using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

public class cameraPan : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    private Collider _coll;

    private void Start()
    {
        _coll = GetComponent<Collider>();
        if (customInspectorObjects == null)
        {
            Debug.LogError("CustomInspectorObjects is not assigned to cameraPan.");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (customInspectorObjects.panCameraOnContact)
            {
                Debug.Log("detected");
                CameraManager.Current.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (customInspectorObjects.panCameraOnContact)
            {
                CameraManager.Current.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}


[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;
    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}

[CustomEditor(typeof(cameraPan))]
public class MyScriptEditor : Editor
{
    cameraPan camPan;

    private void OnEnable()
    {
        camPan = (cameraPan)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (camPan.customInspectorObjects.swapCameras)
        {
            camPan.customInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera on Left", camPan.customInspectorObjects.cameraOnLeft,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            camPan.customInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera on Right", camPan.customInspectorObjects.cameraOnRight,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

        }

        if (camPan.customInspectorObjects.panCameraOnContact)
        {
            camPan.customInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction",
                camPan.customInspectorObjects.panDirection);

            camPan.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", camPan.customInspectorObjects.panDistance);
            camPan.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time", camPan.customInspectorObjects.panTime);
            
            if(GUI.changed)
            {
                EditorUtility.SetDirty(camPan);
            }
        }
    }
}