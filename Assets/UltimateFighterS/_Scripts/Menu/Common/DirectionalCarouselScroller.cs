using UnityEngine;

public class DirectionalCarouselScroller : MonoBehaviour
{
    [SerializeField] private InputSystem input;

    private Carousel _carousel;
    private int _lastDirection;

    public void Awake()
    {
        _carousel = GetComponent<Carousel>();
    }

    public void Update()
    {
        float x = input.GetDirection().x;
        int direction = x > 0 ? 1 : x < 0 ? -1 : 0;

        if (direction != _lastDirection)
            _carousel.SelectRelative(direction);

        _lastDirection = direction;
    }
}