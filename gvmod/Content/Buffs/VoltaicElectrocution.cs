using Terraria;
using Terraria.ModLoader;
using gvmod.Common.Players;
using gvmod.Content.Buffs;

namespace gvmod.Content.Buffs
{
    internal class VoltaicElectrocution: ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 600;
        }
    }
}
