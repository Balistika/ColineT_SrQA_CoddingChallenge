using Platformer.Core;
using UnityEngine;

namespace Platformer.Services
{
    public interface IPlayerController
    {
        Bounds Bounds { get; }

        bool IsGrounded { get; }
        
        void EnableControl(bool enabled);

        void ResetJumpState();

        void TriggerStopJump();

        void ApplyJumpImpulse();
        
        IInputController InputController { get; }
        
        JumpStateMachine  JumpStateMachine { get; }
        
        public AudioSource AudioSource { get; }
        
        public AudioClip JumpAudio { get; }
    }
}