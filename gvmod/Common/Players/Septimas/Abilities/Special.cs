using Microsoft.Xna.Framework;
using Terraria;

namespace gvmod.Common.Players.Septimas.Abilities
{
    public abstract class Special
    {
        public Special(Player player, AdeptPlayer adept)
        {
            Player = player;
            Adept = adept;
            VelocityMultiplier = new Vector2(1f, 1f);
        }

        public int SpecialTimer { get; set; }

        public int CooldownTimer { get; set; }

        public int SpecialDuration { get; set; }

        public Player Player { get; }

        public AdeptPlayer Adept { get; }

        public abstract int UnlockLevel { get; }

        public float ApUsage { get; set; }

        public abstract string Name { get; }

        public abstract bool IsOffensive { get; }

        public abstract bool GivesIFrames { get; }

        public int SpecialCooldownTime { get; set; }

        public bool InCooldown { get; set; }

        public bool BeingUsed { get; set; }

        public Vector2 VelocityMultiplier { get; set; }

        public abstract void Effects();

        public abstract void Attack();

        public abstract void Update();
    }
}
