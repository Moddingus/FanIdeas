using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using FanIdeas.Items;
using Terraria.ModLoader;

namespace FanIdeas.Projectiles
{
    public class PurgingProbeProj : ModProjectile
    {
        int shootTimer = 0;
        bool active = true;
        NPC latched = null;
        Vector2 offset;
        float rotation = MathHelper.TwoPi / 45;
        public override void SetStaticDefaults()
        {
            // Total count animation frames
        }

        public override void SetDefaults()
        {
            Projectile.width = 16; // The width of projectile hitbox
            Projectile.height = 16; // The height of projectile hitbox
            Projectile.scale = 1f;
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.DamageType = DamageClass.Throwing; // Is the projectile shoot by a ranged weapon?
            Projectile.ignoreWater = false; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.penetrate = 1; // Look at comments ExamplePiercingProjectile
            Projectile.light = 0.3f;
            Projectile.alpha = 0; // How transparent to draw this projectile. 0 to 255. 255 is completely transparent.
            Projectile.timeLeft = 350;

        }

        // Allows you to determine the color and transparency in which a projectile is drawn
        // Return null to use the default color (normally light and buff color)
        // Returns null by default.
        public override void AI()
        {
            if (Projectile.velocity.X < 0 && rotation > 0)
            {
                rotation = -rotation;
            }
            if (Projectile.velocity.LengthSquared() > 0.01f)
            {
                Projectile.velocity *= 0.99f;
            }

            Projectile.rotation += Projectile.velocity.Length() / 100;

            if (++shootTimer == 60)
            {
                if (active && ClosestNPC(Projectile.Center) != -1)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Projectile.Center.DirectionTo(Main.npc[ClosestNPC(Projectile.Center)].Center) * 10, Projectile.Center.DirectionTo(Main.npc[ClosestNPC(Projectile.Center)].Center) * 17, ModContent.ProjectileType<LaserProj>(), (int)(Projectile.damage * 1.5), 2, Projectile.owner);
                }
                shootTimer = 0;
            }

            if (latched != null)
            {
                Projectile.Center = latched.Center + offset;
            }
            base.AI();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Projectile.velocity.Y -= oldVelocity.Y * 0.5f;
            Projectile.velocity.X -= oldVelocity.X * 0.5f;

            return false;
        }

        public int ClosestNPC(Vector2 vector)
        {
            NPC closest = Main.npc[0];
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly)
                {
                    if (npc.DistanceSQ(vector) < closest.DistanceSQ(vector))
                    {
                        closest = npc;
                    }
                }

            }
            return (closest.Distance(vector) < 50 * 16) ? closest.whoAmI : -1;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 vel = Vector2.One.RotatedBy(i);

                Dust.NewDust(Projectile.Center, 10, 10, DustID.Torch, vel.X * 2, vel.Y * 2);
                if (i % 3 == 0)
                {
                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, vel, GoreID.Smoke1);
                }
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.WithinRange(Projectile.Center, 16 * 2))
                {
                    npc.StrikeNPC(Projectile.damage * 3, Projectile.knockBack, (Projectile.Center.X > npc.Center.X) ? -1 : 1);
                }
            }
        }
    }
}

