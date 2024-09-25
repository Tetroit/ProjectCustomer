using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CameraFade : MonoBehaviour
{
    public KeyCode key = KeyCode.Space; // Which key should trigger the fade?
    public float speedScale = 1f;
    public Color fadeColor = Color.black;
    // Rather than Lerp or Slerp, we allow adaptability with a configurable curve
    public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));
    public bool startFadedOut = false;

    UnityEvent OnFadeIn;


    private float alpha = 0f;
    private Texture2D texture;
    private int direction = 0;
    private float time = 0f;

    private void Start()
    {
        if (startFadedOut) alpha = 1f; else alpha = 0f;
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }

    private void Update()
    {

    }

    public void SetTransitionEvent(UnityAction action)
    {
        OnFadeIn.RemoveAllListeners();
        OnFadeIn.AddListener(action);
    }

    IEnumerator Transition()
    {
        float timer = speedScale * 2;
        direction = 1;
        while (timer > speedScale)
        {
            timer -= Time.deltaTime;
            float fac = (timer - speedScale) / speedScale;

            alpha = Curve.Evaluate(fac);

            yield return new WaitForEndOfFrame();
        }
        OnFadeIn?.Invoke();
        direction = 1;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            float fac = 1 - timer / speedScale;

            alpha = Curve.Evaluate(fac);

            yield return new WaitForEndOfFrame();
        }
        alpha = 0;
        direction = 0;
        yield return null;
    }
    public void FadeOut()
    {
        if (alpha >= 1f) // Fully faded out
        {
            alpha = 1f;
            time = 0f;
            direction = 1;
        }
    }

    public void FadeIn()
    {
        if (alpha <= 1f) // Fully faded out
        {
            alpha = 1f;
            time = 0f;
            direction = 1;
        }
    }

    public void StartTransition()
    {
        StartCoroutine(Transition());
    }
    public void StartTransition(UnityAction action)
    {
        SetTransitionEvent(action);
        StartCoroutine(Transition());
    }
    public void OnGUI()
    {
        if (alpha > 0f) GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        if (direction != 0)
        {
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            if (alpha <= 0f || alpha >= 1f) direction = 0;
        }
    }
}