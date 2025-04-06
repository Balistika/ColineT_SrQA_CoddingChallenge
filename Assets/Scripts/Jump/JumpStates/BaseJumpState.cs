using Platformer.Services;

namespace Platformer.Mechanics
{
    public abstract class BaseJumpState
    {
        protected readonly IPlayerController playerController;
        
        public BaseJumpState(IPlayerController playerController)
        {
            this.playerController = playerController;
        }
        
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}