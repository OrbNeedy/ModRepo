using gvmod.Common.Players;
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
        private int aiForm = 1;
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
            Projectile.timeLeft = 1;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            switch (aiForm)
            {
                case 0:
                    Projectile.timeLeft = 150;
                    aiForm = -1;
                    break;
                case 1:
                    Projectile.direction = player.direction;
                    Projectile.spriteDirection = player.direction;
                    Projectile.timeLeft = 2;
                    aiForm = -1;
                    break;
            }
        }

        public override void PostDraw(Color lightColor)
        {
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Player player = Main.player[Projectile.owner];
            if (source == player.GetSource_FromThis())
            {
                if (player.GetModPlayer<AdeptPlayer>().septima.Name == "Azure Striker" || player.GetModPlayer<AdeptPlayer>().septima.Name == "Azure Thunderclap")
                {
                    aiForm = 1;
                }
                else
                {
                    aiForm = 0;
                }
            }
        }
    }
}
