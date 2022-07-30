using UnityEngine;

public class Wing : MonoBehaviour
{
    public int Id { get; private set; }

    public float Threshold => Mathf.Lerp(Wings.Instance.WeightOffset, 1.0f, Mathf.Clamp((float)Id / Wings.Instance.AllWings.Length, 0.01f, 1.0f));

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Id = int.Parse(name.Substring(name.LastIndexOf("_") + 1));
    }

    public void UpdateWeight(float weight)
    {
        if (Id == 5)
            Debug.Log($"{Id}: Treshold({Threshold * 100:.#}%)");
        _spriteRenderer.enabled = weight >= Threshold;
    }
}