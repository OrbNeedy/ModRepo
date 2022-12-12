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
        public override bool InstancePerEntity => true;

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (shocked)
            {
                var position = npc.Center - Main.screenPosition + shock.Size()/2;
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
        }
    }
}
