using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWheel : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve hightCurve;
    [SerializeField]
    private AnimationCurve scaleCurve;
    [SerializeField]
    private Vector2 hightValues;
    private float width = 1920f;
    private float middlePoint;
    [SerializeField]
    private float offset = 200f;

    [SerializeField]
    private RectTransform obj1;
    [SerializeField]
    private RectTransform obj2;
    [SerializeField]
    private Vector2 scaleRange;

    private Coroutine activeRoutine;
    private PanelSwitch panelSwitcher;

    private void Start()
    {
        middlePoint = width / 2f;
        panelSwitcher = GetComponent<PanelSwitch>();
        Init();
    }

    public void Swap(bool isRight)
    {
        if (activeRoutine != null) return;

        if (isRight)
        {
            activeRoutine = StartCoroutine(MoveRight());
        }
        else
        {
            activeRoutine = StartCoroutine(MoveLeft());
        }
    }

    private IEnumerator MoveLeft()
    {
        float timer = 0;
        float duration = 1f;
        while (timer < duration)
        {
            float hlerp1 = Mathf.Lerp(hightValues.y, hightValues.x, hightCurve.Evaluate(timer));
            float hlerp2 = Mathf.Lerp(hightValues.x, hightValues.y, hightCurve.Evaluate(timer));
            float wLerp = Mathf.Lerp(0, middlePoint + offset, hightCurve.Evaluate(timer));
            float scaleLerp1 = Mathf.Lerp(scaleRange.y, scaleRange.x, scaleCurve.Evaluate(timer));
            float scaleLerp2 = Mathf.Lerp(scaleRange.x, scaleRange.y, scaleCurve.Evaluate(timer));
            obj1.localPosition = new Vector3(-wLerp, hlerp1, obj1.localPosition.z);
            obj2.localPosition = new Vector3(middlePoint + offset - wLerp, hlerp2, obj2.localPosition.z);

            obj1.localScale = Vector3.one * scaleLerp1;
            obj2.localScale = Vector3.one * scaleLerp2;

            timer += Time.deltaTime;

            yield return null;
        }
        //Centered one
        obj2.localPosition = new Vector3(0, hightValues.y, obj2.localPosition.z);
        obj2.localScale = Vector3.one * scaleRange.y;

        //Side one
        obj1.localPosition = new Vector3((middlePoint + offset) * -1f, hightValues.x, obj1.localPosition.z);
        obj1.localScale = Vector3.one * scaleRange.x;

        activeRoutine = null;
        OnSwapLeftFinished();
    }

    private IEnumerator MoveRight()
    {
        float timer = 0;
        float duration = 1f;
        while (timer < duration)
        {
            float hlerp1 = Mathf.Lerp(hightValues.y, hightValues.x, hightCurve.Evaluate(timer));
            float hlerp2 = Mathf.Lerp(hightValues.x, hightValues.y, hightCurve.Evaluate(timer));
            float wLerp = Mathf.Lerp(0, middlePoint + offset, hightCurve.Evaluate(timer));
            float scaleLerp1 = Mathf.Lerp(scaleRange.y, scaleRange.x, scaleCurve.Evaluate(timer));
            float scaleLerp2 = Mathf.Lerp(scaleRange.x, scaleRange.y, scaleCurve.Evaluate(timer));
            obj1.localPosition = new Vector3((middlePoint + offset)*-1f + wLerp, hlerp2, obj1.localPosition.z);
            obj2.localPosition = new Vector3(wLerp, hlerp1, obj2.localPosition.z);

            obj2.localScale = Vector3.one * scaleLerp1;
            obj1.localScale = Vector3.one * scaleLerp2;

            timer += Time.deltaTime;

            yield return null;
        }
        //Centered one
        obj1.localPosition = new Vector3(0, hightValues.y, obj2.localPosition.z);
        obj1.localScale = Vector3.one * scaleRange.y;
        //Side one
        obj2.localPosition = new Vector3(middlePoint + offset, hightValues.x, obj1.localPosition.z);
        obj2.localScale = Vector3.one * scaleRange.x;

        activeRoutine = null;
        OnSwapRightFinished();
    }

    private void Init()
    {
        //Centered one
        obj1.localPosition = new Vector3(0, hightValues.y, obj2.localPosition.z);
        obj1.localScale = Vector3.one * scaleRange.y;
        //Side one
        obj2.localPosition = new Vector3(middlePoint + offset, hightValues.x, obj1.localPosition.z);
        obj2.localScale = Vector3.one * scaleRange.x;
    }

    private void OnSwapRightFinished()
    {
        panelSwitcher.SwitchNextQuestion();
    }

    private void OnSwapLeftFinished()
    {
        panelSwitcher.SwitchPreviousQuestion();
    }
}
