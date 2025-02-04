using Microsoft.Xna.Framework;
using Remnants.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Remnants.Tiles.Objects.Furniture;

namespace Remnants.Items.Placeable.Objects
{
	[LegacyName("metalchest")]
	public class RustedChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Chest);
            Item.createTile = ModContent.TileType<Tiles.Objects.Furniture.RustedChest>();
		}
	}
}