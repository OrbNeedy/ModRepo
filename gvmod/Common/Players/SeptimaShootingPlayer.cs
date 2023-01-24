using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class SeptimaShootingPlayer : ModPlayer
    {
        public bool ShootsBolts { get; set; }
        private int boltTimer;

        public override void Initialize()
        {
            ShootsBolts = false;
        }

        public override void ResetEffects()
        {
            ShootsBolts = false;
        }

        public override void PostUpdate()
        {
            if (boltTimer > 0) boltTimer--;
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if (ShootsBolts && boltTimer <= 0)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1, 1).RotatedByRandom(MathHelper.ToRadians(360))*10, ModContent.ProjectileType<SeptimaBolt>(), 36, 10, Player.whoAmI);
                boltTimer = 20;
            }
        }
    }
}
