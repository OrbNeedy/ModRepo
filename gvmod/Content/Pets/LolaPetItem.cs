using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using gvmod.Common.Players;

namespace gvmod.Content.Pets
{
    public class LolaPetItem : ModItem
    {
        private static Asset<Texture2D> exterior;
        private Color bodyColor = Color.White;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Battle pod");
            Tooltip.SetDefault("She will drink your oil and call you boss.");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                exterior = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPetItemColorChange");
            }
        }

        public override void Unload()
        {
            exterior = null;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<LolaPetProjectile>(), ModContent.BuffType<LolaPetBuff>());

            Item.width = 38;
            Item.height = 38;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(0, 0, 50);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            return false;
        }

        public override void UpdateInventory(Player player)
        {
            bodyColor = player.GetModPlayer<AdeptPlayer>().Septima.MainColor;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            bodyColor = player.GetModPlayer<AdeptPlayer>().Septima.MainColor;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(
                exterior.Value,
                position,
                frame,
                bodyColor,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0
            );
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            spriteBatch.Draw(
                exterior.Value,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - exterior.Value.Height * 0.5f
                ),
                new Rectangle(0, 0, exterior.Value.Width, exterior.Value.Height),
                Lighting.GetColor(Item.Center.ToTileCoordinates(), bodyColor),
                rotation,
                exterior.Value.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
