using NUnit.Framework;
using Platformer.Mechanics;
using Tests.Mocks;

namespace Tests.Editor
{
    [TestFixture]
    public class PlayerJumpStateMachine_UnitTest
    {
        private MockPlayerController playerMock;
        private MockInputController inputMock;
        private JumpStateMachine jumpStateMachine;

        [SetUp]
        public void SetUp()
        {
            inputMock = new MockInputController();
            playerMock = new MockPlayerController
            {
                InputController = inputMock,
                IsGrounded = true,
                JumpStateMachine = new JumpStateMachine()
            };

            jumpStateMachine = playerMock.JumpStateMachine;
        }

        [Test]
        public void StartsInGrounded()
        {
            jumpStateMachine.SetState(new GroundedState(playerMock));
            Assert.IsInstanceOf<GroundedState>(jumpStateMachine.CurrentState);
        }

        [Test]
        public void TransitionsToPrepareToJump_WhenJumpPressed()
        {
            inputMock.JumpPressed = true;
            jumpStateMachine.SetState(new GroundedState(playerMock));
            jumpStateMachine.Update();
            Assert.IsInstanceOf<PrepareToJumpState>(jumpStateMachine.CurrentState);
        }

        [Test]
        public void TransitionsToJumping_FromPrepareToJump()
        {
            jumpStateMachine.SetState(new PrepareToJumpState(playerMock));
            jumpStateMachine.Update();
            Assert.IsInstanceOf<JumpingState>(jumpStateMachine.CurrentState);
        }

        [Test]
        public void TransitionsToInFlight_FromJumping_WhenNotGrounded()
        {
            playerMock.IsGrounded = false;
            jumpStateMachine.SetState(new JumpingState(playerMock));
            jumpStateMachine.Update();
            Assert.IsInstanceOf<InFlightState>(jumpStateMachine.CurrentState);
        }

        [Test]
        public void TransitionsToLanded_FromInFlight_WhenGrounded()
        {
            jumpStateMachine.SetState(new InFlightState(playerMock));
            playerMock.IsGrounded = true;
            jumpStateMachine.Update();
            Assert.IsInstanceOf<LandedState>(jumpStateMachine.CurrentState);
        }

        [Test]
        public void TransitionsToGrounded_FromLanded()
        {
            jumpStateMachine.SetState(new LandedState(playerMock));
            jumpStateMachine.Update();
            Assert.IsInstanceOf<GroundedState>(jumpStateMachine.CurrentState);
        }
    }
}