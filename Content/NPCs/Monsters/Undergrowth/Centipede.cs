﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Remnants.Content.Dusts;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using System.IO;
using Terraria.GameContent.ItemDropRules;
using Remnants.Content.World;
using Terraria.Localization;
using Remnants.Content.Items.Consumable;

namespace Remnants.Content.NPCs.Monsters.Undergrowth
{
    public class CentipedeHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NeedsExpertScaling[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "Remnants/Content/NPCs/Monsters/Undergrowth/CentipedeBestiary",
                Position = new Vector2(0f, 36f),
                PortraitPositionXOverride = -12f,
                PortraitPositionYOverride = 20f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        bool TailSpawned = false;

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.GiantWormHead);

            NPC.aiStyle = -1;

            NPC.width = 10;
            NPC.height = 18;

            NPC.lifeMax = 50;
            NPC.defense = 3;
            NPC.damage = 16;

            NPC.value = 100;

            NPC.HitSound = SoundID.NPCHit31;
            NPC.DeathSound = SoundID.NPCDeath34;

            NPC.buffImmune[BuffID.Poisoned] = true;

            SpawnModBiomes = new int[] { ModContent.GetInstance<Biomes.Undergrowth>().Type };
        }

        public override void AI()
        {
            if (!TailSpawned)
            {
                int thisSegment = NPC.whoAmI;
                int segmentAmt = (int)(20 + Main.GameUpdateCount % 10);

                for (int j = 0; j <= segmentAmt; j++)
                {
                    int segmentType = j == segmentAmt ? ModContent.NPCType<CentipedeTail>() : ModContent.NPCType<CentipedeBody>();

                    int newSegment = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + NPC.width / 2), (int)(NPC.position.Y + NPC.height), segmentType, NPC.whoAmI);

                    Main.npc[newSegment].ai[3] = NPC.whoAmI;
                    Main.npc[newSegment].realLife = NPC.whoAmI;
                    Main.npc[newSegment].ai[1] = thisSegment;
                    Main.npc[thisSegment].ai[0] = newSegment;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, newSegment, 0f, 0f, 0f, 0, 0, 0);
                    thisSegment = newSegment;
                }
                TailSpawned = true;
            }

            if (!NPC.HasValidTarget)
            {
                NPC.TargetClosest();
            }

            int tileCoordX = (int)(NPC.Center.X / 16);
            int tileCoordY = (int)(NPC.Center.Y / 16);

            bool collision = Collision.SolidCollision(NPC.position, NPC.width, NPC.height);

            if (collision || Main.tile[tileCoordX, tileCoordY].WallType != 0 || tileCoordY > Main.worldSurface && tileCoordY < Main.maxTilesY - 200)
            {
                bool attacking = false;

                if (Main.player[NPC.target].InModBiome<Biomes.Undergrowth>() || Main.player[NPC.target].Center.Y / 16 > Main.worldSurface)
                {
                    attacking = (Main.GameUpdateCount + NPC.whoAmI * 37) % 480 < 60 + NPC.whoAmI % 3 * 30;

                    Vector2 target = attacking ? Main.player[NPC.target].Center : Main.player[NPC.target].Center + new Vector2((float)Math.Sin(Main.GameUpdateCount / 50), (float)Math.Cos(Main.GameUpdateCount / 50) * (NPC.whoAmI % 2 == 0 ? -1 : 1)) * 16 * (16 + NPC.whoAmI % 3 * 8);
                    NPC.velocity += Vector2.Normalize(target - NPC.Center) / (attacking ? 5 : 9);
                }
                else NPC.velocity.Y += 1f / 9;

                NPC.velocity *= attacking ? 0.96f : 0.98f;

                if (collision)
                {
                    if (NPC.soundDelay == 0)
                    {
                        // Play sounds quicker the closer the NPC is to the target location
                        float num1 = Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) / 40f;

                        if (num1 < 10)
                            num1 = 10f;

                        if (num1 > 20)
                            num1 = 20f;

                        NPC.soundDelay = (int)num1;

                        SoundEngine.PlaySound(SoundID.WormDig, NPC.position);
                    }
                }
            }
            else
            {
                NPC.velocity.Y += Main.tile[tileCoordX, tileCoordY].LiquidAmount > 64 ? 0.05f : 0.1f;
                NPC.velocity *= 0.99f;
            }

            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Bezoar, 100));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                for (int i = 0; i < hit.Damage; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection * 2, -1f);
                }
                return;
            }
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection * 2, -2f, NPC.alpha);
                if (Main.rand.NextBool(8))
                {
                    Main.dust[dust].noGravity = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>("Remnants/Content/NPCs/Monsters/Undergrowth/CentipedeAntenna").Value, NPC.Center + new Vector2(3, -7).RotatedBy(NPC.rotation) - Main.screenPosition + Vector2.UnitY * 6, new Rectangle(0, 0, 20, 52), drawColor, NPC.rotation + (float)(Math.Sin(Main.GameUpdateCount / 5 + (3 + NPC.whoAmI % 3)) + 1) / 3, new Vector2(10, 26), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("Remnants/Content/NPCs/Monsters/Undergrowth/CentipedeAntenna").Value, NPC.Center + new Vector2(-3, -7).RotatedBy(NPC.rotation) - Main.screenPosition + Vector2.UnitY * 6, new Rectangle(0, 0, 20, 52), drawColor, NPC.rotation - (float)(Math.Sin(Main.GameUpdateCount / 5 + (3 + NPC.whoAmI * 3 % 3)) + 1) / 3, new Vector2(10, 26), 1f, SpriteEffects.FlipHorizontally, 0f);

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Poisoned, Main.expertMode ? 1200 : 600, true);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Mods.Remnants.Bestiary.Centipede"),
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<Biomes.Undergrowth>().ModBiomeBestiaryInfoElement)
            });
        }
    }

    public class CentipedeBody : ModNPC
    {
        public override LocalizedText DisplayName => Language.GetOrRegister("Mods.Remnants.NPCs.CentipedeHead.DisplayName");

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NeedsExpertScaling[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;

            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.GiantWormBody);

            NPC.width = 10;
            NPC.height = 18;

            NPC.lifeMax = 50;
            NPC.defense = 12;
            NPC.damage = 8;

            NPC.buffImmune[BuffID.Poisoned] = true;

            NPC.HitSound = SoundID.NPCHit31;
            NPC.DeathSound = SoundID.NPCDeath34;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override void AI()
        {
            bool shouldDie = false;
            if (!Main.npc[(int)NPC.ai[1]].active || NPC.ai[1] <= 0f)
            {
                shouldDie = true;
            }
            else if (Main.npc[(int)NPC.ai[1]].life <= 0)
            {
                shouldDie = true;
            }
            if (shouldDie)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.checkDead();
            }
        }

        Vector2 oldPos;
        public override void FindFrame(int frameHeight)
        {
            if (oldPos != Vector2.Zero)
            {
                NPC.frameCounter += (Main.npc[(int)NPC.ai[3]].position - oldPos).Length();
            }

            oldPos = Main.npc[(int)NPC.ai[3]].position;

            NPC.frame.Y = (int)((NPC.frameCounter / 10 - NPC.whoAmI + Main.maxNPCs) % Main.npcFrameCount[Type]) * 14;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                for (int i = 0; i < hit.Damage; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection * 2, -1f);
                }
                return;
            }
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection * 2, -2f, NPC.alpha);
                if (Main.rand.NextBool(8))
                {
                    Main.dust[dust].noGravity = true;
                }
            }
        }
        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreKill()
        {
            return false;
        }
    }

    public class CentipedeTail : ModNPC
    {
        public override LocalizedText DisplayName => Language.GetOrRegister("Mods.Remnants.NPCs.CentipedeHead.DisplayName");

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NeedsExpertScaling[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.GiantWormTail);

            NPC.width = 10;
            NPC.height = 18;

            NPC.lifeMax = 50;
            NPC.defense = 12;
            NPC.damage = 8;

            NPC.buffImmune[BuffID.Poisoned] = true;

            NPC.HitSound = SoundID.NPCHit31;
            NPC.DeathSound = SoundID.NPCDeath34;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void AI()
        {
            bool shouldDie = false;
            if (!Main.npc[(int)NPC.ai[1]].active || NPC.ai[1] <= 0f)
            {
                shouldDie = true;
            }
            else if (Main.npc[(int)NPC.ai[1]].life <= 0)
            {
                shouldDie = true;
            }
            if (shouldDie)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.checkDead();
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                for (int i = 0; i < hit.Damage; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection * 2, -1f);
                }
                return;
            }
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection * 2, -2f, NPC.alpha);
                if (Main.rand.NextBool(8))
                {
                    Main.dust[dust].noGravity = true;
                }
            }
        }
        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreKill()
        {
            return false;
        }
    }
}
