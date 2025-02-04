using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Remnants.Tiles.Objects.Furniture;

namespace Remnants.Items.Placeable.Objects
{
	public class LabyrinthDoor : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodenDoor);
			Item.createTile = ModContent.TileType<LabyrinthDoorClosed>();
		}
	}
}