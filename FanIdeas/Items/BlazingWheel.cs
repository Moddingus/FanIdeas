using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FanIdeas.Projectiles;
using Microsoft.Xna.Framework;

namespace FanIdeas.Items
{
    public class BlazingWheel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purging Probe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Throwing;
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 55;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = 2;
            Item.maxStack = 3;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<PurgingProbeProj>();
            Item.shootSpeed = 6;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.Pi / 14, MathHelper.Pi / 14)) * Main.rand.NextFloat(0.9f, 1.1f);
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public override void ModifyItemScale(Player player, ref float scale)
        {
            base.ModifyItemScale(player, ref scale);
        }
    }
}