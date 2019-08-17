using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonBehaviour<CameraController>
{
    public Transform head, body;
    public Transform mainCamera, leftCamera, rightCamera;
    public Canvas bodyCanvas, headCanvas;
    public float LerpFactor = 0.1f;

    public bool splitted = false;
    public bool headIsLeft = false;

    private new Camera camera;
    private Camera leftCameraComp, rightCameraComp;
    public Camera Camera => camera;
    public Camera HeadCamera => headIsLeft ? leftCameraComp : rightCameraComp;
    public Camera BodyCamera => headIsLeft ? rightCameraComp : leftCameraComp;

	private void Start()
    {
        camera = mainCamera.GetComponent<Camera>();
        leftCameraComp = leftCamera.GetComponent<Camera>();
        rightCameraComp = rightCamera.GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 headPos = head.position;
        Vector2 bodyPos = body.position;
        mainCamera.position = new Vector3((headPos.x + bodyPos.x) / 2, (headPos.y + bodyPos.y) / 2, -10f);
        headIsLeft = headPos.x < bodyPos.x;

        Vector2 headScreenPos = camera.WorldToScreenPoint(headPos);
        Vector2 bodyScreenPos = camera.WorldToScreenPoint(bodyPos);
        Vector2 diffScreenPos = headScreenPos - bodyScreenPos;

        Vector3 leftPrefPos, rightPrefPos;
        
        if (Mathf.Abs(diffScreenPos.x) >= Screen.width / 2 || Mathf.Abs(diffScreenPos.y) >= Screen.height / 2)
        {
            splitted = true;
            if (headIsLeft)
            {
                leftPrefPos = headPos;
                rightPrefPos = bodyPos;
                headCanvas.worldCamera = leftCameraComp;
                bodyCanvas.worldCamera = rightCameraComp;
            }
            else
            {
                leftPrefPos = bodyPos;
                rightPrefPos = headPos;
                bodyCanvas.worldCamera = leftCameraComp;
                headCanvas.worldCamera = rightCameraComp;
            }
        }
        else
        {
            splitted = false;
            leftPrefPos = camera.ScreenToWorldPoint(new Vector2(Screen.width / 4, Screen.height / 2));
            rightPrefPos = camera.ScreenToWorldPoint(new Vector2(Screen.width * 3 / 4, Screen.height / 2));
        }
        leftPrefPos = new Vector3(leftPrefPos.x, leftPrefPos.y, -10f);
        rightPrefPos = new Vector3(rightPrefPos.x, rightPrefPos.y, -10f);

        leftCamera.position = Vector3.Lerp(leftCamera.position, leftPrefPos, LerpFactor);
        rightCamera.position = Vector3.Lerp(rightCamera.position, rightPrefPos, LerpFactor);
    }
}
