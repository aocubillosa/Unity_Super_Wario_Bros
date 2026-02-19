using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType {
    coin,
    diamond,
    health,
    mana
}

public class Collectable : MonoBehaviour {

    public CollectableType type;
    public bool isCollected = false;
    public int value = 0;
    public AudioClip audioCollect;

    void Show() {
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
        this.isCollected = false;
    }

    void Hide() {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CircleCollider2D>().enabled = false;
    }

    void Collect() {
        this.isCollected = true;
        Hide();
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && this.audioCollect != null) {
            audio.PlayOneShot(this.audioCollect);
        }
        switch (this.type) {
            case CollectableType.coin:
                GameManager.sharedInstance.CollectCoin(value);
                break;
            case CollectableType.diamond:
                GameManager.sharedInstance.CollectDiamond(value);
                break;
            case CollectableType.health:
                PlayerController.sharedInstance.CollectHealth(value);
                break;
            case CollectableType.mana:
                PlayerController.sharedInstance.CollectMana(value);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Collect();
        }
    }
}