using UnityEngine;

public class JumpProgress : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        Instance = this;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetProgress(0.0f);
    }

    public static void SetProgress(float progress)
    {
        Instance._spriteRenderer.enabled = progress > 0.0f;
        var v = Instance.transform.localScale;
        Instance.transform.localScale = new Vector3(progress, v.y, v.z);
    }


    public static JumpProgress Instance { get; private set; }
}
