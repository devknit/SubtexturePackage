
namespace Subtexture
{
	[System.Serializable]
	public class MaterialBlockNoise : MaterialBaseUvProperties
	{
		public override void Reset()
		{
			base.Reset();
			SetUvScale( 8.0f);
		}
		protected override string GetShaderGuid()
		{
			return "4dde2b0e60992e84082aaa19cc0edef4";
		}
	}
}