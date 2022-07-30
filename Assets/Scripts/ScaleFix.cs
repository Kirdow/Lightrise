using UnityEngine;

[ExecuteInEditMode]
public class ScaleFix : MonoBehaviour
{
    [Header("References")]
    public Transform Parent;

    [Header("General")]
    public Vector2 Scale = new Vector2(1.0f, 1.0f);

    private void Awake()
    {
        Rescale();
    }

    private void Rescale()
    {
        transform.localScale = new Vector3(1.0f / Parent.localScale.x, 1.0f / Parent.localScale.y, 1.0f / Parent.localScale.z);
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(Scale.x, Scale.y, 1.0f));
    }

    private void OnValidate()
    {
        Rescale();
    }
}