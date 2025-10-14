using UnityEngine;
using Zenject;

public class SaveUIControll : MonoBehaviour
{
    [Inject]
    private ILoadable[] loadables;

    [Inject]
    private SaveProvider provider;

    public void Save()
    {
        provider.Save();
    }

    public void Load()
    {
        provider.Load();
        foreach (var loadable in loadables)
        {
            loadable.Load();
        }
    }
}