using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAnimator : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve scaleCurve;
    [SerializeField]
    private float duration = 1f;
    public UnityEvent onAnimationFinished;
    private Transform obj;
    private Coroutine routine;

    void Start()
    {
        obj = transform;
        GetComponent<Button>().onClick.AddListener(delegate { StartAnimation(duration); });
    }

    public void StartAnimation(float duration)
    {
        if (routine == null)
        {
            routine = StartCoroutine(Animation(duration));
        }
    }

    private IEnumerator Animation(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            obj.localScale = Vector3.one * scaleCurve.Evaluate(timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }
        obj.localScale = Vector3.one * scaleCurve.Evaluate(duration/duration);
        routine = null;
        OnAnimationFinished();
    }

    private void OnAnimationFinished()
    {
        onAnimationFinished.Invoke();
    }

}
