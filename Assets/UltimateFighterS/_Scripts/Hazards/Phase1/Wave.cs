using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wave : MonoBehaviour
{
    [Header("Ellipse Parameters")]
    [SerializeField] private float maxAxis;
    [SerializeField] private float minAxis;
    [SerializeField] private float Angles;
    private float maxAngles = Mathf.PI;
    private float minAngles = 0;
    private int direction = 1;
    [SerializeField] private bool isActive = false;
    private float waveDuration;
    [SerializeField]private float speed;

    void Awake(){
        Angles = 0;
        waveDuration = maxAngles/speed;
    }
    
     public void FixedUpdate() {
        if (isActive) {
        if (Angles == 0) {
                StartCoroutine(WaveCycle());
        }
    } 

}

    public void SetPositionWave(float x, float y) 
    =>    this.transform.position = new Vector3(x, -10.5f + y, 0);

    public void SetPositionDefaultWave() 
    =>    this.transform.position = new Vector3(11, -20, 0);

    public void SetDirection(int newDirection) {
       direction = - newDirection;

       if (direction < 0) {
        transform.eulerAngles = new Vector2 (0, 180);
       }
       else 
       transform.eulerAngles = new Vector2 (0, 0);
    }

    public void ActiveWave(bool active) {
       isActive = active;
       if (active) {
        Angles = 0;
       }
    }

    private IEnumerator WaveCycle() {
        float time = 0f;

        while (time < waveDuration) {
            time += Time.deltaTime;
             if (Angles >= minAngles && Angles <= maxAngles) {
        Angles += speed * Time.deltaTime;
        }
        
        else {
         Angles = 0;
        }
        
        float x = maxAxis * Mathf.Cos(Angles) * direction;
        float y = minAxis * Mathf.Sin(Angles);
        SetPositionWave(x, y);
            
            yield return null;
        }
        isActive = false;
        SetPositionDefaultWave();
        Angles = 0;
    }
    
}
