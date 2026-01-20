
namespace Subtexture
{
	[System.Serializable]
	public class MaterialValueNoise : MaterialBaseUvProperties
	{
		public override void Reset()
		{
			base.Reset();
			SetUvScale( 8.0f);
		}
		protected override string GetShaderGuid()
		{
			return "c213afc6e8574a540b30cd6ef7c038ef";
		}
	}
}