using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public static Enemy sharedInstance;
    public int damage;

    void Awake() {
        sharedInstance = this;
    }
}