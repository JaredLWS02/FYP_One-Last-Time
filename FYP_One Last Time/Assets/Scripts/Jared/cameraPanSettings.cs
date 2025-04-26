using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "NewCameraPanSettings", menuName = "Camera/Camera Pan Settings")]
public class CameraPanSettings : ScriptableObject
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;
    public bool enableZoom = false;
    public float zoomFOV = 50f;
    public float zoomDuration = 0.5f;


    public CinemachineVirtualCamera cameraOnLeft;
    public CinemachineVirtualCamera cameraOnRight;

    public cameraPan.PanDirection panDirection;
    public float panDistance = 3f;
    public float panTime = 0.35f;
}
