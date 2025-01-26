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

    public bool ghostSpawned;
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
    [SerializeField] private float sensitivity = 10f;

    public PlayerState currentState = PlayerState.Normal;
    private float _stateTimer = 0f;

    private void Start()
    {
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

        switch (currentState) {
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

        #if UNITY_ANDROID
        var intendedVelocityX = Input.acceleration.x * sensitivity;
        var newPositionX = transform.position.x + intendedVelocityX * Time.deltaTime;
        newPositionX = Mathf.Clamp(newPositionX, leftBound, rightBound);
        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
        #endif

        rb.linearVelocity = velocity;
    }

    private void HandleBoostingState() {
        if (_stateTimer > 0) 
        {
            rb.linearVelocity = new Vector2(0, boostUpwardForce);
            _stateTimer -= Time.deltaTime;
            ScoreManager.instance.AddScore(3);
        } 
        else 
        {
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
        if (currentState != PlayerState.Normal) return;

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
        AudioManager.instance.PlaySoundOnce(AudioManager.instance.excited);
        anim.SetTrigger("yarn");
    }

    public void TriggerJail() {
        AudioManager.instance.PlaySoundOnce(AudioManager.instance.jailBubble);
        anim.SetTrigger("jailed");
    }

    // --- Boost Mode ---
    public void ActivateBoostMode()
    {
        if (currentState != PlayerState.Normal) return;
        currentState = PlayerState.Boosting;
        GetComponent<BoxCollider2D>().enabled = false;
        MusicManager.instance.metal.volume = 1f;
        MusicManager.instance.floriko.volume = 0.8f;
        AudioManager.instance.PlaySoundOnce(AudioManager.instance.purr);
        _stateTimer = boostDuration;
        anim.SetTrigger("boost");
    }

    private void ExitBoostMode() {
        currentState = PlayerState.Normal;
        GetComponent<BoxCollider2D>().enabled = true;
        MusicManager.instance.metal.volume = 0f;
        MusicManager.instance.floriko.volume = 1f;
        rb.linearVelocity = Vector2.zero; // Reset velocity
    }

    // --- Bird Mode ---
    public void ActivateBirdMode() {
        if (currentState != PlayerState.Normal) return;

        currentState = PlayerState.Birding;
        _stateTimer = birdDuration;
        anim.SetTrigger("birding");
    }

    private void ExitBirdMode() {
        currentState = PlayerState.Normal;
        rb.linearVelocity = Vector2.zero; // Reset velocity
    }
}
