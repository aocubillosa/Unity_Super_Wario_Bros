using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public Vector3 offset = new Vector3(0.1f, 0.0f, -10.0f), velocity = Vector3.zero;
    public float dampTine = 0.3f;

    void Awake() {
        Application.targetFrameRate = 60;
    }

    void Update() {
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(offset.x, offset.y, point.z));
        Vector3 destination = point + delta;
        destination = new Vector3(target.position.x, offset.y, offset.z);
        this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampTine);
    }

    public void ResetCameraPosition() {
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(offset.x, offset.y, point.z));
        Vector3 destination = point + delta;
        destination = new Vector3(target.position.x, offset.y, offset.z);
        this.transform.position = destination;
    }
}