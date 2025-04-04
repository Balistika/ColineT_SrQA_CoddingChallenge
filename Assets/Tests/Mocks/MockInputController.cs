using Platformer.Services;

namespace Tests.Mocks
{
    public class MockInputController : IInputController
    {
        public bool JumpPressed = false;
        public bool jumpReleased = false;

        public float GetHorizontal() => 0f;
        public bool IsJumpPressed() => JumpPressed;
        public bool IsJumpReleased() => jumpReleased;
    }
}