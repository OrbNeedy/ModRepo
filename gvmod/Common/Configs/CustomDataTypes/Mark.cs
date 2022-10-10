using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Configs.CustomDataTypes
{
    public class Mark
    {
        public NPC npc;
        public int level;
        public int timer;
        public bool active;

        public Mark(NPC npc)
        {
            this.npc = npc;
            level = 1;
            timer = 0;
            active = true;
        }

        public void Update()
        {
            timer++;
            if (!npc.active || npc.life <= 0 || timer >= 600)
            {
                active = false;
            }
        }

        public void IncreaseMark()
        {
            if (level < 3) level++;
            timer = 0;
        }
    }
}
