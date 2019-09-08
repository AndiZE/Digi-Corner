using UnityEngine;
using UnityEngine.UI;

public class ValueUpdater : MonoBehaviour
{
    [SerializeField]
    private Text valueText;
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnValueChanged);
        slider.value = 0f;
    }

    private void OnValueChanged(float value)
    {
        valueText.text = value.ToString("F0") + "%";
    }

}
