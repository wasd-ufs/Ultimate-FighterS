using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minZoom = 5f;
    public float maxZoom = 17f;
    public float zoomSpeed = 5f;
    public float moveSpeed = 3f;
    public float padding = 15f;

    private Camera cam;
    private Vector2 worldMinBounds, worldMaxBounds;
    
    void Awake()
    {
        cam = GetComponent<Camera>();
        MatchManager.OnMatchStarting.AddListener(OnMatchStarting);
    }

    void OnMatchStarting()
    {
        GetBounds();
    }

    void GetBounds()
    {
        worldMinBounds = Vector2.negativeInfinity;
        worldMaxBounds = Vector2.positiveInfinity;
        
        var limits = GameObject.FindGameObjectsWithTag("StageLimits").ToList();
        if (limits.Count > 0)
        {
            worldMinBounds.x = limits.Min(x => x.transform.position.x);
            worldMinBounds.y = limits.Min(x => x.transform.position.y);

            worldMaxBounds.x = limits.Max(x => x.transform.position.x);
            worldMaxBounds.y = limits.Max(x => x.transform.position.y);
        }
        
        maxZoom = Mathf.Min((worldMaxBounds.x - worldMinBounds.x) / cam.aspect, worldMaxBounds.y - worldMinBounds.y) / 2f;
        Debug.Log(maxZoom);
    }

    void LateUpdate()
    {
        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {
        var players = MatchManager.GetAlivePlayers();
        
        Vector3 averagePosition = Vector3.zero;
        foreach (GameObject player in players)
        {
            averagePosition += player.transform.position;
        }
        
        averagePosition /= players.Count;
        
        float desiredMinY = players.Min(p => p.transform.position.y) - padding / 2f / cam.orthographicSize;
        float camMinY = transform.position.y - cam.orthographicSize;
        float difference = Mathf.Max(desiredMinY - camMinY, 0f);
        averagePosition += difference * Vector3.up;
        
        Vector3 finalPosition = Vector3.Lerp(transform.position, averagePosition, Time.deltaTime * moveSpeed);
                
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        finalPosition.x = Mathf.Clamp(finalPosition.x, worldMinBounds.x + camHalfWidth, worldMaxBounds.x - camHalfWidth);
        finalPosition.y = Mathf.Clamp(finalPosition.y, worldMinBounds.y + camHalfHeight, worldMaxBounds.y - camHalfHeight);

        transform.position = new Vector3(finalPosition.x, finalPosition.y, transform.position.z);
    }

    void ZoomCamera()
    {
        var players = MatchManager.GetAlivePlayers();
        float minX = players.Min(p => p.transform.position.x);
        float maxX = players.Max(p => p.transform.position.x);
        float minY = players.Min(p => p.transform.position.y);
        float maxY = players.Max(p => p.transform.position.y);

        float width = maxX - minX + padding;
        float height = maxY - minY + padding;

        float desiredZoom = Mathf.Max(width / cam.aspect, height) / 2f;
        minZoom = Mathf.Min(padding / cam.orthographicSize / cam.orthographicSize, maxZoom);

        cam.orthographicSize = Mathf.Clamp(Mathf.Lerp(cam.orthographicSize, desiredZoom, Time.deltaTime * zoomSpeed), 0f, maxZoom);
    }
}
