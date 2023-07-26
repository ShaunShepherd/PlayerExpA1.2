using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInHolder : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    void Update()
    {
        transform.position = cameraTransform.position;
    }
}
