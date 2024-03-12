using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    private int movementType;

    [SerializeField]
    private bool movementDirectionXPositive = true;

    [SerializeField]
    private bool movementDirectionYPositive = true;

    [SerializeField]
    private int movementDistanceX = 3;

    [SerializeField]
    private int movementDistanceY = 3;

    private Vector3 startPos;

    private Vector3 finishPos;

    private float trackPercent = 0f;

    private int direction = 1;

    private float x;

    private float y;

    void Start()
    {
        startPos = transform.position;

        if (movementDirectionXPositive)
        {
            finishPos.x = startPos.x + movementDistanceX;
        }
        else
        {
            finishPos.x = startPos.x - movementDistanceX;
        }

        if (movementDirectionYPositive)
        {
            finishPos.y = startPos.y + movementDistanceY;
        } else
        {
            finishPos.y = startPos.y - movementDistanceY;
        }
        x = startPos.x;
        y = startPos.y;
    }

    void Update()
    {
        trackPercent += direction * speed * Time.deltaTime;

        if (movementType == 0)
        {
            x = (finishPos.x - startPos.x) * trackPercent + startPos.x;
        } 
        else if (movementType == 1)
        {
            y = (finishPos.y - startPos.y) * trackPercent + startPos.y;
        } 
        else if (movementType == 2)
        {
            x = (finishPos.x - startPos.x) * trackPercent + startPos.x;
            y = (finishPos.y - startPos.y) * trackPercent + startPos.y;
        }
        transform.position = new Vector3(x, y, startPos.z);

        if ((direction == 1 && trackPercent >= 1f) || (direction == -1 && trackPercent <= 0f))
        {
            direction *= -1;
        }
    }
}
