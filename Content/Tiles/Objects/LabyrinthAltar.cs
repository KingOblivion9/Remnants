﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Remnants.Content.Dusts;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Remnants.Content.Tiles.Objects
{
	[LegacyName("mazealtar")]
	public class LabyrinthAltar : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
			TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 3;
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);

            AddMapEntry(new Color(104, 120, 127), CreateMapEntryName());

			DustType = DustID.Stone;
		}

        public override bool RightClick(int i, int j)
        {
			int style = GetStyle(i, j);
			if (Main.LocalPlayer.GetModPlayer<RemPlayer>().activeEssence != style + 1)
			{
				Main.LocalPlayer.GetModPlayer<RemPlayer>().activeEssence = style + 1;
				SoundEngine.PlaySound(new SoundStyle("Remnants/Content/Sounds/megahammer"), new Vector2(i, j) * 16);

                Main.NewText(".", new Color(51, 51, 51));

                if (style == 0)
				{
					Main.NewText(Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.EssenceofMight"), new Color(255, 255, 153));
					Main.NewText("+20% " + Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Health"), new Color(51, 204, 51));
					Main.NewText("+10% " + Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Damage"), new Color(51, 204, 51));
					Main.NewText("-20% " + Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Speed"), new Color(204, 51, 51));
				}
				else if (style == 1)
				{
					Main.NewText(Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.EssenceofEndurance"), new Color(255, 255, 153));
					Main.NewText("+20% " + Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Health"), new Color(51, 204, 51));
					Main.NewText("+20% " + Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Speed"), new Color(51, 204, 51));
					Main.NewText("-10% " + Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Damage"), new Color(204, 51, 51));
				}
				else if (style == 2)
				{
					Main.NewText(Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.EssenceofFocus"), new Color(255, 255, 153));
					Main.NewText("+10% " + Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Damage"), new Color(51, 204, 51));
					Main.NewText("+20% " + Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Speed"), new Color(51, 204, 51));
					Main.NewText("-20% " + Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Health"), new Color(204, 51, 51));
				}

                Tile tile = Main.tile[i, j];

				for (int k = 0; k < 25; k++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(i - tile.TileFrameX / 16 % 3 + 0.5f, j - tile.TileFrameY / 16) * 16, 26, 8, GetDustType(GetStyle(i, j)), Scale: Main.rand.NextFloat(1, 2));
					dust.noGravity = true;
					dust.velocity.Y = -Main.rand.NextFloat(7.5f);
				}

                for (int k = 0; k < 50; k++)
                {
                    Dust dust = Dust.NewDustPerfect(new Vector2(i - tile.TileFrameX / 16 % 3 + 1.5f, j - tile.TileFrameY / 16 + 2f) * 16, GetDustType(GetStyle(i, j)), Scale: Main.rand.NextFloat(1, 2));
                    dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2CircularEdge(2f, 2f);
                }
            }
			else
			{
				Main.LocalPlayer.GetModPlayer<RemPlayer>().activeEssence = 0;

                Main.NewText(".", new Color(51, 51, 51));
                Main.NewText(Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.Messages.Deactivation"), new Color(255, 204, 102));
            }

			return true;
        }

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
			if (Main.LocalPlayer.GetModPlayer<RemPlayer>().activeEssence == GetStyle(i, j) + 1)
            {
				Tile tile = Main.tile[i, j];

				if (tile.TileFrameY == 0 && tile.TileFrameX % (16 * 3) == 16)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(i - 0.5f, j) * 16, 26, 8, GetDustType(GetStyle(i, j)), Scale: Main.rand.NextFloat(1, 2));
					dust.noGravity = true;
					dust.velocity.Y = -Main.rand.NextFloat(5f);
				}
			}
		}

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			int style = GetStyle(i, j);

			bool selected = Main.LocalPlayer.GetModPlayer<RemPlayer>().activeEssence == style + 1;

			if (style == 0)
            {
				if (selected)
                {
					r = 255f;
					g = 102f;
					b = 57f;
				}
				else
                {
					r = 166f;
					g = 45f;
					b = 54f;
                }

			}
			else if (style == 1)
			{
				if (selected)
				{
					r = 255f;
					g = 56f;
					b = 121f;
				}
				else
				{
					r = 158f;
					g = 37f;
					b = 109f;
				}

			}
			else if (style == 2)
			{
				if (selected)
				{
					r = 184f;
					g = 57f;
					b = 255f;
				}
				else
				{
					r = 93f;
					g = 45f;
					b = 166f;
				}

			}

			r /= 255f;
			g /= 255f;
			b /= 255f;
		}

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
			Tile tile = Main.tile[i, j];
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

			Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY + (Main.LocalPlayer.GetModPlayer<RemPlayer>().activeEssence == GetStyle(i, j) + 1 ? 16 * 6 : 16 * 3) + 2, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

        public override void MouseOver(int i, int j)
		{
			int style = GetStyle(i, j);

			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = -1;
			player.cursorItemIconText = Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.MouseOver.Essence") + " " + (style == 2 ? Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.MouseOver.Focus") : style == 1 ? Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.MouseOver.Endurance") : Language.GetTextValue("Mods.Remnants.Tiles.LabyrinthAltar.MouseOver.Might"));
		}

		private int GetStyle(int i, int j)
        {
			Tile tile = Main.tile[i, j];

			int style = 0;
			if (tile.TileFrameX >= 16 * 6)
			{
				style = 2;
			}
			else if (tile.TileFrameX >= 16 * 3)
			{
				style = 1;
			}

			return style;
		}

		private int GetDustType(int style)
        {
			return style == 2 ? DustID.PurpleTorch : style == 1 ? DustID.RedTorch : DustID.Torch;

		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

		public override bool CanExplode(int i, int j) => false;
	}
}
