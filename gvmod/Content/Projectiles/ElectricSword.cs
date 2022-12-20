using gvmod.Common.Players;
using gvmod.Common.Players.Septimas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class ElectricSword : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luxcaliburg");
        }

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.damage = 150;
            Projectile.knockBack = 12;
            Projectile.Size = new Vector2(180, 90);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 2;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            AdeptPlayer adept = Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>();
            if (Projectile.ai[0] == -1)
            {
                Projectile.penetrate = 2;
            }

            switch (Projectile.ai[1])
            {
                case 1:
                    if (adept.isUsingSpecialAbility)
                    {
                        Projectile.timeLeft = 2;
                        Projectile.velocity *= 0.8f;
                    }
                    break;
                default:
                    if (adept.isUsingPrimaryAbility && adept.canUsePrimary)
                    {
                        Projectile.timeLeft = 2;
                    }
                    break;
            }
        }
    }
}
