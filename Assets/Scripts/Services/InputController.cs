using UnityEngine;

namespace Platformer.Services
{
    public interface IInputController
    {
        float GetHorizontal();
        bool IsJumpPressed();
        bool IsJumpReleased();
    }
    
    public class InputController : IInputController
    {
        public float GetHorizontal() => Input.GetAxis("Horizontal");
        public bool IsJumpPressed() => Input.GetButtonDown("Jump");
        public bool IsJumpReleased() => Input.GetButtonUp("Jump");
    }
}