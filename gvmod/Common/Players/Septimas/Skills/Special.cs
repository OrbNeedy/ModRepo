﻿using Microsoft.Xna.Framework;
using Terraria;

namespace gvmod.Common.Players.Septimas.Skills
{
    public abstract class Special
    {
        public Special(Player player, AdeptPlayer adept, string type)
        {
            Player = player;
            Adept = adept;
            VelocityMultiplier = new Vector2(1f, 1f);
            Type = type;
        }

        public int SpecialTimer { get; set; }

        public int CooldownTimer { get; set; }

        public int SpecialDuration { get; set; }

        public Player Player { get; }

        public AdeptPlayer Adept { get; }

        public string Type { get; set; }

        public abstract int UnlockLevel { get; }

        public float ApUsage { get; set; }

        public abstract string Name { get; }

        public abstract bool IsOffensive { get; }

        public abstract bool StayInPlace { get; }

        public abstract bool GivesIFrames { get; }

        public int SpecialCooldownTime { get; set; }

        public bool InCooldown { get; set; }

        public bool BeingUsed { get; set; }

        public Vector2 VelocityMultiplier { get; set; }

        public virtual void StatChangeEffects()
        {

        }

        public virtual void Effects()
        {
        }

        public virtual void MoveOverride()
        {
        }

        public abstract void Attack();

        public abstract void Update();
    }
}
