using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilites;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public float speed = 10;
    float _speed = 0;
    public float boostTime = 5;
    [SerializeField] MovingController movingController;
    [SerializeField] CameraController cameraController;
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
        movingController.Moving(speed, transform);

        if (Input.touchCount > 0)
        {

            Touch parmak = Input.GetTouch(0);
            _speed = _speed + parmak.deltaPosition.x * Time.deltaTime * 10;
            movingController.Roration(_speed, gameObject);
        }
        if (Input.GetKey(KeyCode.A))
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
        }

    }
    IEnumerator SpeedBoost()
    {
        speed *= 1.2f;
        yield return new WaitForSeconds(boostTime);
        speed *= 0.83333333333333333333f;
    }
}
