using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static bool _isFirstLaunch = true;

    public static bool IsFirstLaunch
    {
        get { return _isFirstLaunch; }
        set { _isFirstLaunch = value; }
    }
    
    public static GameState Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log(IsFirstLaunch);
    }
}

