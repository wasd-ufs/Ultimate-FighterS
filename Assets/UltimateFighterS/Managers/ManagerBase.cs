using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe base para gerenciadores que utilizam o padrao singleton
/// </summary>
public abstract class ManagerBase <T> : MonoBehaviour where T : ManagerBase<T>
{
	private static T _instance;
	
	public static T Instance => _instance;

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
			DontDestroyOnLoad(_instance);
		}
		
		Destroy(gameObject);
	}

}
