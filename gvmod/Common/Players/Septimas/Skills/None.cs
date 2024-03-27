using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class None : Special
    {
        public None(Player player, AdeptPlayer adept, string type="") : base(player, adept, type)
        {
        }

        public override int UnlockLevel => 0;

        public override string Name => "None";

        public override bool IsOffensive => false;

        public override bool StayInPlace => false;

        public override bool GivesIFrames => false;

        public override void Attack()
        {
        }

        public override void Update()
        {
        }
    }
}
