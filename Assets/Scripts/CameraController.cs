using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject dino;
    public float speedMax;
    public static Camera instance;

    private Vector3 offsetMove = Vector3.zero;
    private Vector3 offsetMoveMax;
    private Vector3 offsetPlayer;
    private float speed;

    // Start is called before the first frame update

    void Start()
    {
        instance = GetComponent<Camera>();
        offsetPlayer = transform.position - dino.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = dino.transform.position + offsetPlayer + offsetMove;
        if (offsetMove != Vector3.zero)
        {
            MoveCamera();
        }
    }

    public void MoveCamera(Vector3 offset)
    {
        if (offsetMoveMax != offset)
        {
            speed = 0;
            offsetMoveMax = offset;
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        Vector2 sentido, move;
        sentido.x = offsetMoveMax.x!= 0 ? offsetMoveMax.x / Mathf.Abs(offsetMoveMax.x) : 0;
        sentido.y = offsetMoveMax.y != 0 ? offsetMoveMax.y / Mathf.Abs(offsetMoveMax.y) : 0;

        speed += speed < speedMax ? (speedMax / 20) * Time.deltaTime : 0;
        move = (speed) * sentido;
        offsetMove += offsetMove.Compare(offsetMoveMax, 1) ? Vector3.zero : new Vector3(move.x, move.y, 0);
    }
}