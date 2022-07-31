using UnityEngine;
using UnityEngine.UI;

public class HelpText : MonoBehaviour
{
    private Text _text;

    private string openText = "[F1] Toggle Help";
    private string fullText = @"
[→/D] Move Right
[←/A] Move Left
[Space] Jump
[R] Retry Level
[R] Retry Game (On Win)
[ESC] Quit

Your goal is to climb the clouds to find the lights,
and bring back the Light Module. But watch out,
you're a heavy robot. Don't lose your feathers.";

    private bool _active = false;

    private void Awake()
    {
        Instance = this;

        _text = GetComponent<Text>();
        SetActive(false);
    }

    private void Toggle()
    {
        SetActive(!_active);
    }

    private void SetActive(bool active)
    {
        _active = active;

        string text = openText;
        if (active) text += $"\n{fullText}";

        _text.text = text;
    }

    public static void ToggleHelpText()
    {
        Instance.Toggle();
    }

    public static HelpText Instance { get; private set; }
}