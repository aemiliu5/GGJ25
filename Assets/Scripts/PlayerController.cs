using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public enum JumpType { Normal, Jail, Yarn }
    public enum PlayerState { Normal, Boosting, Birding }

    public float horizontalSpeed;
    public float jumpForce;
    public float initialJumpForce;
    [SerializeField] private float jailJumpForce = 12;
    [SerializeField] private float yarnJumpForce = 12;

    public float leftBound;
    public float rightBound;

    public GameObject ghostPrefab;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public static PlayerController instance;

    public bool IsInJail { get; set; }

    private Dictionary<JumpType, float> _jumpTypeToForce;

    [Header("Boost and Bird Mode Settings")]
    [SerializeField] private float boostDuration = 10f;
    [SerializeField] private float boostUpwardForce = 10f;
    [SerializeField] private float birdDuration = 3f;
    [SerializeField] private float birdUpwardForce = 7f;

    private PlayerState _currentState = PlayerState.Normal;
    private float _stateTimer = 0f;

    private void Start() {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        _jumpTypeToForce = new Dictionary<JumpType, float>() {
            { JumpType.Normal, jumpForce },
            { JumpType.Jail, jailJumpForce },
            { JumpType.Yarn, yarnJumpForce },
        };
    }

    private void Update() {
        if (IsInJail) { return; }

        switch (_currentState) {
            case PlayerState.Normal:
                HandleNormalMovement();
                break;
            case PlayerState.Boosting:
                HandleBoostingState();
                break;
            case PlayerState.Birding:
                HandleBirdingState();
                break;
        }
    }

    private void HandleNormalMovement() {
        Vector2 velocity = rb.linearVelocity;

        if (Input.GetKey(KeyCode.A) && transform.position.x > leftBound) {
            velocity.x = -horizontalSpeed;
            sr.flipX = true;
        } else if (Input.GetKey(KeyCode.D) && transform.position.x < rightBound) {
            velocity.x = horizontalSpeed;
            sr.flipX = false;
        } else {
            velocity.x = 0;
        }

        rb.linearVelocity = velocity;
    }

    private void HandleBoostingState() {
        if (_stateTimer > 0) {
            rb.linearVelocity = new Vector2(0, boostUpwardForce);
            _stateTimer -= Time.deltaTime;
        } else {
            ExitBoostMode();
        }
    }

    private void HandleBirdingState() {
        if (_stateTimer > 0) {
            rb.linearVelocity = new Vector2(0, birdUpwardForce);
            _stateTimer -= Time.deltaTime;
        } else {
            ExitBirdMode();
        }
    }

    public void Jump(JumpType jumpType = JumpType.Normal) {
        if (_currentState != PlayerState.Normal) return;

        anim.SetTrigger("col");
        rb.linearVelocityY = 0;
        rb.AddForce(new Vector2(0, _jumpTypeToForce[jumpType]), ForceMode2D.Impulse);
    }

    public void InitialJump() {
        Debug.Log("Initial Jump called!");
        anim.SetTrigger("Initial_Jump");
    }

    public void LaunchCatUpwards() {
        Debug.Log("Will launch cat upwards!");
        rb.AddForce(new Vector2(0, initialJumpForce), ForceMode2D.Impulse);
    }

    public void Simulated(bool simulated) {
        rb.simulated = simulated;
    }

    public void TriggerYarn() {
        anim.SetTrigger("yarn");
    }

    public void TriggerJail() {
        anim.SetTrigger("jailed");
    }

    // --- Boost Mode ---
    public void ActivateBoostMode() {
        if (_currentState != PlayerState.Normal) return;

        _currentState = PlayerState.Boosting;
        _stateTimer = boostDuration;
        anim.SetTrigger("boost");
    }

    private void ExitBoostMode() {
        _currentState = PlayerState.Normal;
        rb.linearVelocity = Vector2.zero; // Reset velocity
    }

    // --- Bird Mode ---
    public void ActivateBirdMode() {
        if (_currentState != PlayerState.Normal) return;

        _currentState = PlayerState.Birding;
        _stateTimer = birdDuration;
        anim.SetTrigger("birding");
    }

    private void ExitBirdMode() {
        _currentState = PlayerState.Normal;
        rb.linearVelocity = Vector2.zero; // Reset velocity
    }
}
