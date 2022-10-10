using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Common.Players.Septimas;
using gvmod.Common.Players;
using Terraria.DataStructures;

namespace gvmod.Content.Projectiles
{
    public class HairDartProjectile : ModProjectile
    {
        public int septimaSource = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.scale = 1f;
            Projectile.light = 0.1f;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 1;
            Projectile.knockBack = 0;

            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 150;
            Projectile.ownerHitCheck = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            Septima septima = Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>().septima;
            if (septima is AzureStriker)
            {
                AzureStriker azureStriker = (AzureStriker)septima;
                azureStriker.AddMarkedNPC(target);
                return;
            }
            if (septima is AzureThunderclap)
            {
                AzureThunderclap azureThunderclap = (AzureThunderclap)septima;
                azureThunderclap.AddMarkedNPC(target);
                return;
            }
        }
    }
}
