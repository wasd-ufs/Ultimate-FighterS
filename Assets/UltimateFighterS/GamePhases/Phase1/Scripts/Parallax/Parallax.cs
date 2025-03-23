using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform cam;
    public float speedParallaxEfx;
    private float _spritelength;
    private Vector2 _startPos;

    private void Start()
    {
        _startPos = transform.position;
        _spritelength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        //0 = Dont move & 1 = Move with cam
        Vector2 distance = cam.position * speedParallaxEfx;

        //Move
        transform.position = new Vector3(_startPos.x + distance.x, _startPos.y + distance.y, transform.position.z);
    }
}