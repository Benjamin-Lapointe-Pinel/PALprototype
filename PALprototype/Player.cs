using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.LibGdxAtlases;
using Nez.Sprites;
using Nez.TextureAtlases;
using Nez.TexturePackerImporter;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PALprototype;

namespace paPrototype
{
	class Player : Component, IUpdatable
	{
		enum Animations
		{
			WalkUp,
			WalkDown,
			WalkRight,
			WalkLeft
		}

		private float moveSpeed = 100f;

		private Sprite<Animations> animation;
		private Animations animationKey;
		private SubpixelVector2 subpixelV2 = new SubpixelVector2();
		private Mover mover;
		private VirtualIntegerAxis xAxisInput;
		private VirtualIntegerAxis yAxisInput;

		public override void onAddedToEntity()
		{
			//TODO: factory
			//TODO: test with lgbdx
			TexturePackerAtlas atlas = entity.scene.content.Load<TexturePackerAtlas>(@"player/atlas");
			animation = entity.addComponent(new Sprite<Animations>());
			animation.addAnimation(Animations.WalkDown, atlas.getSpriteAnimation("walk-down"));
			animation.addAnimation(Animations.WalkUp, atlas.getSpriteAnimation("walk-up"));
			animation.addAnimation(Animations.WalkLeft, atlas.getSpriteAnimation("walk-left"));
			animation.addAnimation(Animations.WalkRight, atlas.getSpriteAnimation("walk-right"));
			animationKey = Animations.WalkDown;

			mover = entity.addComponent(new Mover());
			xAxisInput = new VirtualIntegerAxis();
			xAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadDpadLeftRight());
			xAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadLeftStickX());
			xAxisInput.nodes.Add(new Nez.VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.A, Keys.D));
			yAxisInput = new VirtualIntegerAxis();
			yAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadDpadUpDown());
			yAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadLeftStickY());
			yAxisInput.nodes.Add(new Nez.VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.W, Keys.S));
		}

		void IUpdatable.update()
		{
			Vector2 moveDir = new Vector2(xAxisInput.value, yAxisInput.value);

			if (moveDir.X < 0)
				animationKey = Animations.WalkLeft;
			else if (moveDir.X > 0)
				animationKey = Animations.WalkRight;

			if (moveDir.Y < 0)
				animationKey = Animations.WalkUp;
			else if (moveDir.Y > 0)
				animationKey = Animations.WalkDown;


			if (moveDir == Vector2.Zero)
			{
				animation.play(animationKey, 0);
				animation.stop();
			}
			else
			{
				if (!animation.isAnimationPlaying(animationKey))
				{
					animation.play(animationKey, 1);
				}


				var movement = moveDir * moveSpeed * Time.deltaTime;

				mover.calculateMovement(ref movement, out var res);
				subpixelV2.update(ref movement);
				mover.applyMovement(movement);
			}
		}
	}
}