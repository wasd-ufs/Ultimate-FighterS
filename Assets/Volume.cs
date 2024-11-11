using TMPro;
using UnityEngine;

public class Volume : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;
    private AudioManager audioManager;
    public float percent = 50;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftArrow) && percent > 0)
        {
            percent--;
        }

        if (Input.GetKey(KeyCode.RightArrow) && percent < 100)
        {
            percent++;
        }
        audioManager.SetGlobalVolume(percent);
        textMeshPro.text = percent.ToString();
    }
}
