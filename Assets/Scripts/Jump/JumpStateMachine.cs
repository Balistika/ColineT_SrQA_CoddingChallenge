using UnityEngine;

namespace Platformer.Core
{
    public class JumpStateMachine
    {
        public BaseJumpState CurrentState { get; private set; }

        public void SetState(BaseJumpState newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void Update()
        {
            CurrentState?.Update();
        }
    }
}