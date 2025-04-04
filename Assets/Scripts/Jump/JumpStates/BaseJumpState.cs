using Platformer.Mechanics;
using Platformer.Services;

namespace Platformer.Core
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