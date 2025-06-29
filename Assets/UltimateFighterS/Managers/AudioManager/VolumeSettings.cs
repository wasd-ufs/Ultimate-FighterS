using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Responsavel por gerenciar o volume geral do jogo
/// </summary>
public class VolumeSettings : MonoBehaviour
{
	[SerializeField] private AudioMixer _audioMixer;
	[SerializeField] private Slider _volumeSlider;
	[SerializeField] private Slider _volumeSFXSlider;

	void Start()
	{
		if (PlayerPrefs.HasKey("MusicVolumeSlider") || PlayerPrefs.HasKey("MusicVolumeSFXSlider"))
		{
			LoadVolume();
		}
	}

	/// <summary>
	/// Responsavel por atualizar o volume da musica de background 
	/// </summary>
	/// <returns>void</returns>
	/// <author>Wallisson</author>
	public void SetVolumeMusic()
	{
		float volume = _volumeSlider.value;
		_audioMixer.SetFloat("MusicVolume",Mathf.Log10(volume) * 20);
		PlayerPrefs.SetFloat("MusicVolumeSlider", volume);
	}

	/// <summary>
	/// Responsavel por atualizar o volume do SFX 
	/// </summary>
	/// <returns>void</returns>
	/// <author>Wallisson</author>
	public void SetVolumeSFX()
	{
		float volume = _volumeSFXSlider.value;
		_audioMixer.SetFloat("MusicVolumeSFX", Mathf.Log10(volume) * 20);
		PlayerPrefs.SetFloat("MusicVolumeSFXSlider", volume);
	}

	/// <summary>
	/// Responsavel por carregar o volume salvo
	/// </summary>
	/// <returns>void</returns>
	/// <author>Wallisson</author>
	private void LoadVolume()
	{
		_volumeSlider.value = PlayerPrefs.GetFloat("MusicVolumeSlider");
		_volumeSFXSlider.value = PlayerPrefs.GetFloat("MusicVolumeSFXSlider");
		SetVolumeMusic();
		SetVolumeSFX();
	}
	
	
}
