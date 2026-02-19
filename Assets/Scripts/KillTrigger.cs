using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (!PlayerController.sharedInstance.killEnemy) {
                PlayerController.sharedInstance.AudioKill();
                PlayerController.sharedInstance.Kill();
            }
        }
    }
}