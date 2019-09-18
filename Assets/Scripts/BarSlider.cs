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
    [SerializeField]
    private GameObject richtigImage;

    private void Awake()
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

    public void ActivateBar(float amount, bool isRightAnswer, bool isUserAnswer, string info, bool categorySwitched)
    {
        richtigImage?.SetActive(false);
        if (isRightAnswer || isUserAnswer)
        {
            selectedOutline.enabled = true;
            selectedOutline.sprite = isRightAnswer ? rightAnswer : userAnswer;
            if (isRightAnswer && isUserAnswer)
            {
                selectedOutline.sprite = rightAnswer;
                richtigImage.SetActive(true);
                if (categorySwitched)
                {
                    richtigImage.SetActive(false);
                }
            }
        }
        else if (selectedOutline != null)
        {
            selectedOutline.enabled = false;
        }
        StartCoroutine(FillBar(amount));
        infoText.text = info;
        userSelectionText.SetActive(isUserAnswer);
    }

    public void SimpleActivateBar(float amount)
    {
        StartCoroutine(FillBar(amount));
    }
}
