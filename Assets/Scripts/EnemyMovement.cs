using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float runningSpeed = 2.0f;
    public bool turnAround;
    private Rigidbody2D isRigidbody;
    public bool movingForward;

    void Awake(){
        isRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        float currentRunningSpeed = runningSpeed;
        if (turnAround) {
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180.0f, 0);
        }
        else {
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            isRigidbody.velocity = new Vector2(currentRunningSpeed, isRigidbody.velocity.y);
        }
        else {
            isRigidbody.velocity = new Vector2(0, isRigidbody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag != "Wall") {
            return;
        }
        if (movingForward) {
            turnAround = true;
        }
        else {
            turnAround = false;
        }
        movingForward = !movingForward;
    }
}