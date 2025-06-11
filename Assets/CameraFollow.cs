using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // camera
    public Camera characterCamera;
    
    void Update()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.z = characterCamera.transform.position.z;
        characterCamera.transform.position = cameraPosition;
    }
}
