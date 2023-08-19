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

namespace FanIdeas.Buffs
{
	public class Quicksanded : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;

            DisplayName.SetDefault("Quick-Sanded");
            
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 0.8f;
            Dust.NewDust(player.Center, player.width, player.height, DustID.Sand);
        }
    }
}

