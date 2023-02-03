using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilites;
using System;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public float speed = 10;
    float _speed = 0;
    public float boostTime = 5;
    [SerializeField] MovingController movingController;
    [SerializeField] CameraController cameraController;
    

    public DynamicJoystick joystick;
    private void Awake()
    {
        Instance = this;
    }
    private void LateUpdate() => cameraController.Camera_Moving();
    private void Start()
    {
        StartCoroutine(SpeedBoost());
    }
    // Update is called once per frame
    void Update()
    {
        InputController();
    }
    void InputController()
    {
        if (Input.touchCount > 0)
        {
            speed = 3;
            Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            transform.Translate(direction * (speed * Time.deltaTime),Space.World);
            transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(direction), transform.rotation, Time.deltaTime * speed);
            

        }
        else
        {
            speed = 0;
        }
        /*if (Input.GetKey(KeyCode.A))
        {

            _speed -= 3f;
            movingController.Roration(_speed, gameObject);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _speed -= 0;
            movingController.Roration(_speed, gameObject);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _speed += 3f;
            movingController.Roration(_speed, gameObject);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _speed -= 0;
            movingController.Roration(_speed, gameObject);
        }*/

    }
    IEnumerator SpeedBoost()
    {
        speed *= 1.2f;
        yield return new WaitForSeconds(boostTime);
        speed *= 0.83333333333333333333f;
    }
}
