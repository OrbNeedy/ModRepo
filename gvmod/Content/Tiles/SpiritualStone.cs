using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Tiles
{
    public class SpiritualStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = false;
            Main.tileOreFinderPriority[Type] = 280;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 255;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = false;

            TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(152, 171, 198), name);

            DustType = 84;
            HitSound = SoundID.Tink;
        }
    }
}
