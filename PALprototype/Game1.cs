using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace paPrototype
{
	public class PaPrototype : Core
	{
		public PaPrototype() : base(width: 640, height: 400)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();

			Scene protypeScene = Scene.createWithDefaultRenderer(Color.CornflowerBlue);
			Entity playerEntity = protypeScene.createEntity("player", new Vector2(256 / 2, 224 / 2));
			playerEntity.addComponent(new Player());

			scene = protypeScene;
		}
	}
}