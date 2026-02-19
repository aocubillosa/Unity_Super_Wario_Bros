using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController sharedInstance;
    public float jumpForce = 60f, runningSpeed = 5f;
    public LayerMask groundLayer;
    public Animator animator;
    public AudioClip audioJump, audioKill;
    public bool killEnemy, isAlive;
    public const int MAX_HEALTH = 100, MAX_MANA = 50, SUPERJUMP_COST = 25;
    public const float SUPERJUMP_FORCE = 1.25f;
    private Rigidbody2D isRigidbody;
    private Vector3 startPosition;
    private CapsuleCollider2D isCollider;
    private int healthPoints, manaPoints;

    void Awake() {
        sharedInstance = this;
        isRigidbody = GetComponent<Rigidbody2D>();
        isCollider = GetComponent<CapsuleCollider2D>();
        startPosition = this.transform.position;
    }

    public void StartGame() {
        animator.SetBool("isAlive", true);
        animator.SetBool("isGrounded", true);
        this.transform.position = startPosition;
        isCollider.enabled = true;
        this.healthPoints = MAX_HEALTH;
        this.manaPoints = MAX_MANA;
        killEnemy = false;
        isAlive = true;
    }

    void Update() {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            if (Input.GetButtonDown("Jump")) {
                Jump(false);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Jump(true);
            }
            else if (Input.GetButtonDown("Jump") && Input.GetMouseButtonDown(1)) {
                Jump(false);
            }
            animator.SetBool("isGrounded", IsTouchingTheGround());
        }
    }

    void FixedUpdate() {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame && this.healthPoints > 0) {
            if (isRigidbody.velocity.x < runningSpeed) {
                isRigidbody.velocity = new Vector2(runningSpeed, isRigidbody.velocity.y);
            }
        }
        else {
            isRigidbody.velocity = new Vector2(0, isRigidbody.velocity.y);
        }
    }

    void Jump(bool isSuperJump) {
        if (IsTouchingTheGround()) {
            GetComponent<AudioSource>().PlayOneShot(this.audioJump);
            if (isSuperJump && this.manaPoints >= SUPERJUMP_COST) {
                this.manaPoints -= SUPERJUMP_COST;
                isRigidbody.AddForce(Vector2.up * jumpForce * SUPERJUMP_FORCE, ForceMode2D.Impulse);
            }
            else {
                isRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    bool IsTouchingTheGround() {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.0f, groundLayer)) {
            return true;
        }
        else {
            return false;
        }
    }

    public void Kill() {
        isAlive = false;
        this.healthPoints = 0;
        this.animator.SetBool("isAlive", false);
        isCollider.enabled = false;
        StartCoroutine("ActivateCollider");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            this.healthPoints -= Enemy.sharedInstance.damage;
            if (GameManager.sharedInstance.currentGameState == GameState.inGame && this.healthPoints <= 0) {
                isRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                killEnemy = true;
                AudioKill();
                Kill();
            }
        }
    } 

    IEnumerator ActivateCollider() {
        yield return new WaitForSeconds(1.2f);
        isCollider.enabled = true;
        StartCoroutine("GameOver");
    }

    IEnumerator GameOver() {
        yield return new WaitForSeconds(3.0f);
        GameManager.sharedInstance.GameOver();
        float currentMaxScore = PlayerPrefs.GetFloat("maxScore", 0);
        if (currentMaxScore < this.GetDistance())
        {
            PlayerPrefs.SetFloat("maxScore", this.GetDistance());
        }
        this.transform.position = new Vector2(0.2f, 0);
    }

    public float GetDistance() {
        float travelledDistance = Vector2.Distance(new Vector2(startPosition.x, 0), new Vector2(this.transform.position.x, 0));
        return travelledDistance;
    }

    public void CollectHealth(int points) {
        this.healthPoints += points;
        if (this.healthPoints > MAX_HEALTH) {
            this.healthPoints = MAX_HEALTH;
        }
    }

    public void CollectMana(int points) {
        this.manaPoints += points;
        if (this.manaPoints > MAX_MANA) {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth() {
        return this.healthPoints;
    }

    public int GetMana() {
        return this.manaPoints;
    }

    public void AudioKill() {
        GameManager.sharedInstance.StopSoundtrack();
        GetComponent<AudioSource>().PlayOneShot(this.audioKill);
    }
}