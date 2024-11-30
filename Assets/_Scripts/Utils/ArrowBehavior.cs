using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArrowBehavior : MonoBehaviour
{
    private int id;
    private int sumCoef;
    
    private float fatherPosX;
    private float leftCamPosition;
    private float rightCamPosition;
    private Color color = Color.white;
    [SerializeField] private IdComponent idcomponent;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private Camera cam;
    
    void Start()
    {
        id = idcomponent.id;
        sumCoef = id * 3;
        
        if(id == 0){color = Color.red;}
        if(id == 1){color = Color.blue;}
        if(id == 2){color = Color.yellow;}
        if(id == 3){color = Color.green;}
        
        myRenderer.sprite = sprites[1];
        myRenderer.color = color;
        var fatherSprite = GetComponentInChildren<SpriteRenderer>();
        var spriteLength = fatherSprite.bounds.size.y;
        print(spriteLength);
        gameObject.transform.position = new Vector3(transform.position.x, (transform.position.y + spriteLength + 0.2f) , transform.position.z);
    
    }
    
    void Update()
    {   
        float distanceDif = (transform.position - cam.transform.position).z;
        leftCamPosition = cam.ViewportToWorldPoint(new Vector3(0f, 0f, distanceDif)).x;
        rightCamPosition = cam.ViewportToWorldPoint(new Vector3(1f, 0f, distanceDif)).x;
        
        fatherPosX = transform.parent.position.x;

        if (fatherPosX <= leftCamPosition)
        {
            myRenderer.sprite = sprites[0 + sumCoef];
            gameObject.transform.position = new Vector3(leftCamPosition + 0.5f, gameObject.transform.position.y , gameObject.transform.position.z);
        }
        else if(fatherPosX >= rightCamPosition)
        {
            myRenderer.sprite = sprites[2 + sumCoef];
            gameObject.transform.position = new Vector3(rightCamPosition - 0.5f, gameObject.transform.position.y , gameObject.transform.position.z);
        }
        else
        {
            myRenderer.sprite = sprites[1 + sumCoef];
            gameObject.transform.position = new Vector3(fatherPosX, gameObject.transform.position.y , gameObject.transform.position.z);
        }
        
    }
}
