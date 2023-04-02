using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using gvmod.Common.Players.Septimas.Skills;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using gvmod.Common.Players;

namespace gvmod.UI.Menus
{
    public class SpecialOption : UIImageButton
    {
        public int specialIndex { get; set; }
        public Special assignedSpecial { get; set; }
        private Texture2D icon;

        public SpecialOption(Asset<Texture2D> texture, int assignedSpecial) : base(texture)
        {
            specialIndex = assignedSpecial;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(icon, GetDimensions().ToRectangle(), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            assignedSpecial = adept.Septima.Abilities[specialIndex];
            if (assignedSpecial != null)
            {
                icon = ModContent.Request<Texture2D>("gvmod/Assets/Icons/" + assignedSpecial.Name + "Icon").Value;
            } else
            {
                icon = ModContent.Request<Texture2D>("gvmod/Assets/Icons/EmptyIcon").Value;
            }
        }
    }
}
