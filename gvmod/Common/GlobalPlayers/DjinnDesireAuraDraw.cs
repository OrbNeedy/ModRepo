using gvmod.Common.Players;
using gvmod.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalPlayers
{
    public class DjinnDesireAuraDraw : PlayerDrawLayer
    {
        private Asset<Texture2D> djinnDesireAura = ModContent.Request<Texture2D>("gvmod/Assets/Effects/DjinnDesire");
        private int rotation = 0;
        public override Position GetDefaultPosition()
        {
            return new Between(PlayerDrawLayers.WebbedDebuffBack, PlayerDrawLayers.LeinforsHairShampoo);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.HasBuff(ModContent.BuffType<AnthemBuff>()) && drawInfo.drawPlayer.GetModPlayer<AdeptPlayer>().AnthemLevel == 5)
            {
                var position = drawInfo.Center - Main.screenPosition;
                position = new Vector2((int)position.X, (int)position.Y);
                drawInfo.DrawDataCache.Add(new DrawData(
                    djinnDesireAura.Value,
                    position,
                    null,
                    Color.White * 0.3f,
                    MathHelper.ToRadians(rotation),
                    djinnDesireAura.Size() * 0.5f,
                    1f,
                    SpriteEffects.None,
                    0
                ));
                Rotate();
            } else
            {
                rotation = 0;
            }
        }

        private void Rotate()
        {
            rotation++;
            if (rotation >= 361)
            {
                rotation = 0;
            }
        }
    }
}
