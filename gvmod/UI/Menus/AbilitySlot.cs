using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using gvmod.Common.Players.Septimas.Abilities;
using Terraria.ModLoader;
using gvmod.Common.Players;
using Microsoft.Xna.Framework;

namespace gvmod.UI.Menus
{
    internal class AbilitySlot : UIImageButton
    {
        public Special assignedSpecial { get; set; }
        private int assignedSlot;

        public AbilitySlot(Asset<Texture2D> texture, Special asignedSpecial, int assignedSlot) : base(texture)
        {
            assignedSpecial = asignedSpecial;
            this.assignedSlot = assignedSlot;
        }

        public AbilitySlot(Asset<Texture2D> texture, int assignedSlot) : base(texture)
        {
            assignedSpecial = null;
            this.assignedSlot = assignedSlot;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (IsMouseHovering)
            {
                if (assignedSpecial == null)
                {
                    Main.hoverItemName = "Empty";
                } else
                {
                    Main.hoverItemName = assignedSpecial.Name;
                }
            }
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            if (assignedSpecial != null && assignedSpecial.Name != "")
            {
                if (!assignedSpecial.InCooldown)
                {
                    if (adept.abilityPower < assignedSpecial.ApUsage)
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityActive" + (assignedSlot + 1), AssetRequestMode.ImmediateLoad).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), Color.Red);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Icons/APCost" + assignedSpecial.ApUsage, AssetRequestMode.ImmediateLoad).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityActive" + (assignedSlot + 1), AssetRequestMode.ImmediateLoad).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), Color.White);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Icons/APCost" + assignedSpecial.ApUsage, AssetRequestMode.ImmediateLoad).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), Color.White);
                    }
                    spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Icons/" + assignedSpecial.Name + "Icon", AssetRequestMode.ImmediateLoad).Value, new Vector2(GetInnerDimensions().X + 28, GetInnerDimensions().Y + 18), Color.White);
                }
                else
                {
                    spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityCooldown", AssetRequestMode.ImmediateLoad).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityEmpty" + (assignedSlot + 1), AssetRequestMode.ImmediateLoad).Value, new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y), Color.White);
            }
        }
    }
}
