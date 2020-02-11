
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialFractalNoise : MaterialBase
	{
		public override void OnGUI()
		{
			bool update = false;
			
			Vector2 uvScaleValue = EditorGUILayout.Vector2Field( "UV Scale", uvScale);
			if( uvScale.Equals( uvScaleValue) == false)
			{
				Record( "Change UV Scale");
				uvScale = uvScaleValue;
				update = true;
			}
			Vector2 uvOffsetValue = EditorGUILayout.Vector2Field( "UV Offset", uvOffset);
			if( uvOffset.Equals( uvOffsetValue) == false)
			{
				Record( "Change UV Offset");
				uvOffset = uvOffsetValue;
				update = true;
			}
			float brightnessValue = EditorGUILayout.Slider( "Brightness", brightness, 0.0f, 1.0f);
			if( brightness.Equals( brightnessValue) == false)
			{
				Record( "Change Brightness");
				brightness = brightnessValue;
				update = true;
			}
			int octaveValue = EditorGUILayout.IntSlider( "Octave", octave, 1, 8);
			if( octave.Equals( octaveValue) == false)
			{
				Record( "Change Octave");
				octave = octaveValue;
				update = true;
			}
			EditorGUILayout.BeginVertical( GUI.skin.box);
			{
				for( int i0 = 0; i0 < octave; ++i0)
				{
					if( noiseParams[ i0].OnGUI( "Element" + i0, Record) != false)
					{
						update = true;
					}
				}
			}
			EditorGUILayout.EndVertical();
			
			if( update != false)
			{
				OnUpdateMaterial();
			}
		}
		public override void OnUpdateMaterial()
		{
			materialCache.SetVector( "_UVScale", new Vector4( uvScale.x, uvScale.y, 1, 1));
			materialCache.SetVector( "_UVOffset", new Vector4( uvOffset.x, uvOffset.y, 1, 1));
			materialCache.SetFloat( "_Brightness", brightness);
			materialCache.SetInt( "_Octave", octave);
			
			materialCache.SetVector( 
				"_Amplitude0123", new Vector4(
					noiseParams[ 0].amplitude,
					noiseParams[ 1].amplitude,
					noiseParams[ 2].amplitude,
					noiseParams[ 3].amplitude));
			materialCache.SetVector( 
				"_Amplitude4567", new Vector4(
					noiseParams[ 4].amplitude,
					noiseParams[ 5].amplitude,
					noiseParams[ 6].amplitude,
					noiseParams[ 7].amplitude));
					
			materialCache.SetVector( 
				"_Lacunarity0123", new Vector4(
					noiseParams[ 0].lacunarity,
					noiseParams[ 1].lacunarity,
					noiseParams[ 2].lacunarity,
					noiseParams[ 3].lacunarity));
			materialCache.SetVector( 
				"_Lacunarity4567", new Vector4(
					noiseParams[ 4].lacunarity,
					noiseParams[ 5].lacunarity,
					noiseParams[ 6].lacunarity,
					noiseParams[ 7].lacunarity));
					
			materialCache.SetVector( 
				"_Rotation0123", new Vector4(
					noiseParams[ 0].rotation,
					noiseParams[ 1].rotation,
					noiseParams[ 2].rotation,
					noiseParams[ 3].rotation));
			materialCache.SetVector( 
				"_Rotation4567", new Vector4(
					noiseParams[ 4].rotation,
					noiseParams[ 5].rotation,
					noiseParams[ 6].rotation,
					noiseParams[ 7].rotation));
					
			materialCache.SetVector( 
				"_Shift01", new Vector4(
					noiseParams[ 0].shift.x,
					noiseParams[ 0].shift.y,
					noiseParams[ 1].shift.x,
					noiseParams[ 1].shift.y));
			materialCache.SetVector( 
				"_Shift23", new Vector4(
					noiseParams[ 2].shift.x,
					noiseParams[ 2].shift.y,
					noiseParams[ 3].shift.x,
					noiseParams[ 3].shift.y));
			materialCache.SetVector( 
				"_Shift45", new Vector4(
					noiseParams[ 4].shift.x,
					noiseParams[ 4].shift.y,
					noiseParams[ 5].shift.x,
					noiseParams[ 5].shift.y));
			materialCache.SetVector( 
				"_Shift67", new Vector4(
					noiseParams[ 6].shift.x,
					noiseParams[ 6].shift.y,
					noiseParams[ 7].shift.x,
					noiseParams[ 7].shift.y));
				
		}
		protected override string GetShaderGuid()
		{
			return "b649d9c025e729849bf5138c0dad8d6b";
		}
		
		[SerializeField]
		Vector2 uvScale = new Vector2( 8, 8);
		[SerializeField]
		Vector2 uvOffset = Vector2.zero;
		[SerializeField]
		float brightness = 0.5f;
		[SerializeField]
		int octave = 4;
		[SerializeField]
		NoiseParam[] noiseParams = new NoiseParam[]
		{
			new NoiseParam( 0.5f, 1.0f, 0.0f, Vector2.zero),
			new NoiseParam( 0.25f, 2.0f, 0.0f, Vector2.zero),
			new NoiseParam( 0.125f, 2.0f, 0.0f, Vector2.zero),
			new NoiseParam( 0.0625f, 2.0f, 0.0f, Vector2.zero),
			new NoiseParam( 0.03125f, 2.0f, 0.0f, Vector2.zero),
			new NoiseParam( 0.015625f, 2.0f, 0.0f, Vector2.zero),
			new NoiseParam( 0.0078125f, 2.0f, 0.0f, Vector2.zero),
			new NoiseParam( 0.00390625f, 2.0f, 0.0f, Vector2.zero)
		};
		[System.Serializable]
		class NoiseParam
		{
			public NoiseParam( float amplitude, float lacunarity, float rotation, Vector2 shift)
			{
				this.amplitude = amplitude;
				this.lacunarity = lacunarity;
				this.rotation = rotation;
				this.shift = shift;
			}
			public bool OnGUI( string caption, System.Action<string> record)
			{
				bool update = false;
				
				enabled = EditorGUILayout.Foldout( enabled, caption);
				if( enabled != false)
				{
					++EditorGUI.indentLevel;
					
					float amplitudeValue = EditorGUILayout.FloatField( "Amplitude", amplitude);
					if( amplitude.Equals( amplitudeValue) == false)
					{
						record?.Invoke( "Change Amplitude");
						amplitude = amplitudeValue;
						update = true;
					}
					float lacunarityValue = EditorGUILayout.FloatField( "Lacunarity", lacunarity);
					if( lacunarity.Equals( lacunarityValue) == false)
					{
						record?.Invoke( "Change Lacunarity");
						lacunarity = lacunarityValue;
						update = true;
					}
					float rotationValue = EditorGUILayout.FloatField( "Rotation", rotation);
					if( rotation.Equals( rotationValue) == false)
					{
						record?.Invoke( "Change Rotation");
						rotation = rotationValue;
						update = true;
					}
					Vector2 shiftValue = EditorGUILayout.Vector2Field( "Shift", shift);
					if( shift.Equals( shiftValue) == false)
					{
						record?.Invoke( "Change Shift");
						shift = shiftValue;
						update = true;
					}
					--EditorGUI.indentLevel;
				}
				return update;
			}
			[SerializeField]
			bool enabled = true;
			[SerializeField]
			internal float amplitude;
			[SerializeField]
			internal float lacunarity;
			[SerializeField]
			internal float rotation;
			[SerializeField]
			internal Vector2 shift;
		}
	}
	
}