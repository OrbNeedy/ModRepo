using Terraria;

namespace gvmod.Common.Configs.CustomDataTypes
{
    public class Tag
    {
        public int npcIndex;
        public int level;
        public int timer;
        public bool active;

        public Tag(int npcIndex)
        {
            this.npcIndex = npcIndex;
            level = 1;
            timer = 0;
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
        }

        public void IncreaseMark()
        {
            if (level < 3) level++;
            timer = 0;
        }
    }
}
