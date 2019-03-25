using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			Texture2D texture = entity.scene.content.Load<Texture2D>(@"player/player-spritesheet");
			List<Subtexture> subtextures = Subtexture.subtexturesFromAtlas(texture, 14, 18);

			mover = entity.addComponent(new Mover());
			animation = entity.addComponent(new Sprite<Animations>(subtextures[0]));
			animationKey = Animations.WalkDown;

			animation.addAnimation(Animations.WalkDown, new SpriteAnimation(new List<Subtexture>()
			{
				subtextures[0],
				subtextures[1],
				subtextures[2],
				subtextures[3]
			}));
			animation.addAnimation(Animations.WalkUp, new SpriteAnimation(new List<Subtexture>()
			{
				subtextures[4],
				subtextures[5],
				subtextures[6],
				subtextures[7]
			}));
			animation.addAnimation(Animations.WalkLeft, new SpriteAnimation(new List<Subtexture>()
			{
				subtextures[8],
				subtextures[9],
				subtextures[10],
				subtextures[11]
			}));
			animation.addAnimation(Animations.WalkRight, new SpriteAnimation(new List<Subtexture>()
			{
				subtextures[12],
				subtextures[13],
				subtextures[14],
				subtextures[15]
			}));

			setupInput();
		}

		void setupInput()
		{
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