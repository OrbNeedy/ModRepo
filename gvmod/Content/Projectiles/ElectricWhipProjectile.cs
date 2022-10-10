using Terraria.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace gvmod.Content.Projectiles
{
    public class ElectricWhipProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.WhipSettings.Segments = 20;
            Projectile.WhipSettings.RangeMultiplier = .5f;
        }
		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * Timer;
			Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;

			Timer++;

			float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;

			if (Timer >= swingTime || owner.itemAnimation <= 0)
			{
				Projectile.Kill();
				return;
			}

			owner.heldProj = Projectile.whoAmI;

			owner.itemAnimation = owner.itemAnimationMax - (int)(Timer / Projectile.MaxUpdates);
			owner.itemTime = owner.itemAnimation;

			if (Timer == swingTime / 2)
			{
				List<Vector2> points = Projectile.WhipPointsForCollision;
				Projectile.FillWhipControlPoints(Projectile, points);
				SoundEngine.PlaySound(SoundID.Item153, points[points.Count - 1]);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Electrified, 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}

		private void DrawLine(List<Vector2> list)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 1; i++)
			{
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.White);
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);

			DrawLine(list);

			Main.DrawWhip_WhipBland(Projectile, list);

			return false;
		}
	}
}