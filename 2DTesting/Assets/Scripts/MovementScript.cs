using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementScript : MonoBehaviour {

    Rigidbody2D body;

    float horizontalPress, verticalPress;

    float horizontalSpeed = 0, verticalSpeed = 0;

    float diagonalLimit = 0.707f;
    public float speed = 20.0f;
    public float acceleration = 5.0f;
    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        int horizontalDirection = (int)((horizontalSpeed / Math.Abs(horizontalSpeed)));
        int verticalDirection = (int)((verticalSpeed / Math.Abs(verticalSpeed)));

        if (horizontalPress != 0 && verticalPress != 0) {
            horizontalPress *= diagonalLimit;
            verticalPress *= diagonalLimit;
        } else if (horizontalPress == 0 & verticalPress == 0)
        {
            horizontalSpeed = roundUp(horizontalSpeed, 1);
            verticalSpeed = roundUp(verticalSpeed, 1);
        }

        if ((horizontalPress == 0) && Math.Abs(horizontalSpeed) > 0)
        {
            horizontalSpeed -= horizontalDirection * acceleration;
        }
        else if ((horizontalPress == -horizontalDirection) && Math.Abs(horizontalSpeed) > 0)
        {
            horizontalSpeed -= horizontalDirection * 2 * acceleration;
        }
        else if (Math.Abs(horizontalSpeed) < speed)
        {
            horizontalSpeed += horizontalPress * acceleration;
        }

        if (verticalPress == 0 && Math.Abs(verticalSpeed) > 0)
        {
            verticalSpeed -= verticalDirection * acceleration;
        }
        else if ((verticalPress == -verticalDirection) && Math.Abs(verticalSpeed) > 0)
        {
            verticalSpeed -= verticalDirection * 2 * acceleration;
        }
        else if (Math.Abs(verticalSpeed) < speed)
        {
            verticalSpeed += verticalPress * acceleration;
        }
        body.velocity = new Vector2(horizontalSpeed, verticalSpeed);
    }

    // Update is called once per frame
    void Update() {
        horizontalPress = Input.GetAxisRaw("Horizontal");
        verticalPress = Input.GetAxisRaw("Vertical");
    }

    float roundUp(float num, int factor)
    {
        if(num < 0)
        {
            num = -num;
            return -(num + factor - 1 - (num + factor - 1) % factor);
        }
        return num + factor - 1 - (num + factor - 1) % factor;
    }
}
