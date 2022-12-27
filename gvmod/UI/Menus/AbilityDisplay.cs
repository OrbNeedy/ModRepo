using Microsoft.Xna.Framework.Graphics;
using Terraria;
using ReLogic.Content;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using gvmod.Common.Players;
using gvmod.Common.Players.Septimas.Abilities;

namespace gvmod.UI.Menus
{
    internal class AbilityDisplay : UIImageButton
    {
        public int specialIndex { get; set; }
        public Special assignedSpecial { get; set; }
        private int assignedSlot;
        private Texture2D activeAbility;
        private Texture2D[] apCost = new Texture2D[4];
        private Texture2D empty;
        private Texture2D cooldown;
        private Texture2D icon;
        private Color color;

        public AbilityDisplay(Asset<Texture2D> texture, int slot) : base(texture)
        {
            color = Color.White;

            assignedSlot = slot;

            specialIndex = 0;

            empty = texture.Value;

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (IsMouseHovering)
            {
                if (assignedSpecial == null)
                {
                    Main.hoverItemName = "Empty";
                }
                else
                {
                    Main.hoverItemName = assignedSpecial.Name;
                }
            }
            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            if (assignedSpecial != null && assignedSpecial is not None)
            {
                if (!assignedSpecial.InCooldown)
                {
                    if (adept.AbilityPower < assignedSpecial.ApUsage)
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityActive" + (assignedSlot + 1)).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), color);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Icons/APCost" + assignedSpecial.ApUsage).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), color);
                    }
                    else
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityActive" + (assignedSlot + 1)).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), color);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Icons/APCost" + assignedSpecial.ApUsage).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), color);
                    }
                    spriteBatch.Draw(icon, new Vector2(GetInnerDimensions().X + 28, GetInnerDimensions().Y + 18), Color.White);
                }
                else
                {
                    spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityCooldown").Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(empty, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            assignedSpecial = adept.Septima.Abilities[specialIndex];
            if (assignedSpecial != null)
            {
                icon = ModContent.Request<Texture2D>("gvmod/Assets/Icons/" + assignedSpecial.Name + "Icon").Value;
                activeAbility = ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityActive" + (assignedSlot + 1)).Value;
                cooldown = ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityCooldown").Value;
                for (int i = 0; i < 4; i++)
                {
                    apCost[i] = ModContent.Request<Texture2D>("gvmod/Assets/Icons/APCost" + i).Value;
                }

                if (adept.AbilityPower < assignedSpecial.ApUsage)
                {
                    color = Color.Red;
                }
                else
                {
                    color = Color.White;
                }
            }
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            if (assignedSpecial == null || assignedSpecial is None)
            {
                Main.hoverItemName = "Empty";
            }
            else
            {
                Main.hoverItemName = assignedSpecial.Name;
            }
        }
    }
}
