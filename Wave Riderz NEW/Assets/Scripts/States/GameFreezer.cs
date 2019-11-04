using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFreezer : MonoBehaviour
{
	public static GameFreezer instance;
	private static float m_duration;
	private bool m_frozen = false;

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		if (m_duration > 0 && !m_frozen)
		{
			StartCoroutine(FreezeCoroutine());
		}
	}

	public static void Freeze(float duration)
	{
		m_duration = duration;
	}

	IEnumerator FreezeCoroutine()
	{
		m_frozen = true;
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(m_duration);
		Time.timeScale = 1;
		m_duration = 0;
		m_frozen = false;
	}
}
