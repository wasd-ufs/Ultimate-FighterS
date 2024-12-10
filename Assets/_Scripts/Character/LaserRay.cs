using System.Linq;
using UnityEngine;

public class LaserRay : MonoBehaviour
{
    private float minimumSize = 0.1f;
    private float maximumSize = 100f;
    public GameObject shootPoint;

    private float DetermineSize()
    {
        RaycastHit2D[] hits;
        RaycastHit2D hit;
        hits = Physics2D.RaycastAll(shootPoint.transform.position, new Vector2(shootPoint.transform.lossyScale.x, 0), maximumSize);
        hit = hits.FirstOrDefault(hitTemp => !hitTemp.transform.CompareTag("Laser"));
        if(hit == null) return maximumSize;
        float size = Mathf.Max(hit.distance, minimumSize);
        return size;
    }

    private void FixedUpdate()
    {
        transform.localPosition = new Vector3(DetermineSize()/2, 0, 0);
        transform.localScale = new Vector3(DetermineSize(), 0.1f, 1);
    }
}
