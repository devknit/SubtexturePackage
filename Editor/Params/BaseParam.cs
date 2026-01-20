
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Subtexture
{
	[System.Serializable]
	public class BaseParam
	{
		public BaseParam( bool opened)
		{
			m_DefaultOpened = opened;
			m_Enabled = true;
		}
		public virtual void OnEnable( Window window)
		{
			m_Handle = window;
			
			if( m_Opend == null)
			{
				m_Opend = new AnimBool( m_DefaultOpened);
				m_Opend.valueChanged.AddListener( window.Repaint);
			}
		}
		public virtual void OnDisable()
		{
		}
		public void Record( string label)
		{
			m_Handle.Record( label);
		}
		public virtual int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			return 0;
		}
		protected void OnPUI( string caption, bool disableGroup, System.Action onReset, System.Action onGUI)
		{
			EditorGUILayout.BeginVertical( GUI.skin.box);
			{
				EditorGUILayout.BeginHorizontal();
				{
					bool target = EditorGUILayout.Foldout( m_Opend.target, caption);
					if( m_Opend.target.Equals( target) == false)
					{
						Record( "Change Foldout");
						m_Opend.target = target;
					}
					if( disableGroup != false)
					{
						GUILayout.FlexibleSpace();
						bool enabledValue = EditorGUILayout.Toggle( m_Enabled);
						if( m_Enabled.Equals( enabledValue) == false)
						{
							Record( "Change Enabled");
							m_Enabled = enabledValue;
						}
					}
					if( onReset != null)
					{
						GUILayout.FlexibleSpace();
						
						if( GUILayout.Button( "Reset") != false)
						{
							onReset.Invoke();
						}
					}
				}
				EditorGUILayout.EndHorizontal();
				
				if( EditorGUILayout.BeginFadeGroup( m_Opend.faded) != false)
				{
					++EditorGUI.indentLevel;
					
					EditorGUI.BeginDisabledGroup( m_Enabled == false);
					{
						EditorGUILayout.Space( 2);
						onGUI?.Invoke();
					}
					EditorGUI.EndDisabledGroup();
					
					--EditorGUI.indentLevel;
				}
				EditorGUILayout.EndFadeGroup();
			}
			EditorGUILayout.EndVertical();
		}
		public bool Opend
		{
			get => m_Opend?.target ?? false;
		}
		public bool Enabled
		{
			get => m_Enabled;
		}
		[SerializeField]
		bool m_DefaultOpened;
		[SerializeField]
		AnimBool m_Opend;
		[SerializeField]
		bool m_Enabled;
		[System.NonSerialized]
		Window m_Handle;
	}
}