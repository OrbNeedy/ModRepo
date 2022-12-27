using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using gvmod.Content.Projectiles;

namespace gvmod.Common.GlobalProjectiles
{
    internal class GlobalVasukiEffect : GlobalProjectile
    {
        public int SourceNPC { get; set; }
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (projectile.ModProjectile is HairDartProjectile || projectile.ModProjectile is DullahanProjectile)
            {

            }
        }
    }
}
