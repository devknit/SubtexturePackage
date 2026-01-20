
using UnityEngine;
using UnityEditor;
using Knit.EditorWindow;

namespace Subtexture
{
	public sealed class Window : MDIEditorWindow
	{
		[MenuItem ("Tools/Subtexture/Open &T")]	
		static void ShowWindow() 
		{
			CreateWindow<Window>( null, "Subtexture").Show();	
		}
		protected override void OnEnable()
		{
			base.OnEnable();
			
			if( m_Project == null)
			{
				m_Project = new Project();
			}
			m_Project.OnEnable( this);
			
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			Undo.undoRedoPerformed += OnUndoRedo;
		}
		protected override void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			Undo.undoRedoPerformed -= OnUndoRedo;
			
			if( m_Project != null)
			{
				m_Project.OnDisable();
			}
			base.OnDisable();
		}
		void OnPlayModeStateChanged( PlayModeStateChange state)
		{
			switch( state)
			{
				case PlayModeStateChange.EnteredEditMode:
				case PlayModeStateChange.EnteredPlayMode:
				{
					m_Project.OnEnable( this);
					break;
				}
				case PlayModeStateChange.ExitingEditMode:
				case PlayModeStateChange.ExitingPlayMode:
				{
					m_Project.OnDisable();
					break;
				}
			}
		}
		void OnUndoRedo()
		{
			OnDisable();
			OnEnable();
		}
		void Update()
		{
			m_Project.Update();
		}
		protected override void OnDrawToolBar()
		{
			m_Project.OnToolbarGUI();
		}
		[SubWindow( "Preview", SubWindowIcon.Scene, true, SubWindowStyle.Preview)]
		void OnPreviewGUI( Rect rect)
		{
			m_Project.OnPreviewGUI( rect);
		}
		[SubWindow( "Inspector", SubWindowIcon.Inspector)]
		void OnInspectorGUI( Rect rect)
		{
			m_Project.OnInspectorGUI( rect);
		}
		public void Record( string label)
		{
			Undo.RecordObject( this, label);
		}
		[SerializeField]
		Project m_Project;
	}
}
