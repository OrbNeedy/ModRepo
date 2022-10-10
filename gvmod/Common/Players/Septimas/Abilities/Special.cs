using Microsoft.Xna.Framework;
using Terraria;

namespace gvmod.Common.Players.Septimas.Abilities
{
    public abstract class Special
    {
        private Player player;
        private AdeptPlayer adept;
        private float apUsage;
        private int specialCooldownTime;
        private Vector2 velocityMultiplier;
        private bool inCooldown;
        private bool beingUsed;

        public Special(Player player, AdeptPlayer adept)
        {
            this.player = player;
            this.adept = adept;
            this.velocityMultiplier = new Vector2(1f, 1f);
        }

        public int SpecialTimer { get; set; }

        public int CooldownTimer { get; set; }

        public Player Player { get => player; }

        public AdeptPlayer Adept { get => adept; }

        public abstract int UnlockLevel { get; }

        public float ApUsage { get => apUsage; set => apUsage = value; }

        public abstract string Name { get; }

        public int SpecialCooldownTime { get => specialCooldownTime; set => specialCooldownTime = value; }

        public bool InCooldown { get => inCooldown; set => inCooldown = value; }

        public bool BeingUsed { get => beingUsed; set => beingUsed = value; }

        public Vector2 VelocityMultiplier { get => velocityMultiplier; set => velocityMultiplier = value; }

        public abstract void Effects();

        public abstract void Attack();

        public abstract void Update();
    }
}
