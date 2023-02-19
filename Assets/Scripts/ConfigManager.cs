using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    [Header("Default Values")]
    [SerializeField]
    float respawnTime = 5f;
    [SerializeField]
    float matchDuration = 120;

    private float respawn;
    private float duration;

    public static ConfigManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        respawn = respawnTime;
        duration = matchDuration;
    }

    public float GetMatchDuration() { 
        return duration; 
    }
    public float GetRespawnTime() { 
        return respawn; 
    }
    
    public string SetRespawnTime(string str)
    {
        float value;
        bool result = float.TryParse(str, out value);
        if (result)
            respawn = Mathf.Clamp(value, 0, 1000);
        return respawn.ToString();
    }

    public string SetMatchDuration(string str)
    {
        float value;
        bool result = float.TryParse(str, out value);
        if (result)
            duration = Mathf.Clamp(value, 60, 180);
        return duration.ToString();
    }

}
