using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace gvmod.Common.GlobalNPCs
{
    public class TaggedNPC : GlobalNPC
    {
        private Asset<Texture2D> shock = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/Shock");
        public int tagLevel { get; set; }
        public bool shocked { get; set; }
        private int rotation;
        public override bool InstancePerEntity => true;

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (shocked)
            {
                var position = npc.Center - Main.screenPosition + shock.Size() / 2;
                position = new Vector2((int)position.X, (int)position.Y);
                spriteBatch.Draw(
                    shock.Value,
                    position,
                    null,
                    Color.White * 0.8f,
                    0f,
                    shock.Size(),
                    1f,
                    SpriteEffects.None,
                    0
                );
                Lighting.AddLight(position, 255, 255, 255);
            }

            if (tagLevel > 0)
            {
                if (tagLevel == 2)
                {
                    Rotate();
                } else if (tagLevel == 3)
                {
                    ReverseRotate();
                } else {
                    rotation = 0;
                }
                Asset<Texture2D> tag = ModContent.Request<Texture2D>("gvmod/Assets/Effects/Tag" + tagLevel);
                var position = npc.Center - Main.screenPosition;
                position = new Vector2((int)position.X, (int)position.Y);
                spriteBatch.Draw(
                    tag.Value,
                    position,
                    null,
                    Color.White * 0.5f,
                    MathHelper.ToRadians(rotation),
                    tag.Size()*0.5f,
                    1f,
                    SpriteEffects.None,
                    0
                );
                Lighting.AddLight(position, 255, 255, 255);
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

        private void ReverseRotate()
        {
            rotation -= 2;
            if (rotation <= -361)
            {
                rotation = 0;
            }
        }
    }
}
