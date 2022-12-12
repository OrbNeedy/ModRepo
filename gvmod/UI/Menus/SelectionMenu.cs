using gvmod.Common.Players.Septimas.Abilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace gvmod.UI.Menus
{
    public class SelectionMenu : UIPanel
    {
        private Color color = Color.White;
        private Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("gvmod/Assets/Menus/SelectionMenuBack",
            ReLogic.Content.AssetRequestMode.ImmediateLoad);
        private List<Special> posibleSpecials = new List<Special>();

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Left.Pixels, (int)Top.Pixels, 150, 60), color);
        }
    }
}
