using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using gvmod.Common.Players.Septimas.Abilities;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace gvmod.UI.Menus
{
    internal class SelectionOption : UIImageButton
    {
        public Special assignedSpecial { get; set; }

        public SelectionOption(Asset<Texture2D> texture, Special asignedSpecial) : base(texture)
        {
            assignedSpecial = asignedSpecial;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (assignedSpecial == null || assignedSpecial.Name == "")
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Icons/None", AssetRequestMode.ImmediateLoad).Value, GetDimensions().ToRectangle(), Color.White);
            } else
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Icons/" + assignedSpecial.Name + "Icon", AssetRequestMode.ImmediateLoad).Value, GetDimensions().ToRectangle(), Color.White);
            }
        }
    }
}
