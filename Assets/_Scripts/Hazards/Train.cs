using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Timer))]
public class Train : MonoBehaviour
{   
    [Header("Parameters")]
    [SerializeField] private float maxRequiredTimeForTrain;
    [SerializeField] private float minRequiredTimeForTrain;
    [SerializeField] private float maxStoppedTime;
    [SerializeField] private float minStoppedTime;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 restPoint;
    
    [Header(" Read Only Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float direction;
    [SerializeField] private bool isDecelerating;
    [SerializeField] private bool isStopped;
    [SerializeField] private bool isAcelerating;
    [SerializeField] private Timer stoppedTimer;
    [SerializeField] private Timer runningTimer;
    private Vector3 origin;
    void Awake()
    {
        origin = new Vector3(0, spawnPoint.y, 0);
        SetRestPosition();
        isDecelerating = false;
        isStopped = false;
        isAcelerating = false;
    }               
            
    void Start()
    {
        runningTimer.waitTime = Random.Range(minRequiredTimeForTrain, maxRequiredTimeForTrain);
        runningTimer.Init();
    }

    void Update()
    {
        if (isDecelerating)
        {
            Vector3 currentPosition = transform.position;
            transform.position = Vector3.MoveTowards(currentPosition, origin, speed * Time.deltaTime);  
            if (currentPosition.x == 0)
            {
                stoppedTimer.waitTime = Random.Range(minStoppedTime, maxStoppedTime);
                stoppedTimer.Init();
                isDecelerating = false;
                isStopped = true;
            }
        }
        else if (isStopped)
        {
            // faça o passageiro misterioso sair
        }
        else if (isAcelerating)
        {
            Vector3 currentPosition = transform.position;
            Vector3 target = new Vector3(direction * spawnPoint.x, spawnPoint.y, 0);
            transform.position = Vector3.MoveTowards(currentPosition, target, speed * Time.deltaTime);
            
            if (Mathf.Approximately(currentPosition.x, direction * spawnPoint.x)) //(Mathf.Abs(currentPosition.x) - Mathf.Abs(direction * spawnPoint.x) < float.Epsilon)
            {
                runningTimer.waitTime = Random.Range(minRequiredTimeForTrain, maxRequiredTimeForTrain);
                runningTimer.Init();
                isAcelerating = false;
            }

        }
    }
    
    public void RunTrain()
    {   
        this.direction = GetRandomSign(); 
        this.speed = CalculateSpeed();
        MoveToSpawnPosition();
        Flip();
        isDecelerating = true;
    }
    public void ResetTrain()                
    {   
        Vector3 scale = this.transform.localScale;
        if (scale.x < 0)
        {
            Vector3 newScale = new Vector3(-1 * scale.x, scale.y, scale.z);
            transform.localScale = newScale; 
        }
        isAcelerating = false;                  	
    }

    public void Acelerate()
    {
        isStopped = false;
        isAcelerating = true;
        this.speed = CalculateSpeed();
    }
    public void SetRestPosition()
    {
        this.transform.position = restPoint;    
    }

    private void MoveToSpawnPosition()
    {
        this.transform.position = new Vector3(-direction*spawnPoint.x,spawnPoint.y,spawnPoint.z);
    }
    private float CalculateSpeed()
    {
        return Random.Range(minSpeed, maxSpeed);
    }
    private void Flip()
    {
        Vector3 scale = this.transform.localScale;
        Vector3 newScale = new Vector3(direction * scale.x, scale.y, scale.z);
        transform.localScale = newScale;
    }

    private int GetRandomSign()
    {
        return Random.Range(-1.0f, 1.0f) < 0 ? -1 : 1;
    }
}
