using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [Min(0.001f)]
    public float maxMoveSpeed = 6.0f;
    public float maxJumpHeight = 6.0f;
    public float weightPerJump = 0.07f;

    [Header("References")]
    public BoxCollider2D groundCheck;

    [Header("Player Sprites")]
    public Sprite idleSprite;
    public Sprite flyingSprite;
    public Sprite jumpingSprite;

    private Rigidbody2D _rigidbody;
    private Tilemap _map;
    private GroundCheck _groundCheck;
    private SpriteRenderer _spriteRenderer;

    private float _currentWeight;

    public float ActualJumpHeight => Mathf.Clamp(maxJumpHeight - _currentWeight, 0, maxJumpHeight);

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _rigidbody = GetComponent<Rigidbody2D>();
        _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentWeight = 0.0f;
    }

    private void Update()
    {
        if (_groundCheck.isOnGround)
        {
            if (Mathf.Abs(Velocity.x) <= 0.01f)
                _spriteRenderer.sprite = jumpingSprite;
            else
                _spriteRenderer.sprite = idleSprite;
        }
        else
            _spriteRenderer.sprite = flyingSprite;

    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        Velocity = new Vector2(moveX * maxMoveSpeed, Velocity.y);

        if (_groundCheck.isOnGround && Input.GetAxis("Jump") > 0.0f)
        {
            Velocity = Vector2.right * Velocity.x;
            _rigidbody.AddForce(Vector2.up * maxJumpHeight * 2.0f, ForceMode2D.Impulse);

            _currentWeight = Mathf.Clamp(_currentWeight + weightPerJump, 0.0f, maxJumpHeight);
            float weight = ActualJumpHeight / maxJumpHeight;
            Wings.Instance.UpdateWeight(weight);

            Debug.Log($"Weight({weight * 100:.#}%)");
        }
    }

    private Vector2 Velocity { get { return _rigidbody.velocity; } set { _rigidbody.velocity = value; } }

    public static PlayerController Instance { get; private set; }
}
