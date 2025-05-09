﻿using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;

namespace Remnants.Content.Tiles.Objects.chests
{
	//public class vaultlocker : ModTile
	//{
	//	public override void SetStaticDefaults()
	//	{
	//		Main.tileSpelunker[Type] = true;
	//		Main.tileContainer[Type] = true;
	//		Main.tileShine2[Type] = true;
	//		Main.tileShine[Type] = 1200;
	//		Main.tileFrameImportant[Type] = true;
	//		Main.tileNoAttach[Type] = true;
	//		Main.tileOreFinderPriority[Type] = 500;

	//		TileID.Sets.HasOutlines[Type] = true;
	//		TileID.Sets.DisableSmartCursor[Type] = true;

	//		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
	//		TileObjectData.newTile.Origin = new Point16(0, 1);
	//		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
	//		TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
	//		TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
	//		TileObjectData.newTile.AnchorInvalidTiles = new int[] { TileID.MagicalIceBlock };
	//		TileObjectData.newTile.StyleHorizontal = true;
	//		TileObjectData.newTile.LavaDeath = false;
	//		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
	//		TileObjectData.addTile(Type);

	//		ModTranslation name = CreateMapEntryName();
	//		name.SetDefault("Vault Locker");
	//		AddMapEntry(new Color(107, 97, 92), name, MapChestName);

	//		DustType = 8;
	//		AdjTiles = new int[] { TileID.Containers };
	//		ChestDrop = ModContent.ItemType<Items.placeable.metalchest>();

	//		ContainerName.SetDefault("Vault Locker");
	//	}

	//	public override ushort GetMapOption(int i, int j) => (ushort)(Main.tile[i, j].TileFrameX / 36);

	//	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

	//	//public override bool IsLockedChest(int i, int j) => Main.tile[i, j].frameX / 36 == 1;

	//	//public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int DustType, ref bool manual)
	//	//{
	//	//	if (Main.dayTime)
	//	//		return false;
	//	//	DustType = this.DustType;
	//	//	return true;
	//	//}

	//	public string MapChestName(string name, int i, int j)
	//	{
	//		int left = i;
	//		int top = j;
	//		Tile tile = Main.tile[i, j];
	//		if (tile.TileFrameX % 36 != 0)
	//		{
	//			left--;
	//		}
	//		if (tile.TileFrameY != 0)
	//		{
	//			top--;
	//		}
	//		int chest = Chest.FindChest(left, top);
	//		if (chest < 0)
	//		{
	//			return Language.GetTextValue("LegacyChestType.0");
	//		}
	//		else if (Main.chest[chest].name == "")
	//		{
	//			return name;
	//		}
	//		else
	//		{
	//			return name + ": " + Main.chest[chest].name;
	//		}
	//	}

	//	public override void NumDust(int i, int j, bool fail, ref int num)
	//	{
	//		num = 1;
	//	}

	//	public override void rKillMultiTile(int i, int j, int frameX, int frameY)
	//	{
	//		Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ChestDrop);
	//		Chest.DestroyChest(i, j);
	//	}

	//	public override bool RightClick(int i, int j)
	//	{
	//		Player player = Main.LocalPlayer;
	//		Tile tile = Main.tile[i, j];
	//		Main.mouseRightRelease = false;
	//		int left = i;
	//		int top = j;
	//		if (tile.TileFrameX % 36 != 0)
	//		{
	//			left--;
	//		}
	//		if (tile.TileFrameY != 0)
	//		{
	//			top--;
	//		}
	//		if (player.sign >= 0)
	//		{
	//			SoundEngine.PlaySound(SoundID.MenuClose);
	//			player.sign = -1;
	//			Main.editSign = false;
	//			Main.npcChatText = "";
	//		}
	//		if (Main.editChest)
	//		{
	//			SoundEngine.PlaySound(SoundID.MenuOpen);
	//			Main.editChest = false;
	//			Main.npcChatText = "";
	//		}
	//		if (player.editedChestName)
	//		{
	//			NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
	//			player.editedChestName = false;
	//		}
	//		// bool isLocked = IsLockedChest(left, top);
	//		bool isLocked = false;
	//		if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
	//		{
	//			if (left == player.chestX && top == player.chestY && player.chest >= 0)
	//			{
	//				player.chest = -1;
	//				Recipe.FindRecipes();
	//				SoundEngine.PlaySound(SoundID.MenuClose);
	//			}
	//			else
	//			{
	//				NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
	//				Main.stackSplit = 600;
	//			}
	//		}
	//		else
	//		{
	//			int chest = Chest.FindChest(left, top);
	//			if (chest >= 0)
	//			{
	//				Main.stackSplit = 600;
	//				if (chest == player.chest)
	//				{
	//					player.chest = -1;
	//					SoundEngine.PlaySound(SoundID.MenuClose);
	//				}
	//				else
	//				{
	//					player.chest = chest;
	//					Main.playerInventory = true;
	//					Main.recBigList = false;
	//					player.chestX = left;
	//					player.chestY = top;
	//					SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
	//				}
	//				Recipe.FindRecipes();
	//			}
	//		}
	//		return true;
	//	}

	//	public override void MouseOver(int i, int j)
	//	{
	//		Player player = Main.LocalPlayer;
	//		Tile tile = Main.tile[i, j];
	//		int left = i;
	//		int top = j;
	//		if (tile.TileFrameX % 36 != 0)
	//		{
	//			left--;
	//		}
	//		if (tile.TileFrameY != 0)
	//		{
	//			top--;
	//		}
	//		int chest = Chest.FindChest(left, top);
	//		player.cursorItemIconID = -1;
	//		if (chest < 0)
	//		{
	//			player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
	//		}
	//		else
	//		{
	//			player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Rusted Chest";
	//			if (player.cursorItemIconText == "Rusted Chest")
	//			{
	//				player.cursorItemIconID = ModContent.ItemType<Items.placeable.metalchest>();
	//				player.cursorItemIconText = "";
	//			}
	//		}
	//		player.noThrow = 2;
	//		player.cursorItemIconEnabled = true;
	//	}

	//	public override void MouseOverFar(int i, int j)
	//	{
	//		MouseOver(i, j);
	//		Player player = Main.LocalPlayer;
	//		if (player.cursorItemIconText == "")
	//		{
	//			player.cursorItemIconEnabled = false;
	//			player.cursorItemIconID = 0;
	//		}
	//	}
	//}
}