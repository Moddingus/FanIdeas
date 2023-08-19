using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace FanIdeas.Projectiles
{
        public class LaserProj : ModProjectile
        {

            public override void SetStaticDefaults()
            {
                // Total count animation frames
            }

            public override void SetDefaults()
            {
                Projectile.width = 40; // The width of projectile hitbox
                Projectile.height = 2; // The height of projectile hitbox
                Projectile.friendly = true; // Can the projectile deal damage to enemies?
                Projectile.DamageType = DamageClass.Throwing; // Is the projectile shoot by a ranged weapon?
                Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
                Projectile.tileCollide = false; // Can the projectile collide with tiles?
                Projectile.penetrate = -1; // Look at comments ExamplePiercingProjectile
                Projectile.alpha = 0; // How transparent to draw this projectile. 0 to 255. 255 is completely transparent.
            }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() *0.25f);
        }
    }
}

