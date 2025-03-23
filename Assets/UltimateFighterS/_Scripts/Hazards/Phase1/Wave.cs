using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Wave : MonoBehaviour
{
    [Header("Ellipse Parameters")] [SerializeField]
    private float maxAxis;

    [SerializeField] private float minAxis;
    [FormerlySerializedAs("Angles")] [SerializeField] private float angles;
    [SerializeField] private bool isActive;
    [SerializeField] private float speed;
    private int _direction = 1;
    private readonly float _maxAngles = Mathf.PI;
    private readonly float _minAngles = 0;
    private float _waveDuration;

    private void Awake()
    {
        angles = 0;
        _waveDuration = _maxAngles / speed;
    }

    public void FixedUpdate()
    {
        if (isActive)
            if (angles == 0)
                StartCoroutine(WaveCycle());
    }

    public void SetPositionWave(float x, float y)
    {
        transform.position = new Vector3(x, -10.5f + y, 0);
    }

    public void SetPositionDefaultWave()
    {
        transform.position = new Vector3(11, -20, 0);
    }

    public void SetDirection(int newDirection)
    {
        _direction = -newDirection;

        if (_direction < 0)
            transform.eulerAngles = new Vector2(0, 180);
        else
            transform.eulerAngles = new Vector2(0, 0);
    }

    public void ActiveWave(bool active)
    {
        isActive = active;
        if (active) angles = 0;
    }

    private IEnumerator WaveCycle()
    {
        float time = 0f;

        while (time < _waveDuration)
        {
            time += Time.deltaTime;
            if (angles >= _minAngles && angles <= _maxAngles)
                angles += speed * Time.deltaTime;

            else
                angles = 0;

            float x = maxAxis * Mathf.Cos(angles) * _direction;
            float y = minAxis * Mathf.Sin(angles);
            SetPositionWave(x, y);

            yield return null;
        }

        isActive = false;
        SetPositionDefaultWave();
        angles = 0;
    }
}