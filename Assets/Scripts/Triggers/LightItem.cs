using UnityEngine;
using UnityEngine.Events;

public class LightItem : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent EnterEvent;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnterEvent.Invoke();
            Destroy(this);
        }
    }

    public static void SummonAt(int x, int y, UnityAction action)
    {
        LightItem item = Instantiate(GameAssets.I.lightItem, new Vector3(x + 0.5f, y + 0.5f, -0.1f), Quaternion.identity).GetComponent<LightItem>();
        item.EnterEvent.AddListener(action);
    }
}