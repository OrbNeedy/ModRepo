using gvmod.Common.Players;
using gvmod.Content.Buffs;
using gvmod.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Items.Accessories
{
    public class Glaive : ModItem
    {
        private static Asset<Texture2D> glowmask;
        private Color bodyColor = Color.White;
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Transforms the player into an armed phenomenon, incresing their \n"
                             + "capabilities."); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Load()
        {
            if (!Main.dedServ)
            {
                glowmask = ModContent.Request<Texture2D>("gvmod/Content/Items/Accessories/Glaive_glowmask");
            }
        }

        public override void Unload()
        {
            glowmask = null;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 56;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            bodyColor = player.GetModPlayer<AdeptPlayer>().Septima.MainColor;
            Item.color = bodyColor;
            if (!player.HasBuff<ArmedPhenomenon3>() && !player.HasBuff<ArmedPhenomenon2A>() && !player.HasBuff<ArmedPhenomenon2B>())
            {
                player.AddBuff(ModContent.BuffType<ArmedPhenomenon>(), 2, true);
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            spriteBatch.Draw(
                glowmask.Value,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - glowmask.Value.Height * 0.5f
                ),
                new Rectangle(0, 0, glowmask.Value.Width, glowmask.Value.Height),
                bodyColor,
                rotation,
                glowmask.Value.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<SpiritualStone>(24)
            .AddIngredient(ItemID.IronBroadsword)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe()
            .AddIngredient<SpiritualStone>(24)
            .AddIngredient(ItemID.LeadBroadsword)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
