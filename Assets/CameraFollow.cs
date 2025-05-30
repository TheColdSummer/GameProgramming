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
        // ÿһ֡�������������λ�õ���ǰ��ɫ��λ�ã�����Ľ�ɫ���ǵ�ǰ����
        Vector3 cameraPosition = transform.position;
        cameraPosition.z = characterCamera.transform.position.z;
        characterCamera.transform.position = cameraPosition;
    }
}
