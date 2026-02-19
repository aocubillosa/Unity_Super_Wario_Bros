using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveZone : MonoBehaviour {

    float timeDestruction = 0.0f;

    void Update() {
        timeDestruction += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (timeDestruction > 3.0f)
            {
                LevelGenerator.sharedInstance.AddLevelBlock();
                LevelGenerator.sharedInstance.RemoveOldLevelBlock();
                timeDestruction = 0.0f;
            }
        }
    }
}