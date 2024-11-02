using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] players;
    public Vector2 worldMinBounds, worldMaxBounds;
    public float minZoom = 10f;
    public float maxZoom = 17f;
    public float zoomSpeed = 5f;
    public float padding = 15f;
    public float centerWeight = 0.3f;

    private Camera cam;
    private Vector2 levelCenter;

    void Start()
    {
        cam = GetComponent<Camera>();

        levelCenter = (worldMinBounds + worldMaxBounds) / 2f;
    }

    void LateUpdate()
    {
        players = players.Where(p => p != null).ToArray();

        if (players.Length == 0)
            return;

        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {
        Vector3 averagePosition = Vector3.zero;
        foreach (Transform player in players)
        {
            averagePosition += player.position;
        }
        averagePosition /= players.Length;

        Vector3 finalPosition = Vector3.Lerp(averagePosition, levelCenter, centerWeight);
                
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        finalPosition.x = Mathf.Clamp(finalPosition.x, worldMinBounds.x + camHalfWidth, worldMaxBounds.x - camHalfWidth);
        finalPosition.y = Mathf.Clamp(finalPosition.y, worldMinBounds.y + camHalfHeight, worldMaxBounds.y - camHalfHeight);

        transform.position = new Vector3(finalPosition.x, finalPosition.y, transform.position.z);
    }

    void ZoomCamera()
    {
        float minX = players.Min(p => p.position.x);
        float maxX = players.Max(p => p.position.x);
        float minY = players.Min(p => p.position.y);
        float maxY = players.Max(p => p.position.y);

        float width = maxX - minX + padding;
        float height = maxY - minY + padding;

        float desiredZoom = Mathf.Max(width / cam.aspect, height) / 2f;

        cam.orthographicSize = Mathf.Clamp(Mathf.Lerp(cam.orthographicSize, desiredZoom, Time.deltaTime * zoomSpeed), minZoom, maxZoom);
    }
}
