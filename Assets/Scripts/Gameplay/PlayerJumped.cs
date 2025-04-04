using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Services;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player performs a Jump.
    /// </summary>
    /// <typeparam name="PlayerJumped"></typeparam>
    public class PlayerJumped : Simulation.Event<PlayerJumped>
    {
        public IPlayerController player;

        public override void Execute()
        {
            if (player.AudioSource && player.JumpAudio)
                player.AudioSource.PlayOneShot(player.JumpAudio);
        }
    }
}