
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	public sealed class Window : MDIEditorWindow
	{
		[MenuItem ("Tools/Subtexture/Open &T")]	
		static void ShowWindow() 
		{
			CreateWindow<Window>().Show();	
		}
		protected override void OnEnable()
		{
			base.OnEnable();
			
			if( project == null)
			{
				project = new Project();
			}
			project.OnEnable( this);
			
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			Undo.undoRedoPerformed += OnUndoRedo;
		}
		protected override void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			Undo.undoRedoPerformed -= OnUndoRedo;
			
			if( project != null)
			{
				project.OnDisable();
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
					project.OnEnable( this);
					break;
				}
				case PlayModeStateChange.ExitingEditMode:
				case PlayModeStateChange.ExitingPlayMode:
				{
					project.OnDisable();
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
			project.Update();
		}
		protected override void OnDrawToolBar()
		{
			project.OnToolbarGUI();
		}
		[EWSubWindow( "Preview", EWSubWindowIcon.Scene, true, SubWindowStyle.Preview)]
		void OnPreviewGUI( Rect rect)
		{
			project.OnPreviewGUI( rect);
		}
		[EWSubWindow( "Inspector", EWSubWindowIcon.Inspector)]
		void OnInspectorGUI( Rect rect)
		{
			project.OnInspectorGUI( rect);
		}
		public void Record( string label)
		{
			Undo.RecordObject( this, label);
		}
		
		[SerializeField]
		Project project;
	}
}
