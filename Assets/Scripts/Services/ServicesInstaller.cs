using Platformer.Mechanics;
using UnityEngine;
using Zenject;

namespace Platformer.Services
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] 
        private PlayerController playerController;

        public override void InstallBindings()
        {
            var input = new InputController();
            playerController.Setup(input);

            Container.Bind<IInputController>().FromInstance(input).AsSingle();
            Container.Bind<IPlayerController>().FromInstance(playerController).AsSingle();
        }
    }
}