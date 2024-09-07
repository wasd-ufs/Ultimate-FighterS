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
    [SerializeField] private float trainLength;
    [SerializeField] private float trainHeight;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 restPoint;
    [SerializeField] private Passenger passenger;
    [SerializeField] [Range(0, 100)] private int passengerChance;
    [SerializeField] private Timer stoppedTimer;
    [SerializeField] private Timer runningTimer;

    

    private bool isPassengerOutOfTrain;
    private float aceleration;
    private float speedAux;
    private float speed;
    private float direction;
    private bool isDecelerating;
    private bool isAcelerating;
    private Vector3 origin;
    void Awake()
    {
        origin = new Vector3(0, spawnPoint.y, 0);
        MoveToRestPosition();
        isDecelerating = false;
        isAcelerating = false;              
        isPassengerOutOfTrain = true;
        SetScale(new Vector3(trainLength, trainHeight, 1));
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
            speedAux = Mathf.MoveTowards(speedAux, 0, aceleration * Time.deltaTime);
            transform.position = Vector3.MoveTowards(currentPosition, origin, speedAux * Time.deltaTime);  
            if (currentPosition.x == 0)
            {
                stoppedTimer.waitTime = Random.Range(minStoppedTime, maxStoppedTime);
                stoppedTimer.Init();
                isDecelerating = false;
                if (!isPassengerOutOfTrain)
                {
                    int chance = Random.Range(0, 101);
                    if (chance <= passengerChance)
                    {
                        passenger.MoveToPlataform();
                        isPassengerOutOfTrain = true;
                    }
                }
                else if (isPassengerOutOfTrain)
                {
                    passenger.MoveToRestPoint();
                    isPassengerOutOfTrain = false;  
                }
            }
        }
        else if (isAcelerating)
        {
            Vector3 currentPosition = transform.position;
            Vector3 target = new Vector3(direction * spawnPoint.x, spawnPoint.y, 0);

            speedAux = Mathf.MoveTowards(speedAux, speed, 2*aceleration * Time.deltaTime);
            transform.position = Vector3.MoveTowards(currentPosition, target, speedAux * Time.deltaTime);
            
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
        this.speedAux = speed;
        aceleration = 0.1f * speed;
        MoveToSpawnPosition();
        FlipInDirection();
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
        MoveToRestPosition();
        isAcelerating = false;       
    }

    public void Acelerate()
    {
        isAcelerating = true;
        this.speed = CalculateSpeed();
        this.speedAux = 0;
        aceleration = 0.3f * speed;
    }
    public void MoveToRestPosition()
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
    private void FlipInDirection()
    {
        Vector3 scale = this.transform.localScale;
        Vector3 newScale = new Vector3(direction * scale.x, scale.y, scale.z);
        transform.localScale = newScale;
    }

    private void SetScale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }
        
    private int GetRandomSign()
    {
        return Random.Range(-1.0f, 1.0f) < 0 ? -1 : 1;
    }
    public float GetTrainLength() 
        => this.trainLength;

    public float GetSpawnPointY()
        => spawnPoint.y;

    public Vector3 GetRestPoint()
        => restPoint;               
}
