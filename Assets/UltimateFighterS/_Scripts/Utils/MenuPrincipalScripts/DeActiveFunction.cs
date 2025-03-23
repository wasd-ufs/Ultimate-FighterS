using UnityEngine;

public class DeActiveFunction : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) gameObject.SetActive(false);
    }
}