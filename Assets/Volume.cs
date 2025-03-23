using TMPro;
using UnityEngine;

public class Volume : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    public float percent = 50;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && percent > 0) percent--;

        if (Input.GetKey(KeyCode.RightArrow) && percent < 100) percent++;
        //audioManager.SetGlobalVolume(percent);
        textMeshPro.text = percent.ToString();
    }
}