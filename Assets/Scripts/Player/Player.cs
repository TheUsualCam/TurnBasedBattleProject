using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float moveX;
    private float moveZ;
    private bool isPressing = false;
    private bool isInBattle = false;

    public float playerSpeed;

    // Update is called once per frame
    void Update()
    {
        if(!isInBattle)
        {
            UpdatePlayerMovement();
        }
    }

    void UpdatePlayerMovement()
    {
        if (isPressing)
        {
            if (moveX > 0)
            {
                if (playerSpeed < 0)
                {
                    playerSpeed *= -1;
                }

                transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
            }
            else if (moveX < 0)
            {
                if (playerSpeed > 0)
                {
                    playerSpeed *= -1;
                }
         
                transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
            }

            if (moveZ > 0)
            {
                if (playerSpeed < 0)
                {
                    playerSpeed *= -1;
                }

                transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
            }
            else if (moveZ < 0)
            {
                if (playerSpeed > 0)
                {
                    playerSpeed *= -1;
                }

                transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
            }
        }

        isPressing = !isPressing;
    }

    void OnMoveForward(InputValue value)
    {
        if(!isInBattle)
        {
            isPressing = !isPressing;
            moveZ = value.Get<float>();
        }
    }

    void OnMoveSideways(InputValue value)
    {
        if(!isInBattle)
        {
            isPressing = !isPressing;
            moveX = value.Get<float>();
        }
    }

    public void DisableMovement()
    {
        isInBattle = true;
        transform.position = transform.position;
    }

    public void ResetMovement()
    {
        isInBattle = false;
        transform.Translate(Vector3.zero);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
