using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Passenger : MonoBehaviour
{
    [SerializeField] private Train train;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    private float height;
    private float trainLength;
    private Vector3 restPoint;

    private void Awake()
    {
        height = train.GetSpawnPoint().y;
        trainLength = train.GetTrainLength();
        restPoint = train.GetRestPoint();
        MoveToRestPoint();
    }

    public void MoveToPlataform()
    {
        float randomPlace = Random.Range((-trainLength / 2)+offsetX, (trainLength / 2)-offsetX);
        this.transform.position = new Vector3(randomPlace, height+offsetY, 0);
    }               
    public void MoveToRestPoint()
        => this.transform.position = restPoint;
}
