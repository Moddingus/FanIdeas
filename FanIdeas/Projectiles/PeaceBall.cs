using FanIdeas.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using ReLogic.Content;

namespace FanIdeas.Projectiles
{
    public class PeaceBall : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Peace Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22; // The width of projectile hitbox
            Projectile.height = 22; // The height of projectile hitbox
            Projectile.hostile = true;
            Projectile.damage = 802;
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false; // Can the projectile collide with tiles?
            Projectile.penetrate = 1; // Look at comments ExamplePiercingProjectile
            Projectile.alpha = 0; // How transparent to draw this projectile. 0 to 255. 255 is completely transparent.
            Projectile.timeLeft = 150;
            
        }
        public override void AI()
        {
            Projectile.rotation += 0.02f;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Confused, 120);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                //Dust.NewDust()
            }
            //Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PeaceShard>(), Projectile.damage / 2, 2, Projectile.owner, Projectile.whoAmI, -1);
            //Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PeaceShard>(), Projectile.damage / 2, 2, Projectile.owner, Projectile.whoAmI, 1);
        }
    }
    public class PeaceShard : ModProjectile
    {
        Vector2 center;
        public override void SetStaticDefaults()
        {
            // Total count animation frames
            DisplayName.SetDefault("Peace Shard");
            
        }
        float dist, rot;
        public override void SetDefaults()
        {
            Projectile.width = 16; // The width of projectile hitbox
            Projectile.height = 20; // The height of projectile hitbox
            Projectile.hostile = true;
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false; // Can the projectile collide with tiles?
            Projectile.penetrate = -1; // Look at comments ExamplePiercingProjectile
            Projectile.alpha = 0; // How transparent to draw this projectile. 0 to 255. 255 is completely transparent.

            center = Main.projectile[(int)Projectile.ai[0]].Center;
        }
        public override string Texture => $"FanIdeas/Projectiles/PeaceShardDark";
        public override void AI()
        {
            
            dist += 0.5f;
            rot += 2;
            Projectile.Center = center + Vector2.One.RotatedBy(MathHelper.ToRadians(rot /*+ Projectile.ai[1] == 1? 180: 0*/)) * dist;
        }
    }
}

