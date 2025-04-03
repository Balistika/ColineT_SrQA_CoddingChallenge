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
    }
}