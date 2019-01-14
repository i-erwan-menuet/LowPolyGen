﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;

    public float gravity = -10f;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    public float smoothMoveTime = 0.2f;

    float smoothRotationVelocity;
    public float smoothRotationTime = 0.2f;

    Rigidbody rigidbody;
    Transform cameraT;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        cameraT = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 inputDir = input.normalized;

        if (inputDir != Vector3.zero)
        {
            float targetLocalUpRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetLocalUpRotation, ref smoothRotationVelocity, smoothRotationTime);
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = (running ? runSpeed : walkSpeed) * inputDir.magnitude;
        Vector3 targetMoveAmount = inputDir * targetSpeed;

        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, smoothMoveTime);
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(Vector3.up * gravity);
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}