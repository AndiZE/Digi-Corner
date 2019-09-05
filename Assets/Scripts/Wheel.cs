using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public GameObject iconPrefabs;
    public GameObject wheel;
    public int amount;
    public float offset = 1f;

    public Transform focusPoint;
    private float snapAngles;
    public AnimationCurve curve;

    private float swapXDelta = 0f;
    public float rotateSpeed = 1f;

    public float lastAngle;
    public Coroutine moveRoutine;

    void Start()
    {

        for (int i = 0; i < amount; i++)
        {
            var distance = wheel.GetComponent<CapsuleCollider>().radius * wheel.transform.localScale.x;
            snapAngles = 360f / amount;
            Vector3 position = transform.localPosition + (transform.rotation * Quaternion.Euler(0, snapAngles * i, 0)) * transform.forward * distance;
            var obj = Instantiate(iconPrefabs, position, Quaternion.identity, transform);
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, offset, obj.transform.localPosition.z);
        }

        float angle = Vector3.SignedAngle(transform.forward, focusPoint.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        lastAngle = angle;

    }

    private IEnumerator Rotate(float duration, bool swapLeft)
    {
        float angle = swapLeft ? -snapAngles : snapAngles;
        float totalDuration = duration;
        float time = 0f;
        float startAngle = transform.rotation.eulerAngles.y;
        float endAngle = lastAngle + angle;

        if (startAngle > 315)
        {
            Debug.LogError("Start Angle Modified - " + transform.rotation.eulerAngles.y.ToString("F1"));
            startAngle -= 360f;
        }
        else if (endAngle < -315)
        {
            Debug.LogError("End Angle Modified + " + transform.rotation.eulerAngles.y.ToString("F1"));
            endAngle += 360f;
        }

        if (endAngle > 360)
        {
            Debug.LogError("End Angle Modified - " + transform.rotation.eulerAngles.y.ToString("F1"));
            endAngle -= 360;
        }
        else if (startAngle < -360)
        {
            Debug.LogError("Start Angle Modified + " + transform.rotation.eulerAngles.y.ToString("F1"));
            startAngle += 360f;
        }


        Debug.Log("Angle: " + angle);


        //Debug.LogFormat("After correction StartAngle:{0} /// EndAngle:{1}", startAngle.ToString("F1"), endAngle.ToString("F1"));
        Debug.LogFormat("{0} to {1}",  startAngle.ToString("F1"), endAngle.ToString("F1"));

        while (time <= totalDuration)
        {
            time += Time.deltaTime;
            float lerpAngle = Mathf.Lerp(startAngle, endAngle, curve.Evaluate(time / totalDuration));
            transform.rotation = Quaternion.Euler(0, lerpAngle, 0);
            //Debug.LogFormat("Value: {0} from {1} to {2}", lerpAngle.ToString("F1"), startAngle.ToString("F1"), endAngle.ToString("F1"));
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(0, endAngle, 0);
        lastAngle = transform.eulerAngles.y;
        moveRoutine = null;
    }

    public void SwapRotate(float xDelta)
    {
        swapXDelta += xDelta;
        //swapXDelta = Mathf.Clamp(swapXDelta, -snapAngles, snapAngles);
        //if (Mathf.Abs(swapXDelta) - 5f  <= snapAngles)
        //{
            transform.rotation = transform.rotation * Quaternion.Euler(0, -xDelta * rotateSpeed * Time.deltaTime, 0);
        //}

    }

    public void SwapEnd()
    {

        float diff = swapXDelta;
        //ResetValue
        bool isLeft = swapXDelta < 0 ? false : true;
        if (moveRoutine == null)
        {
            moveRoutine = StartCoroutine(Rotate(0.5f, isLeft));
        }
        swapXDelta = 0f;

    }

}
