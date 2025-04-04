using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Services;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerLanded"></typeparam>
    public class PlayerLanded : Simulation.Event<PlayerLanded>
    {
        public IPlayerController player;

        public override void Execute()
        {

        }
    }
}