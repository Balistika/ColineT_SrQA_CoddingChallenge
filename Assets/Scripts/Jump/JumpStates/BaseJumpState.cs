using Platformer.Mechanics;

namespace Platformer.Core
{
    public abstract class BaseJumpState
    {
        protected readonly PlayerController playerController;
        protected readonly JumpStateMachine jumpStateMachine;
        
        public BaseJumpState(PlayerController playerController, JumpStateMachine jumpStateMachine)
        {
            this.playerController = playerController;
            this.jumpStateMachine = jumpStateMachine;
        }
        
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}