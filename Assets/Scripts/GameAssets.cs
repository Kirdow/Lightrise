using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    public static GameAssets I
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    [Header("Sounds")]
    public AudioClip jumpAudio;
    public AudioClip deathAudio;
    public AudioClip winAudio;
    public AudioClip[] jumpGaugeAudio;

    [Header("Item Prefabs")]
    public GameObject lightItem;
    public GameObject moduleItem;

}
