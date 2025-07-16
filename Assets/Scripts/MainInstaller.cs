using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var _sceneController = gameObject.AddComponent<SceneController>();
        Container.Bind<SceneController>().FromInstance(_sceneController).AsSingle();
    }
}
