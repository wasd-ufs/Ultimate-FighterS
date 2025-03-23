using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    [SerializeField] private IdComponent idcomponent;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private SpriteRenderer myRenderer;
    private Camera _cam;
    private Color _color = Color.white;

    private float _fatherPosX;
    private int _id;
    private float _leftCamPosition;
    private float _rightCamPosition;
    private int _sumCoef;

    private void Awake()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        _id = idcomponent.id;
        _sumCoef = _id * 3;

        if (_id == 0) _color = Color.red;
        if (_id == 1) _color = Color.blue;
        if (_id == 2) _color = Color.yellow;
        if (_id == 3) _color = Color.green;

        myRenderer.sprite = sprites[1];
        myRenderer.color = _color;
        SpriteRenderer fatherSprite = GetComponentInChildren<SpriteRenderer>();
        float spriteLength = fatherSprite.bounds.size.y;
        print(spriteLength);
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + spriteLength + 0.2f,
            transform.position.z);
    }

    private void Update()
    {
        float distanceDif = (transform.position - _cam.transform.position).z;
        _leftCamPosition = _cam.ViewportToWorldPoint(new Vector3(0f, 0f, distanceDif)).x;
        _rightCamPosition = _cam.ViewportToWorldPoint(new Vector3(1f, 0f, distanceDif)).x;

        _fatherPosX = transform.parent.position.x;

        if (_fatherPosX <= _leftCamPosition)
        {
            myRenderer.sprite = sprites[0 + _sumCoef];
            gameObject.transform.position = new Vector3(_leftCamPosition + 0.5f, gameObject.transform.position.y,
                gameObject.transform.position.z);
        }
        else if (_fatherPosX >= _rightCamPosition)
        {
            myRenderer.sprite = sprites[2 + _sumCoef];
            gameObject.transform.position = new Vector3(_rightCamPosition - 0.5f, gameObject.transform.position.y,
                gameObject.transform.position.z);
        }
        else
        {
            myRenderer.sprite = sprites[1 + _sumCoef];
            gameObject.transform.position = new Vector3(_fatherPosX, gameObject.transform.position.y,
                gameObject.transform.position.z);
        }
    }
}