using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    Actor _hasActor;
    float _actorSpeed;
	// Use this for initialization
	void Start () {
        _hasActor = GetComponent<Actor>();
        _actorSpeed = 8.0f;
    }
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        TrackCamera();

        Vector3 dir = new Vector3(x, 0, z);
        if (Vector3.Distance(dir, Vector3.zero) > 0.0f)
        {
            Vector3 cameraDir = transform.position - Camera.main.transform.position;
            float angle = Mathf.Atan2(cameraDir.z, cameraDir.x) - Mathf.Atan2(x, z);
            x = Mathf.Cos(angle);
            z = Mathf.Sin(angle);
            dir = new Vector3(x, 0, z).normalized;
            _hasActor.Movement(dir, _actorSpeed);
        }
	}

    Vector3 amountCameraAngle;
    Vector3 clickPos;
    void TrackCamera()
    {
        // 入力部、後で分離するかも
        if(Input.GetMouseButtonDown(1))
        {
            clickPos = Input.mousePosition;
            return;
        }
        if(Input.GetMouseButton(1))
        {
            float sensitivity = 0.2f;
            Vector3 dir = (Input.mousePosition - clickPos) * sensitivity;
            dir.y *= -1;
            amountCameraAngle += dir;
            clickPos = Input.mousePosition;
            amountCameraAngle.y = Mathf.Clamp(amountCameraAngle.y, 0.0f, 90.0f);
        }
        // 更新
        Transform camTrans = Camera.main.transform;
        camTrans.position = _hasActor.transform.position + Vector3.up * 4;
        camTrans.rotation = Quaternion.Euler(amountCameraAngle.y, amountCameraAngle.x, 0.0f);
        camTrans.Translate(Vector3.back * 10);
    }
}
