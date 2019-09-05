using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonHighlighter : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float deactivateStrengh = 0.6f;
    private Color originalColor;
    private Color deactivateColor;
    private Image image;

    public void Init()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        deactivateColor = image.color;
        deactivateColor.a = deactivateStrengh;
    }

    public void SelectButton(bool currentSelected)
    {
        image.color = currentSelected ? originalColor : deactivateColor;
    }

}
