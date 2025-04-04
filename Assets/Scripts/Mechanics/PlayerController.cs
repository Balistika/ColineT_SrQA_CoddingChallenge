using UnityEngine;
using Platformer.Model;
using Platformer.Core;
using Platformer.Services;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject, IPlayerController
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;
        
        public Collider2D collider2d;
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;
        
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;
        public void EnableControl(bool enabled) => controlEnabled = enabled;
        
        private IInputController inputController;
        private JumpStateMachine jumpStateMachine;

        public IInputController InputController => inputController;
        public JumpStateMachine JumpStateMachine => jumpStateMachine;
        
        public AudioSource AudioSource => audioSource;
        public AudioClip JumpAudio => jumpAudio;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        public void Setup(IInputController input)
        {
            inputController = input;
        }

        void Start()
        {
            jumpStateMachine = new JumpStateMachine();
            ResetJumpState();
        }
        
        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = inputController.GetHorizontal();
            }
            else
            {
                move.x = 0;
            }
            
            jumpStateMachine.Update();
            base.Update();
        }

        protected override void ComputeVelocity()
        {
            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }
        
        public void ResetJumpState()
        {
            jumpStateMachine.SetState(new GroundedState(this));
        }
        
        public void TriggerStopJump()
        {
            if (velocity.y > 0)
            {
                velocity.y *= model.jumpDeceleration;
            }
        }

        public void ApplyJumpImpulse()
        {
            velocity.y = jumpTakeOffSpeed * model.jumpModifier;
        }
    }
}