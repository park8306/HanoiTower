using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T :MonoBehaviour
{
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<T>();
                DontDestroyOnLoad(m_instance.gameObject);
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        if (m_instance != null)
        {
            if(m_instance != this) Destroy(gameObject);

            return;
        }

        m_instance = GetComponent<T>();

        DontDestroyOnLoad(gameObject);
    }
}
