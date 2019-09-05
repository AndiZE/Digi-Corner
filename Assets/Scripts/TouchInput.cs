using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    //private Wheel wheel;

    private bool isActive;

    [SerializeField]
    private Transform inputArea;
    private float xInput = 0;
    private UIWheel uiWheel;
    private bool isInputActive;
    private void Start()
    {
        //wheel = GetComponentInParent<Wheel>();
        Input.simulateMouseWithTouches = true;
        uiWheel = GetComponent<UIWheel>();
    }

    public void ActivateInput(bool active)
    {
        isInputActive = active;
        Debug.Log(active);

        if (!active)
        {
            if (xInput > 0)
            {
                SwapRight();
            }
            else
            {
                SwapLeft();
            }

            xInput = 0;
        }
    }

    private void Update()
    {
        if (isInputActive && Input.touchCount == 1)
        {
            //for (int i = 0; i < Input.touchCount; i++)
            //{
            //    var touch = Input.GetTouch(i);

            //    //Debug.Log(Input.touchCount);
            //    //Check if Hitted object is Wheel
            //    //if (hit.transform != null && hit.transform.GetInstanceID() == inputArea.GetInstanceID())
            //    //{

            //    //    if (touch.phase == TouchPhase.Moved)
            //    //    {
            //    //        xInput += touch.deltaPosition.x;
            //    //        //wheel.SwapRotate(touch.deltaPosition.x);
            //    //    }
            //    //    else if (touch.phase == TouchPhase.Ended ||
            //    //            touch.phase == TouchPhase.Canceled)
            //    //    {
            //    //        if (xInput > 0)
            //    //        {
            //    //            SwapRight();
            //    //        }
            //    //        else if (xInput < 0)
            //    //        {
            //    //            SwapRight();
            //    //        }
            //    //        xInput = 0;
            //    //        //wheel.SwapEnd();
            //    //    }
            //    //    break;
            //    //}

            //}
            xInput += Input.GetTouch(0).deltaPosition.x;
        }
    }

    private void SwapLeft()
    {
        uiWheel.Swap(false);
    }

    private void SwapRight()
    {
        uiWheel.Swap(true);
    }
}
