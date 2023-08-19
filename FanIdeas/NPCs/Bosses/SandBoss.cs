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
namespace FanIdeas.NPCs.Bosses
{
    [AutoloadBossHead]
    public class SandBoss : ModNPC
    {
        float grav, attackTimer, moveTimer;
        bool trail, active;
        int moveStyle = 1;
        Vector2 endPoint;
        public bool SecondStage
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value ? 1f : 0f;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
            // Here you'd want to change the potion type that drops when the boss is defeated. Because this boss is early pre-hardmode, we keep it unchanged
            // (Lesser Healing Potion). If you wanted to change it, simply write "potionType = ItemID.HealingPotion;" or any other potion type
        }
        public override void SetStaticDefaults()
        {
            //Main.npcFrameCount[Type] = 6;

            DisplayName.SetDefault("Lord of Shifting Sands");
            // Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Specify the debuffs it is immune to
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned,
                    BuffID.OnFire,
                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.TrailingMode[Type] = 1;
            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "FanIdeas/NPCs/Bosses/SandBoss",
                PortraitScale = 0.6f, // Portrait refers to the full picture when clicking on the icon in the bestiary
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 66;
            NPC.damage = 12;
            NPC.defense = 10;
            NPC.lifeMax = 39500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.scale = 1.5f;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 5);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f; // Take up open spawn slots, preventing random NPCs from spawning during the fight
            NPC.ScaleStats_UseStrengthMultiplier(1.6f);
            NPC.noTileCollide = false;
            // Don't set immunities like this as of 1.4:
            // NPC.buffImmune[BuffID.Confused] = true;
            // immunities are handled via dictionaries through NPCID.Sets.DebuffImmunitySets

            // Custom AI, 0 is "bound town NPC" AI which slows the NPC down and changes sprite orientation towards the target
            NPC.aiStyle = -1;

            // Custom boss bar
            //NPC.BossBar = ModContent.GetInstance<SandBossBossBar>();

            // The following code assigns a music track to the boss in a simple way.
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/DuneBattle");
            }
        }
        public override void AI()
        {
            // This should almost always be the first code in AI() as it is responsible for finding the proper player target
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            Player player = Main.player[NPC.target];
            Vector2 toPlayer = NPC.Center.DirectionTo(player.Center);
            Main.NewText(moveStyle);
            switch (moveStyle)
            {
                case 1:
                    //normal + teleport
                    NPC.damage = 12;
                    trail = false;
                    NPC.noTileCollide = false;
                    grav = -.4f;

                    if (player.Distance(NPC.Center) > 50 * 16 || NPC.velocity.Length() < 0.05f && player.Distance(NPC.Center) > 5*16)
                    {
                        moveTimer++;
                        if (moveTimer == 1) { endPoint = player.Center; }
                        Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Sand);

                        Dust.NewDust(endPoint, NPC.width, NPC.height, DustID.Sand);

                        if (moveTimer == 60)
                        {
                            NPC.Teleport(endPoint - new Vector2(NPC.width/2, NPC.height));
                            moveTimer = 0;
                        }
                    }
                    else {
                        moveTimer = 0;
                        
                    }

                    NPC.velocity.Y = Math.Clamp(NPC.velocity.Y + 0.4f, -14, 14);
                    NPC.velocity.X = toPlayer.X * 5;
                    NPC.spriteDirection = (player.Center.X > NPC.Center.X) ? -1 : 1;

                    FirstStageAttack(player);
                    CheckSecondStage();
                    break;

                case 2:
                    //dash
                    Main.NewText(moveTimer);
                    NPC.damage = 120;
                    if (moveTimer == 0)
                    {
                        NPC.noTileCollide = true;
                        trail = true;
                        endPoint = NPC.Center + NPC.Center.DirectionTo(player.Center) * Math.Clamp(NPC.Distance(player.Center) * 1.5f, 0, 1600); //some vector
                    }
                    moveTimer++;
                    grav = 0f;
                    
                    if (moveTimer % 10 == 0 && NPC.velocity.Length() > 0.01f)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<PeaceBall>(), NPC.damage, 2, NPC.whoAmI);
                    }
                    if (trail)
                    {
                        NPC.velocity = NPC.Center.DirectionTo(endPoint) * 25;
                    }
                    if (moveTimer == 50 || NPC.WithinRange(endPoint, 40) && trail)
                    {
                        NPC.velocity = Vector2.Zero;
                        trail = false;
                        NPC.noTileCollide = false;
                        grav = 1.5f;
                    }
                    if (moveTimer > 50) { NPC.velocity.Y = Math.Clamp(NPC.velocity.Y - grav, -30, 30); }

                    if (moveTimer == 85)
                    {
                        moveStyle = SecondStage == true? 4: 1;
                        moveTimer = 0;
                    }
                    break;
                case 3:
                    //transition to second stage
                    ++moveTimer;
                    NPC.dontTakeDamage = true;
                    NPC.velocity.Y = -0.2f;
                    NPC.velocity.X = 0;
                    NPC.spriteDirection = (player.Center.X > NPC.Center.X) ? -1 : 1;
                    //circling rings of sand

                    if (moveTimer == 60*3.5)
                    {
                        moveStyle = 4;
                        moveTimer = 0;
                    }
                    break;
                case 4:
                    moveTimer++;
                    NPC.noTileCollide = true;
                    break;

            }
            
            


            if (player.dead)
            {
                NPC.EncourageDespawn(10);
                return;
            }
            

            if (!SecondStage)
            {
                FirstStageAttack(player);
            }
        }

        public void CheckSecondStage()
        {
            if (SecondStage)
            {
                return;
            }
            if (NPC.life <= NPC.lifeMax/2)
            {
                SecondStage = true;
                NPC.netUpdate = true;
                moveTimer = 0;
                moveStyle = 3;
            }
        }


        public void FirstStageAttack(Player player)
        {
            Vector2 toPlayer = NPC.Center.DirectionTo(player.Center);

            ++attackTimer;

            if (attackTimer % 500 == 0)
            {
                int a = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(45 * Math.Clamp(toPlayer.X, -1, 1), 0), new Vector2(toPlayer.X * 10, 0), ProjectileID.SandnadoHostile, NPC.damage*3, 3);
                Main.projectile[a].scale = 0.5f;
            }


            if (attackTimer % 660 == 0 && player.Distance(NPC.Center) > 5*16)
            {
                moveTimer = 0;
                moveStyle = 2;
            }
        }
        public void DespawnAnimation()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 position = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi/10) * 160;
                Vector2 velocity = position.DirectionTo(NPC.Center) * 8;
                Dust.NewDust(position, 1, 1, DustID.Sand, velocity.X, velocity.Y);
            }
        }
        float RedEyesPercent = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            if (trail)
            {
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    
                    Vector2 drawPos = NPC.oldPos[k] + new Vector2(NPC.width/2, NPC.height) - Main.screenPosition;
                    Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    spriteBatch.Draw(texture, drawPos, null, color, NPC.rotation, drawOrigin, NPC.scale - k / (float)NPC.oldPos.Length / 3, spriteEffects, 0f);
                }
            }
            
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (SecondStage)
            {
                RedEyesPercent = Math.Clamp(RedEyesPercent + 0.01f, 0, 1);
                for (int i = 0; i < 4; i++)
                {
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, (NPC.position + new Vector2(39.1f + i * 4*NPC.scale, 37)) - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.Lerp(new Color(172, 208, 250), new Color(244, 66, 54), RedEyesPercent), 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                }
                for (int i = 0; i < 4; i++)
                {
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, (NPC.position + new Vector2(39.1f + i * 4 * NPC.scale, 37+2*NPC.scale)) - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.Lerp(new Color(49, 115, 186), new Color(213, 0, 0), RedEyesPercent), 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                }

            }
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
    }
} 

