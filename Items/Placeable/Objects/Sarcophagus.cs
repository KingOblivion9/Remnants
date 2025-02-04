using Microsoft.Xna.Framework;
using Remnants.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Remnants.Tiles.Objects;

namespace Remnants.Items.Placeable.Objects
{
	[LegacyName("sarcophagus")]
	public class Sarcophagus : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Sunflower);
			Item.width = 12;
			Item.height = Item.width * 2;
			Item.maxStack = 99;
			Item.createTile = ModContent.TileType<Tiles.Objects.Sarcophagus>();
		}
	}
}