using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ButtonHighlighter : MonoBehaviour
{
    [Header("Button")]
    [SerializeField]
    private Color buttonActiveColor;
    [SerializeField]
    private Color buttonPassiveColor;

    [Header("Font")]
    [SerializeField]
    private Color fontColorActive;
    [SerializeField]
    private Color fontColorPassive;

    private Image image;
    private Text text;

    [SerializeField]
    private bool useOutline = true;
    private Outline outline;
    public void Init()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();

        image.color = buttonActiveColor;
        text.color = fontColorActive;

        outline = GetComponent<Outline>();
    }

    public void SelectButton(bool currentSelected)
    {
        image.color = currentSelected ? buttonActiveColor : buttonPassiveColor;
        text.color = currentSelected ? fontColorActive : fontColorPassive;
        if (useOutline && outline != null)
            outline.effectDistance = currentSelected ? new Vector2(2f,0f) : new Vector2(2,2);
            //outline.enabled = currentSelected ? false : true;
    }

}
