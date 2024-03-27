using Terraria;
using Terraria.ModLoader;
using gvmod.Content.Projectiles;

namespace gvmod.Common.GlobalProjectiles
{
    internal class GlobalVasukiEffect : GlobalProjectile
    {
        public int SourceNPC { get; set; }
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ProjectileType<HairDartProjectile>() ||
                entity.type == ModContent.ProjectileType<DullahanProjectile>();
        }
    }
}
