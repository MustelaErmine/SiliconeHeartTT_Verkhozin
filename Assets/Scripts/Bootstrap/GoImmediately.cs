using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GoImmediately : IInitializable
{
    string _nextScene;
    public GoImmediately(Settings settings)
    {
        _nextScene = settings.nextScene;
    }
    public void Initialize()
    {
        SceneManager.LoadScene(_nextScene);
    }
    public struct Settings 
    {
        public string nextScene;
    }
}
