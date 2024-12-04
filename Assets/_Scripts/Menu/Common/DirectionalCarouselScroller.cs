using System;
using UnityEngine;

public class DirectionalCarouselScroller : MonoBehaviour
{
    [SerializeField] private InputSystem input;
    
    private Carousel carousel;
    private int lastDirection;
    
    public void Awake()
    {
        carousel = GetComponent<Carousel>();
    }

    public void Update()
    {
        var x = input.GetDirection().x;
        var direction = (x > 0) ? 1 : (x < 0) ? -1 : 0;
        
        if (direction != lastDirection)
            carousel.SelectRelative(direction);
        
        lastDirection = direction;
    }
}