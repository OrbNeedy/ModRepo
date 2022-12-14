using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Abilities
{
    public class None : Special
    {
        public None(Player player, AdeptPlayer adept) : base(player, adept)
        {
        }

        public override int UnlockLevel => 0;

        public override string Name => "None";

        public override bool IsOffensive => false;

        public override bool GivesIFrames => false;

        public override void Attack()
        {
        }

        public override void Effects()
        {
        }

        public override void Update()
        {
        }
    }
}
