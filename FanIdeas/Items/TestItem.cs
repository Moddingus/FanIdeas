using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FanIdeas.Projectiles;
using Microsoft.Xna.Framework;

namespace FanIdeas.Items
{
    public class TestItem : ModItem
    {
        public int Mode;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Configurator"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Throwing;
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 55;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = 0;
            Item.shootSpeed = 6;
        }
        public override bool AltFunctionUse(Player player)
        {
            Main.NewText("right click");
            Mode = (Mode == 1) ? 0 : Mode + 1;
            return true;
        }
        public override void HoldItem(Player player)
        {
            Item.shoot = Main.LocalPlayer.GetModPlayer<TestPlayer>().TestItemProjectile;
            base.HoldItem(player);
        }
        public override bool? UseItem(Player player)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(player.Center, 10, 10, DustID.FartInAJar, 5 * player.direction, 0);
            }
            return base.UseItem(player);
        }
    }
    public class TestPlayer : ModPlayer
    {
        public int TestItemProjectile;
        public int TestItemShootSpeed;
    }
    public class ExampleSummonCommand : ModCommand
    {
        // CommandType.World means that command can be used in Chat in SP and MP, but executes on the Server in MP
        public override CommandType Type
            => CommandType.Chat;

        // The desired text to trigger this command
        public override string Command
            => ".shoot";

        // A short usage explanation for this command
        public override string Usage
            => "/summon type/name shootSpeed" +
            "\n type/name - Projectile ID or name of Projectile.\n" +
            "shootSpeed - shoot speed";

        // A short description of this command
        public override string Description
            => "set shoot of configurator";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            var player = Main.LocalPlayer.GetModPlayer<TestPlayer>();
            // Checking input Arguments
            if (args.Length == 0)
            {
                throw new UsageException("At least one argument was expected.");
            }
            if (int.TryParse(args[0], out int type))
            {
                player.TestItemProjectile = int.Parse(args[0]);
                throw new UsageException(args[0] + " is not a correct value.");
            } else
            {
                for (int i = 0; i < ContentSamples.ProjectilesByType.Count; i++)
                {
                    var a = ContentSamples.ProjectilesByType[i];
                    
                }
                
            }

        }
    }
}