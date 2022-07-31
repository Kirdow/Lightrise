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
    public float jumpTimeMod = 1.0f;
    [Range(0f, 1f)]
    public float[] jumpLevels;

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

    private float _lastJump = 0.0f;
    private float _jumpStart = 0.0f;
    private int _jumpsUsed = 0;
    private int _prevGauge = -1;

    private LevelData _levelData;

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
        Reset(null);
    }

    public void Reset(LevelData levelData)
    {
        _jumpsUsed = 0;
        _levelData = levelData;
        Velocity = Vector2.zero;
        Wings.Instance.UpdateWeight(1.0f);
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

    
    private float GetJumpForce()
    {
        int jumpForce = -1;
        bool isJump = Input.GetAxis("Jump") > 0.0f;
        if (!isJump)
        {
            if (_jumpStart > 0.0f)
                jumpForce = Mathf.CeilToInt((Time.realtimeSinceStartup - _jumpStart) * jumpTimeMod);
            _jumpStart = -1.0f;
        }
        else
        {
            if (_jumpStart <= 0.0f)
            {
                _prevGauge = -1;
                _jumpStart = Time.realtimeSinceStartup;
            }
            else
            {
                int jumpForceDelta = Mathf.CeilToInt((Time.realtimeSinceStartup - _jumpStart) * jumpTimeMod);
                if (jumpForceDelta >= jumpLevels.Length) jumpForceDelta = jumpLevels.Length - 1;

                if (jumpForceDelta != _prevGauge)
                {
                    var audio = GameAssets.I.jumpGaugeAudio;
                    int audioClipId = jumpForceDelta - 1;
                    if (audioClipId >= audio.Length) audioClipId = audio.Length - 1;

                    if (audioClipId >= 0)
                        SoundManager.PlaySound(audio[audioClipId]);

                    JumpProgress.SetProgress((float)jumpForceDelta / jumpLevels.Length);
                }
                _prevGauge = jumpForceDelta;
            }
        }

        if (jumpForce > 0)
        {
            JumpProgress.SetProgress(0.0f);
            _jumpsUsed += jumpForce;

            if (jumpForce >= jumpLevels.Length) jumpForce = jumpLevels.Length - 1;
            return jumpLevels[jumpForce - 1];
        }

        return -1.0f;

    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        Velocity = new Vector2(moveX * maxMoveSpeed, Velocity.y);

        float jumpForce = GetJumpForce();

        float jump = Input.GetAxis("Jump");
        if (_groundCheck.isOnGround && jumpForce > 0.0f)
        {
            bool canJump = true;

            if ((_levelData?.Jumps ?? 0) != 0)
            {
                float weight = 1.0f - ((float)_jumpsUsed / _levelData.Jumps);
                Wings.Instance.UpdateWeight(weight);

                Debug.Log($"Weight({weight * 100:.#}%)");

                if (weight <= 0.0f)
                {
                    LevelHandler.Instance.ResetWithFade();
                    canJump = false;
                }
            }

            if (canJump)
            {
                Velocity = Vector2.right * Velocity.x;
                _rigidbody.AddForce(Vector2.up * maxJumpHeight * 2.0f * jumpForce, ForceMode2D.Impulse);
                SoundManager.PlaySound(GameAssets.I.jumpAudio);
            }
        }

        _lastJump = jump;
    }

    private Vector2 Velocity { get { return _rigidbody.velocity; } set { _rigidbody.velocity = value; } }

    public static PlayerController Instance { get; private set; }
}
