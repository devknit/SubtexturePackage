
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Subtexture
{
	[System.Serializable]
	public class BaseParam
	{
		public BaseParam( string captionName)
		{
			caption = captionName;
		}
		public virtual void OnEnable( EditorWindow window, bool opened)
		{
			if( enabled == null)
			{
				enabled = new AnimBool( opened);
				enabled.valueChanged.AddListener( window.Repaint);
			}
		}
		public virtual void OnDisable()
		{
		}
		public void OnGUI()
		{
			EditorGUILayout.BeginVertical( GUI.skin.box);
			{
				enabled.target = EditorGUILayout.Foldout( enabled.target, caption);
				
				if( EditorGUILayout.BeginFadeGroup( enabled.faded) != false)
				{
					++EditorGUI.indentLevel;
					OnParamGUI();
					--EditorGUI.indentLevel;
				}
				EditorGUILayout.EndFadeGroup();
			}
			EditorGUILayout.EndVertical();
			
			OnAfterGUI();
		}
		protected virtual void OnParamGUI()
		{
		}
		protected virtual void OnAfterGUI()
		{
		}
		
		[SerializeField]
		string caption;
		[SerializeField]
		AnimBool enabled;
	}
}