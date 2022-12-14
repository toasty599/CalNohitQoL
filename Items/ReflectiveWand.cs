using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalNohitQoL.NPCs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalNohitQoL.Items
{
    public class ReflectiveWand : ModItem
    {
        private int bhType = 1;
        private string TypeText = "Bullet Hell 1 (100%)";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reflective Wand");
            Tooltip.SetDefault("Summons a simulation of Supreme Calamitas' bullet hells\n" +
                "Right click to change type of bullet hell forward or backward depending on mouse position\nCurrent Type");
        }

        public override void SetDefaults()
        {
            Item item = Item;
            item.width = 88;
            item.height = 88;
            item.maxStack = 1;
            item.rare = ItemRarityID.Purple;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = ItemUseStyleID.Swing;
        }

        public override bool AltFunctionUse(Player player) => true;
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                int Direction = Math.Sign(Main.MouseWorld.X - player.position.X);

                bhType += Direction;
                if (bhType > 5)
                    bhType = 1;
                if (bhType < 1)
                    bhType = 5;
                
                switch (bhType)
                {
                    case 1:
                        TypeText = "Bullet Hell 1 (100%)";
                        break;
                    case 2:
                        TypeText = "Bullet Hell 2 (75%)";
                        break;
                    case 3:
                        TypeText = "Bullet Hell 3 (50%)";
                        break;
                    case 4:
                        TypeText = "Bullet Hell 4 (30%)";
                        break;
                    case 5:
                        TypeText = "Bullet Hell 5 (10%)";
                        break;
                    default:
                        TypeText = "None";
                        break;
                }
                CalNohitQoL.BHTYPE = bhType;
                CalNohitQoLUtils.DisplayText("Type changed to: " + TypeText, Color.DarkRed);
            }
            else
            {
                NPC BHSpawner = CalamityUtils.SpawnBossBetter(player.Center+new Vector2(0, -1), ModContent.NPCType<BulletHellSimulator>());
                   
            }
            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault((TooltipLine x) => x.Name == "ItemName" && x.Mod == "Terraria");
            if (nameLine != null)
            {
                nameLine.OverrideColor = CalNohitQoLUtils.TwoColorPulse(new Color(104, 13, 13), new Color(190, 145, 55), 2f);
            }
            Player player = Main.player[Main.myPlayer];
            if (player is null)
                return;
            foreach (TooltipLine l in tooltips)
            {
                if (l.Text == null)
                    continue;

                if (l.Text.StartsWith("Current Type"))
                {
                    if (TypeText != null)
                    {
                        l.OverrideColor = Color.Firebrick;
                        l.Text = "Current Type: " + TypeText;
                    }
                    
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<CalamityMod.Items.Materials.AuricBar>(), 1);
            recipe.AddIngredient(ModContent.ItemType<CalamityMod.Items.Materials.AshesofCalamity>(), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}