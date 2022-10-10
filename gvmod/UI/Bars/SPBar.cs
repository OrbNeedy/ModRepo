using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace gvmod.UI.Bars
{
    public class SPBar : UIState
    {
        public int x = (int)(Main.screenWidth * 0.55f);
        public int y = (int)(Main.screenHeight * 0.02f);
        public SPBarBack spBarBack;
        public UIImage spBarFill;
        public UIText text;
        public Texture2D filling = ModContent.Request<Texture2D>("gvmod/Assets/Bars/SPBarBody", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public override void OnInitialize()
        {
            spBarBack = new SPBarBack();
            spBarBack.Left.Set(x, 0f);
            spBarBack.Top.Set(y, 0f);
            spBarBack.Width.Set(120, 0f);
            spBarBack.Height.Set(30, 0f);

            spBarFill = new UIImage(ModContent.Request<Texture2D>("gvmod/Assets/Bars/SPBarBody", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            spBarFill.Left.Set(x + 4, 0);
            spBarFill.Top.Set(y, 0);
            spBarFill.Width.Set(112, 0f);
            spBarFill.Height.Set(30, 0f);

            text = new UIText("0%", 0.8f);
            text.Width.Set(x + 130, 0f);
            text.Height.Set(y + 15, 0f);
            text.Top.Set(y + 10, 0f);
            text.Left.Set(x, 0f);

            spBarBack.Append(spBarFill);
            spBarBack.Append(text);
            Append(spBarBack);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            if (adept.septima.Name == "Human") return;

            if (adept.isOverheated)
            {
                spriteBatch.Draw(filling, new Rectangle((int)spBarBack.Left.Pixels + 4, (int)spBarBack.Top.Pixels, (int)(adept.SeptimalPowerToFraction() * 112), 30), Color.Red);
            } else
            {
                spriteBatch.Draw(filling, new Rectangle((int)spBarBack.Left.Pixels + 4, (int)spBarBack.Top.Pixels, (int)(adept.SeptimalPowerToFraction() * 112), 30), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            text.SetText(adept.SeptimalPowerToFraction()*100 + "%");
            if (IsMouseHovering)
            {
                Main.hoverItemName = adept.septimalPower.ToString();
            }
            text.SetText("SP: " + (int)adept.septimalPower + "/" + (int)adept.maxSeptimalPower);
        }
    }
}
