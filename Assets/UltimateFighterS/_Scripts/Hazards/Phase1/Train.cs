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
    [SerializeField] private float maxTimeToArriveOrigin;
    [SerializeField] private float minTimeToArriveOrigin;
    [SerializeField] private float trainLength;
    [SerializeField] private float trainHeight;
    [Tooltip("positive coordinate")]
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 originPoint;
    [SerializeField] private Vector3 restPoint;
    [SerializeField] private Passenger passenger;
    [SerializeField] [Range(0, 100)] private int passengerChance;
    [SerializeField] private Timer stoppedTimer;
    [SerializeField] private Timer runningTimer;

    
    [Header("just to see")]
    [SerializeField] private float distance;
    [SerializeField] private bool isPassengerOutOfTrain;
    [SerializeField] private float aceleration;
    [SerializeField] private float speedAux;
    [SerializeField] private float speed;
    [SerializeField] private int direction;
    [SerializeField] private bool isDecelerating;
    [SerializeField] private bool isAcelerating;
   
    void Start()
    {   
        MoveToRestPosition();
        isDecelerating = false;
        isAcelerating = false;              
        isPassengerOutOfTrain = false;
        transform.localScale.Scale(new Vector3(trainLength, trainHeight, 1));
        runningTimer.waitTime = Random.Range(minRequiredTimeForTrain, maxRequiredTimeForTrain);
        runningTimer.Init();
        distance = spawnPoint.x - originPoint.x;
    }

    void Update()
    {   
        if (isDecelerating)
        {
            Vector3 currentPosition = transform.position;
            transform.position = Vector3.MoveTowards(currentPosition, originPoint, speedAux * Time.deltaTime);  
            speedAux = Mathf.MoveTowards(speedAux, 0, aceleration * Time.deltaTime);
                            
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

            transform.position = Vector3.MoveTowards(currentPosition, target, speedAux * Time.deltaTime);
            speedAux = Mathf.MoveTowards(speedAux, speed, 2*aceleration * Time.deltaTime);
            
            if (Mathf.Approximately(currentPosition.x, direction * spawnPoint.x)) 
            {
                runningTimer.waitTime = Random.Range(minRequiredTimeForTrain, maxRequiredTimeForTrain);
                runningTimer.Init();
                isAcelerating = false;
            }
        }
    }
    
    public void RunTrain()
    {                   
        direction = GetRandomSign();
        speed = CalculateSpeed();
        aceleration = CalculateAceleration(speed);
        speedAux = speed;
        MoveToSpawnPosition(direction);
        FlipInDirection(direction);

        Wave wave = FindObjectOfType<Wave>();
        if (wave) {
            wave.SetDirection(direction);
            wave.ActiveWave(false);
        }
        
        isDecelerating = true;
    }
    public void ResetTrain()                
    {   
        if(transform.localScale.x < 0)
            FlipInDirection(-1);
        MoveToRestPosition();
        isAcelerating = false;
        Wave wave = FindObjectOfType<Wave>();
        if (wave) {
            wave.ActiveWave(false);
        }  
    }

    public void Acelerate()
    {
        isAcelerating = true;
        speed = CalculateSpeed();
        aceleration = CalculateAceleration(speed);
        speedAux = 0;
        Wave wave = FindObjectOfType<Wave>();
        if (wave) {
            wave.ActiveWave(true);
        }  
    }
    public float CalculateAceleration(float speed)
        =>  speed * speed / (2 * distance);
    public float CalculateSpeed()
        => spawnPoint.x / Random.Range(minTimeToArriveOrigin, maxTimeToArriveOrigin);
    public void MoveToRestPosition()
        => this.transform.position = restPoint;    
    private void MoveToSpawnPosition(int direction)
        => this.transform.position = new Vector3(-direction*spawnPoint.x,spawnPoint.y,spawnPoint.z);
    private void FlipInDirection(int sign)
    {
        Vector3 scale = this.transform.localScale;
        transform.localScale.Scale(new Vector3(sign * scale.x, scale.y, scale.z));
    }
    private int GetRandomSign()
        => Random.Range(-1.0f, 1.0f) < 0 ? -1 : 1;
    public float GetTrainLength() 
        => this.trainLength;
    public float GetTrainHeight()
        => this.trainHeight;
    public Vector3 GetSpawnPoint()
        => spawnPoint;
    public Vector3 GetRestPoint()
        => restPoint;               
}
