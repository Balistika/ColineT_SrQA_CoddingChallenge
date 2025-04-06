using Platformer.Mechanics;
using Platformer.Services;
using UnityEngine;

namespace Tests.Mocks
{
    // === MOCKS ===
    public class MockPlayerController : IPlayerController
    {
        public bool IsGrounded { get; set; }
        public IInputController InputController { get; set; }
        public JumpStateMachine JumpStateMachine { get; set; }

        public void TriggerStopJump() {}
        public void ResetJumpState() {}
        public void EnableControl(bool enabled) {}
        public void ApplyJumpImpulse() {}
        public Bounds Bounds => new Bounds(Vector3.zero, Vector3.one);
        public AudioSource AudioSource => null;
        public AudioClip JumpAudio => null;
    }
    
    public class MockInputController : IInputController
    {
        public bool JumpPressed = false;
        public bool jumpReleased = false;

        public float GetHorizontal() => 0f;
        public bool IsJumpPressed() => JumpPressed;
        public bool IsJumpReleased() => jumpReleased;
    }
}