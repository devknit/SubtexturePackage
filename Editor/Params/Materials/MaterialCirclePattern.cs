
namespace Subtexture
{
	[System.Serializable]
	public class MaterialCirclePattern : MaterialBaseUvProperties
	{
		public override void Reset()
		{
			base.Reset();
			SetUvScale( 8.0f);
		}
		protected override string GetShaderGuid()
		{
			return "a19a0a4fc519fda4c8ca5ea82b12bdce";
		}
	}
}