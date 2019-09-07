using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
using UnityEngine.UI;
using System;

public class SplineMover : MonoBehaviour
{
    [SerializeField]
    private Spline spline;
    [SerializeField]
    private Vector3 offset;

    public float currentPosition { get; private set; }

    [SerializeField]
    private Gradient gradient;

    private RectTransform rect;
    private Image barImage;
    private Image iconImage;

    [SerializeField]
    private float speed = 0.5f;
    private Coroutine coroutine;
    void Start()
    {
        rect = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();
        iconImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void Rotate(float end ,Sprite image = null)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Rotate(currentPosition, end, speed, image));
        }
    }

    private IEnumerator Rotate(float start, float end, float duration, Sprite swapImage = null)
    {
        if (start < 0.5f && end > 4f)
        {
            start = 7f;
        }
        else if (start > 6f && end < 3f)
        {
            start = 0f;
        }
        
        float timer = 0;
        bool imageSwaped = false;
        //check if image needs to be Swaped
        if (swapImage != null)
        {
            imageSwaped = true;
        }
        while (timer < duration)
        {
            float sampleFloat = Mathf.Lerp(start, end, timer / duration);

            if (imageSwaped && sampleFloat > 3.1f && sampleFloat < 3.9f)
            {
                iconImage.sprite = swapImage;
                imageSwaped = false;
            }

            rect.localPosition = spline.GetSample(sampleFloat).location + offset;
            barImage.color = gradient.Evaluate(sampleFloat / spline.nodes.Count);
            iconImage.color = gradient.Evaluate(sampleFloat / spline.nodes.Count);

            timer += Time.deltaTime;
            yield return null;
        }

        rect.localPosition = spline.GetSample(end).location + offset;
        barImage.color = gradient.Evaluate(end / spline.nodes.Count);
        iconImage.color = gradient.Evaluate(end / spline.nodes.Count);

        if (end > 6)
        {
            end = 0f;
        }
        else if (end < 0.5f)
        {
            end = 7f;
        }
        currentPosition = end;
        coroutine = null;
    }

    public void Init(float startPosition, Sprite icon)
    {
        currentPosition = startPosition;
        iconImage.sprite = icon;
        rect.localPosition = spline.GetSample(currentPosition).location + offset;
        barImage.color = gradient.Evaluate(currentPosition / spline.nodes.Count);
        iconImage.color = gradient.Evaluate(currentPosition / spline.nodes.Count);
    }
}
