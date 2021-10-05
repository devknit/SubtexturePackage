
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
			defaultOpened = opened;
			enabled = true;
		}
		public virtual void OnEnable( Window window)
		{
			handle = window;
			
			if( opend == null)
			{
				opend = new AnimBool( defaultOpened);
				opend.valueChanged.AddListener( window.Repaint);
			}
		}
		public virtual void OnDisable()
		{
		}
		public void Record( string label)
		{
			handle.Record( label);
		}
		public virtual int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			return 0;
		}
		protected void OnPUI( string caption, bool disableGroup, System.Action callback)
		{
			EditorGUILayout.BeginVertical( GUI.skin.box);
			{
				EditorGUILayout.BeginHorizontal();
				{
					bool target = EditorGUILayout.Foldout( opend.target, caption);
					if( opend.target.Equals( target) == false)
					{
						Record( "Change Foldout");
						opend.target = target;
					}
					if( disableGroup != false)
					{
						GUILayout.FlexibleSpace();
						bool enabledValue = EditorGUILayout.Toggle( enabled);
						if( enabled.Equals( enabledValue) == false)
						{
							Record( "Change Enabled");
							enabled = enabledValue;
						}
					}
				}
				EditorGUILayout.EndHorizontal();
				
				if( EditorGUILayout.BeginFadeGroup( opend.faded) != false)
				{
					++EditorGUI.indentLevel;
					
					EditorGUI.BeginDisabledGroup( enabled == false);
					{
						callback?.Invoke();
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
			get => opend?.target ?? false;
		}
		public bool Enabled
		{
			get => enabled;
		}
		
		[SerializeField]
		bool defaultOpened;
		[SerializeField]
		AnimBool opend;
		[SerializeField]
		bool enabled;
		[System.NonSerialized]
		Window handle;
	}
}