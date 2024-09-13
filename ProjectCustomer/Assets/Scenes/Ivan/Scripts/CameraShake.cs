
using UnityEngine;

public class CameraWalkingShake : MonoBehaviour
{
    Vector3 offset;
    float rotationOffset;
    public float frequency;
    float _cooldown;
    public AnimationCurve XShake;
    public AnimationCurve YShake;
    public PlayerControlsIvan controls;

    public float rotationIntensity;
    bool _rightSide;
    float _amplitude = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.isMoving) _amplitude = Mathf.Lerp(_amplitude, 1, 0.01f);
        else _amplitude = Mathf.Lerp(_amplitude, 0, 0.01f);
        if (_cooldown >= frequency)
        {
            if (controls.isMoving)
            {
                _cooldown -= frequency;
                _rightSide = !_rightSide;
            }
        }
        if (_cooldown < frequency)
        {
            _cooldown += Time.deltaTime;
        }
        float facY = _cooldown / frequency;
        float facX = facY;

        offset = new Vector3((_rightSide) ? XShake.Evaluate(facX) : -XShake.Evaluate(facX), YShake.Evaluate(facY), 0f);
        offset.x *= _amplitude;
        rotationOffset = -rotationIntensity * offset.x;

        transform.localEulerAngles = new Vector3(0f, 0f, rotationOffset);
        transform.localPosition = offset;

    }
}
