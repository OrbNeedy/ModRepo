using Terraria;

namespace gvmod.Common.Configs.CustomDataTypes
{
    public class Tag
    {
        public int npcIndex { get; set; }
        public int level { get; set; }
        public int timer { get; set; }
        public bool active { get; set; }
        public int shockIframes { get; set; }

        public Tag(int npcIndex)
        {
            this.npcIndex = npcIndex;
            level = 1;
            timer = 0;
            shockIframes = 0;
            active = true;
        }

        public void Update()
        {
            timer++;
            NPC theNpcInQuestion = Main.npc[npcIndex];
            if (!theNpcInQuestion.active || theNpcInQuestion.life <= 0 || timer >= 600)
            {
                active = false;
            }
            if (shockIframes > 0)
            {
                shockIframes--;
            }
        }

        public void IncreaseMark()
        {
            if (level < 3) level++;
            timer = 0;
        }
    }
}
