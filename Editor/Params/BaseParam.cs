
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Subtexture
{
	[System.Serializable]
	public class BaseParam
	{
		public virtual void OnEnable( Window window, bool opened)
		{
			handle = window;
			if( enabled == null)
			{
				enabled = new AnimBool( opened);
				enabled.valueChanged.AddListener( window.Repaint);
			}
		}
		public virtual void OnDisable()
		{
		}
		public void Record( string label)
		{
			handle.Record( label);
		}
		public virtual void OnGUI()
		{
		}
		protected void OnPUI( string caption, System.Action callback)
		{
			EditorGUILayout.BeginVertical( GUI.skin.box);
			{
				enabled.target = EditorGUILayout.Foldout( enabled.target, caption);
				
				if( EditorGUILayout.BeginFadeGroup( enabled.faded) != false)
				{
					++EditorGUI.indentLevel;
					callback?.Invoke();
					--EditorGUI.indentLevel;
				}
				EditorGUILayout.EndFadeGroup();
			}
			EditorGUILayout.EndVertical();
		}
		
		[SerializeField]
		AnimBool enabled;
		[System.NonSerialized]
		Window handle;
	}
}