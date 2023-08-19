using System;
using System.Collections.Generic;
using FanIdeas.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
namespace FanIdeas
{
	public static class Tools
	{
        /// <summary>
        /// Picks an number from a dictionary based on the associated chance value from 0-1
        /// </summary>
        /// <param name="dict"> A dictionary with <int, double> pairs where int is item id and the double is percentage/100 </param>
        /// <returns></returns>
        public static int LootPool(Dictionary<int, double> dict)
		{
			var a = Main.player[0];
			List<int> nums = new List<int>();
			foreach (var i in dict)
			{
				for (int j = 0; j < i.Value * 100; j++)
				{
					nums.Add(i.Key);
				}
			}
			int num = Main.rand.Next(0, nums.Count + 1);
			return nums[num];
		}
	}
}

