using Platformer.Mechanics;
using Zenject;

namespace Platformer.Services
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var input = new InputController();
            var playerController = FindFirstObjectByType<PlayerController>();
            playerController.Setup(input);

            Container.Bind<IInputController>().FromInstance(input).AsSingle();
            Container.Bind<IPlayerController>().FromInstance(playerController).AsSingle();
        }
    }
}