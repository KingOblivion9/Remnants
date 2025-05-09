using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Remnants.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Remnants.Content.Projectiles.Weapons
{
	public class SpiritLanceHoldout : ModProjectile
	{
		//public override string Texture => ModContent.GetModItem(ModContent.ItemType<Items.weapon.spiritlance>()).Texture;
        // Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
        protected virtual float HoldoutRangeMin => 48f;
		protected virtual float HoldoutRangeMax => 128f;

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.

			Projectile.width = 48 * 2;
			Projectile.height = 48 * 2;
			Projectile.scale = 1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

        public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
			int duration = player.itemAnimationMax; // Define the duration the projectile will exist in frames

			player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

			// Reset projectile time left if necessary
			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}

			Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

			float halfDuration = duration * 0.5f;
			float progress;

			// Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation.
			if (Projectile.timeLeft < halfDuration)
			{
				progress = Projectile.timeLeft / halfDuration;
			}
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			// Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

			// Avoid spawning dusts on dedicated servers
			if (!Main.dedServ)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - Vector2.One * 2.5f + Projectile.velocity * 16 + Main.rand.NextVector2Circular(16, 16), 5, 5, ModContent.DustType<SpiritLanceDust>(), 0, 0);
				dust.velocity = Projectile.velocity * 2;
				if (Projectile.timeLeft < halfDuration)
                {
					dust.velocity *= -1;
                }
				dust.velocity += Main.rand.NextVector2Circular(1, 1);
			}

			Lighting.AddLight(Projectile.Center, 58f / 255f, 156f / 255f, 255f / 255f);

			return false;
		}

        public override bool PreDraw(ref Color lightColor)
        {
			lightColor = Color.White;
			return true;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			if (Main.player[Projectile.owner].statMana < Main.player[Projectile.owner].statManaMax2)
            {
				modifiers.SourceDamage *= 4;
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.player[Projectile.owner].statMana < Main.player[Projectile.owner].statManaMax)
            {
				Main.player[Projectile.owner].statMana += damageDone / 2;
				if (Main.player[Projectile.owner].statMana > Main.player[Projectile.owner].statManaMax)
                {
					Main.player[Projectile.owner].statMana = Main.player[Projectile.owner].statManaMax;
				}

				Main.player[Projectile.owner].ManaEffect(damageDone / 2);

                float rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                for (int i = 0; i < 18; i++)
                {
                    Dust dust = Dust.NewDustPerfect(target.Center, DustID.PortalBolt, newColor: Color.Aqua);
                    dust.velocity = Main.rand.NextVector2CircularEdge(1.5f, 1.5f);
                    dust.noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Item158, Projectile.Center);
            }
        }
	}
}