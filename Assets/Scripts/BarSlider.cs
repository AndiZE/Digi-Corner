using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSlider : MonoBehaviour
{
    [SerializeField]
    private Sprite userAnswer;
    [SerializeField]
    private Sprite rightAnswer;

    private Slider slider;
    private Image selectedOutline;
    private Text infoText;
    [SerializeField]
    private GameObject userSelectionText;

    [SerializeField]
    private float fillDuration = 1f;

    private void Start()
    {
        slider = GetComponent<Slider>();
        selectedOutline = GetComponent<Image>();
        infoText = GetComponentInChildren<Text>();
    }

    private IEnumerator FillBar(float amount)
    {
        float timer = 0f;
        while(timer < fillDuration)
        {
            float curAmount = Mathf.Lerp(0f, amount, timer / fillDuration);
            slider.value = curAmount;

            timer += Time.deltaTime;
            yield return null;
        }

        slider.value = amount;
    }

    public void ActivateBar(float amount, bool isRightAnswer, bool isUserAnswer, string info)
    {
        if (isRightAnswer || isUserAnswer)
        {
            selectedOutline.enabled = true;
            selectedOutline.sprite = isRightAnswer ? rightAnswer : userAnswer;
        }
        else
        {
            selectedOutline.enabled = false;
        }
        StartCoroutine(FillBar(amount));
        infoText.text = info;
        userSelectionText.SetActive(isUserAnswer);
    }
}
