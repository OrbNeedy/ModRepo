using System.Collections.Generic;
using Terraria;
using gvmod.Common.Players.Septimas.Skills;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace gvmod.Common.Players
{
    public abstract class Septima
    {
        public Player Player { get; }
        public AdeptPlayer Adept { get; }
        public AdeptMuse Muse { get; }

        // Key is the id, Value is the type of interaction
        public abstract Dictionary<int, float> ProjectileInteractions { get; }
        public abstract Dictionary<int, float> NPCInteractions { get; }

        public abstract string Name { get; }

        public int SecondaryTimer { get; set; }
        public int SecondaryCooldownTime { get; set; }

        public List<Special> Abilities { get; } = new List<Special>();
        public float ApBaseRegen { get; set; }

        public float SpBaseRegen { get; set; }
        public float SpBaseOverheatRegen { get; set; }
        public float SpBaseUsage { get; set; }

        public Vector2 VelocityMultiplier { get; set; }

        public abstract bool CanRecharge { get; }

        public abstract Color ClearColor { get; }
        public abstract Color MainColor { get; }
        public abstract Color DarkColor { get; }

        protected Septima(AdeptPlayer adept, AdeptMuse muse, Player player)
        {
            Adept = adept;
            Muse = muse;
            Player = player;
            VelocityMultiplier = new Vector2(1, 1);
        }

        public abstract void InitializeAbilitiesList();

        public abstract void OnOverheat();

        public abstract void OnRecovery();

        public abstract void DuringOverheat();

        public abstract void FirstAbilityEffects();

        public abstract void FirstAbility();

        public abstract void SecondAbilityEffects();
        
        public abstract void SecondAbility();

        /// <summary> 
        /// <para>Runs before prevasion.</para>
        /// </summary>
        /// <returns>False to skip <seealso cref="Septima.OnPrevasion(Player.HurtInfo)"/> and instead return false.</returns>
        
        public abstract bool PrePrevasion(Player.HurtInfo info);

        /// <summary> 
        /// <para>Runs before damage is taken, is skipped if the projectile or NPC is in any of the 
        /// <seealso cref="AdeptPlayer.projectileGlobalPrevasionPenetration"/>, 
        /// <seealso cref="AdeptPlayer.projectileGlobalPrevasionIgnore"/>, 
        /// <seealso cref="AdeptPlayer.npcGlobalPrevasionPenetration"/>, 
        /// <seealso cref="AdeptPlayer.npcGlobalPrevasionIgnore"/> lists.</para>
        /// </summary>
        /// <returns>True to ignore damage for this attack.</returns>
        public abstract bool OnPrevasion(Player.HurtInfo info);

        /// <summary> 
        /// <para>Runs before damage is taken, does not run if <seealso cref="Septima.OnPrevasion(Player.HurtInfo)"/> returns true.</para>
        /// </summary>
        public abstract void OnHit(ref Player.HurtModifiers modifiers);

        // This method will not run if timer is less than 1, so take that in mind when making attacks
        public abstract void MorbAttack(int timer);

        public abstract void MiscEffects();

        public abstract void Updates();

        public List<Special> AvaliableSpecials()
        {
            List<Special> specials = new List<Special>();
            for (int i = 0; i < Abilities.Count; i++)
            {
                if (Abilities[i] == null)
                {
                    specials.Add(new None(Player, Adept, ""));
                }
                else
                {
                    if (Adept.Level >= Abilities[i].UnlockLevel)
                    {
                        specials.Add(Abilities[i]);
                    }
                }
            }
            return specials;
        }

        public List<int> AvaliableSpecialsIndex()
        {
            List<int> specials = new List<int>();
            for (int i = 0; i < Abilities.Count; i++)
            {
                if (Abilities[i] == null)
                {
                    specials.Add(0);
                } else
                {
                    if (Adept.Level >= Abilities[i].UnlockLevel)
                    {
                        specials.Add(i);
                    }
                }
            }
            if (specials.Count == 0)
            {
                specials.Add(0);
            }
            return specials;
        }

        public abstract void CheckEvolution();

        public abstract void UpdateEvolution();
    }
}
