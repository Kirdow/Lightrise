using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    private Text _winText;

    private void Awake()
    {
        Instance = this;

        _winText = GetComponent<Text>();
    }

    public static void SetVisible(bool state)
    {
        Instance._winText.enabled = state;
    }

    public static WinText Instance { get; private set; }
}