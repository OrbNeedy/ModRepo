using gvmod.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalPlayers
{
    public class AnthemAuraDraw : PlayerDrawLayer
    {
        private Asset<Texture2D> anthemAura = ModContent.Request<Texture2D>("gvmod/Assets/Effects/Anthem");
        public override Position GetDefaultPosition()
        {
            return new Between(PlayerDrawLayers.CaptureTheGem, PlayerDrawLayers.BeetleBuff);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.HasBuff(ModContent.BuffType<AnthemBuff>()))
            {
                var position = drawInfo.Center - Main.screenPosition;
                position = new Vector2((int)position.X, (int)position.Y);
                drawInfo.DrawDataCache.Add(new DrawData(
                    anthemAura.Value,
                    position,
                    null,
                    Color.White * 0.25f,
                    0f,
                    anthemAura.Size() * 0.6f,
                    1f,
                    SpriteEffects.None,
                    0
                ));
            }
        }
    }
}
