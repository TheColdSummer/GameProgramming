using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // camera
    public Camera characterCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 每一帧都更新摄像机的位置到当前角色的位置，这里的角色就是当前物体
        Vector3 cameraPosition = transform.position;
        cameraPosition.z = characterCamera.transform.position.z;
        characterCamera.transform.position = cameraPosition;
    }
}
