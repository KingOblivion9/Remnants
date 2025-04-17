using Terraria.ID;
using Terraria.ModLoader;
using Remnants.Content.Tiles;
using Terraria;
using Remnants.Content.Items.Placeable.Blocks;

namespace Remnants.Content.Items.Placeable.Plants
{
    public class DreampodSeeds : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenChair);
            Item.width = 10 * 2;
            Item.height = 10 * 2;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 1);
            Item.createTile = ModContent.TileType<Tiles.Plants.DreampodVine>();
        }
    }
}