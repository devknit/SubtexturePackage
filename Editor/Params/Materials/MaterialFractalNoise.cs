
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialFractalNoise : MaterialBaseUvProperties
	{
		public override void Reset()
		{
			base.Reset();
			SetUvScale( 8.0f);
			m_Brightness = 0.5f;
			m_Octave = 4;
			m_NoiseParams = new NoiseParam[]
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
		}
		public override bool OnGUI()
		{
			bool ret = base.OnGUI();
			
			float brightnessValue = EditorGUILayout.Slider( "Brightness", m_Brightness, 0.0f, 1.0f);
			if( m_Brightness.Equals( brightnessValue) == false)
			{
				Record( "Change Brightness");
				m_Brightness = brightnessValue;
				ret = true;
			}
			int octaveValue = EditorGUILayout.IntSlider( "Octave", m_Octave, 1, 8);
			if( m_Octave.Equals( octaveValue) == false)
			{
				Record( "Change Octave");
				m_Octave = octaveValue;
				ret = true;
			}
			EditorGUILayout.BeginVertical( GUI.skin.box);
			{
				for( int i0 = 0; i0 < m_Octave; ++i0)
				{
					if( m_NoiseParams[ i0].OnGUI( "Element" + i0, Record) != false)
					{
						ret = true;
					}
				}
			}
			EditorGUILayout.EndVertical();
			
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			base.OnUpdateMaterial();
			
			if( Material != null)
			{
				Material.SetFloat( "_Brightness", m_Brightness);
				Material.SetInt( "_Octave", m_Octave);
				
				Material.SetVector( 
					"_Amplitude0123", new Vector4(
						m_NoiseParams[ 0].Amplitude,
						m_NoiseParams[ 1].Amplitude,
						m_NoiseParams[ 2].Amplitude,
						m_NoiseParams[ 3].Amplitude));
				Material.SetVector( 
					"_Amplitude4567", new Vector4(
						m_NoiseParams[ 4].Amplitude,
						m_NoiseParams[ 5].Amplitude,
						m_NoiseParams[ 6].Amplitude,
						m_NoiseParams[ 7].Amplitude));
						
				Material.SetVector( 
					"_Lacunarity0123", new Vector4(
						m_NoiseParams[ 0].Lacunarity,
						m_NoiseParams[ 1].Lacunarity,
						m_NoiseParams[ 2].Lacunarity,
						m_NoiseParams[ 3].Lacunarity));
				Material.SetVector( 
					"_Lacunarity4567", new Vector4(
						m_NoiseParams[ 4].Lacunarity,
						m_NoiseParams[ 5].Lacunarity,
						m_NoiseParams[ 6].Lacunarity,
						m_NoiseParams[ 7].Lacunarity));
						
				Material.SetVector( 
					"_Rotation0123", new Vector4(
						m_NoiseParams[ 0].Rotation,
						m_NoiseParams[ 1].Rotation,
						m_NoiseParams[ 2].Rotation,
						m_NoiseParams[ 3].Rotation));
				Material.SetVector( 
					"_Rotation4567", new Vector4(
						m_NoiseParams[ 4].Rotation,
						m_NoiseParams[ 5].Rotation,
						m_NoiseParams[ 6].Rotation,
						m_NoiseParams[ 7].Rotation));
						
				Material.SetVector( 
					"_Shift01", new Vector4(
						m_NoiseParams[ 0].Shift.x,
						m_NoiseParams[ 0].Shift.y,
						m_NoiseParams[ 1].Shift.x,
						m_NoiseParams[ 1].Shift.y));
				Material.SetVector( 
					"_Shift23", new Vector4(
						m_NoiseParams[ 2].Shift.x,
						m_NoiseParams[ 2].Shift.y,
						m_NoiseParams[ 3].Shift.x,
						m_NoiseParams[ 3].Shift.y));
				Material.SetVector( 
					"_Shift45", new Vector4(
						m_NoiseParams[ 4].Shift.x,
						m_NoiseParams[ 4].Shift.y,
						m_NoiseParams[ 5].Shift.x,
						m_NoiseParams[ 5].Shift.y));
				Material.SetVector( 
					"_Shift67", new Vector4(
						m_NoiseParams[ 6].Shift.x,
						m_NoiseParams[ 6].Shift.y,
						m_NoiseParams[ 7].Shift.x,
						m_NoiseParams[ 7].Shift.y));
			}
		}
		protected override string GetShaderGuid()
		{
			return "b649d9c025e729849bf5138c0dad8d6b";
		}
		[SerializeField]
		float m_Brightness = 0.5f;
		[SerializeField]
		int m_Octave = 4;
		[SerializeField]
		NoiseParam[] m_NoiseParams = new NoiseParam[]
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
				m_Amplitude = amplitude;
				m_Lacunarity = lacunarity;
				m_Rotation = rotation;
				m_Shift = shift;
			}
			public bool OnGUI( string caption, System.Action<string> record)
			{
				bool ret = false;
				
				m_Enabled = EditorGUILayout.Foldout( m_Enabled, caption);
				if( m_Enabled != false)
				{
					++EditorGUI.indentLevel;
					
					float amplitudeValue = EditorGUILayout.FloatField( "Amplitude", m_Amplitude);
					if( m_Amplitude.Equals( amplitudeValue) == false)
					{
						record?.Invoke( "Change Amplitude");
						m_Amplitude = amplitudeValue;
						ret = true;
					}
					float lacunarityValue = EditorGUILayout.FloatField( "Lacunarity", m_Lacunarity);
					if( m_Lacunarity.Equals( lacunarityValue) == false)
					{
						record?.Invoke( "Change Lacunarity");
						m_Lacunarity = lacunarityValue;
						ret = true;
					}
					float rotationValue = EditorGUILayout.FloatField( "Rotation", m_Rotation);
					if( m_Rotation.Equals( rotationValue) == false)
					{
						record?.Invoke( "Change Rotation");
						m_Rotation = rotationValue;
						ret = true;
					}
					Vector2 shiftValue = EditorGUILayout.Vector2Field( "Shift", m_Shift);
					if( m_Shift.Equals( shiftValue) == false)
					{
						record?.Invoke( "Change Shift");
						m_Shift = shiftValue;
						ret = true;
					}
					--EditorGUI.indentLevel;
				}
				return ret;
			}
			public float Amplitude
			{
				get{ return m_Amplitude; }
			}
			public float Lacunarity
			{
				get{ return m_Lacunarity; }
			}
			public float Rotation
			{
				get{ return m_Rotation; }
			}
			public Vector2 Shift
			{
				get{ return m_Shift; }
			}
			[SerializeField]
			bool m_Enabled = true;
			[SerializeField]
			float m_Amplitude;
			[SerializeField]
			float m_Lacunarity;
			[SerializeField]
			float m_Rotation;
			[SerializeField]
			Vector2 m_Shift;
		}
	}
	
}