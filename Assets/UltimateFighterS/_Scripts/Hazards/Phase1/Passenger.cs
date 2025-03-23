using UnityEngine;
using Random = UnityEngine.Random;

public class Passenger : MonoBehaviour
{
    [SerializeField] private Train train;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    private float _height;
    private Vector3 _restPoint;
    private float _trainLength;

    private void Awake()
    {
        _height = train.GetSpawnPoint().y;
        _trainLength = train.GetTrainLength();
        _restPoint = train.GetRestPoint();
        MoveToRestPoint();
    }

    public void MoveToPlataform()
    {
        float randomPlace = Random.Range(-_trainLength / 2 + offsetX, _trainLength / 2 - offsetX);
        transform.position = new Vector3(randomPlace, _height + offsetY, 0);
    }

    public void MoveToRestPoint()
    {
        transform.position = _restPoint;
    }
}