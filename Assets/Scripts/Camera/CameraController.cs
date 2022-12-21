using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] Transform player;
    [SerializeField] Camera _camera;
    public void Camera_Moving()
    {
        Vector3 _cameraPosition = player.transform.position + offset;
        _camera.transform.position = _cameraPosition;
        Vector3 postion = new Vector3(Mathf.Clamp(_camera.transform.position.x, -45, 45), _camera.transform.position.y, Mathf.Clamp(_camera.transform.position.z, -50, 25));
        _camera.transform.position = postion;
    }
}
