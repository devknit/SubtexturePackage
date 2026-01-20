
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System.Linq;

namespace Subtexture
{
	[System.Serializable]
	public sealed class AnimationParam : BaseParam
	{
		public AnimationParam() : base( true)
		{
		}
		public void Dispose()
		{
			if( m_Animator != null)
			{
				m_Animator.Dispose();
				m_Animator = null;
			}
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			int refreshCount = 0;
			
			if( Enabled != false)
			{
				if( param[ (int)PreParamType.kMesh] is MeshParam meshParam)
				{
					if( meshParam.MeshType != MeshType.kPrefab)
					{
						return 0;
					}
					if( meshParam.GameObject != null && m_Animator == null)
					{
						var output = meshParam.GameObject.GetComponent<Animator>();
						
						if( output == null)
						{
							output = meshParam.GameObject.AddComponent<Animator>();
						}
						if( output != null && object.ReferenceEquals( output, null) == false)
						{
							AnimationClip[] clips = null;
							m_ClipIndex = 0;
						
							if( output.runtimeAnimatorController != null)
							{
								clips = output.runtimeAnimatorController.animationClips;
							}
							m_Animator = new SimpleAnimator( output, clips, m_ClipIndex);
							m_Animator.SetTimeUpdateMode( DirectorUpdateMode.Manual);
							
							if( m_Clip != null)
							{
								m_Animator.SetClip( m_Clip);
							}
							if( m_Animator.CurrentClip != null)
							{
								if( m_AnimationSeek > m_Animator.CurrentClip.length)
								{
									m_AnimationSeek = m_Animator.CurrentClip.length;
								}
								m_Animator.EvaluateTime( m_AnimationSeek);
								refreshCount = 2;
							}
						}
					}
				}
			}
			OnPUI( "Animation", true, null, () =>
			{
				var clipValue = EditorGUILayout.ObjectField( "Clip", m_Clip, typeof( AnimationClip), false) as AnimationClip;
				if( object.ReferenceEquals( m_Clip, clipValue) == false)
				{
					Record( "Change Clip");
					m_Animator?.SetClip( clipValue);
					m_Clip = clipValue;
				}
				if( m_Animator != null)
				{
					if( (m_Animator.Clips?.Length ?? 0) > 0 && m_Clip == null)
					{
						int clipIndexValue = EditorGUILayout.Popup( "Clips", m_ClipIndex, m_Animator.Clips.Select( x => x.name).ToArray());
						if( m_ClipIndex.Equals( clipIndexValue) == false)
						{
							Record( "Change Clips");
							m_ClipIndex = clipIndexValue;
							m_Animator.SetClip( m_ClipIndex);
							
							if( m_AnimationSeek > m_Animator.CurrentClip.length)
							{
								m_AnimationSeek = m_Animator.CurrentClip.length;
							}
							refreshCount = 2;
						}
					}
					if( m_Animator.CurrentClip != null)
					{
						float animationSeekValue = EditorGUILayout.Slider( "Seconds", m_AnimationSeek, 0.0f, m_Animator.CurrentClip.length);
						if( m_AnimationSeek.Equals( animationSeekValue) == false)
						{
							Record( "Change Seconds");
							m_AnimationSeek = animationSeekValue;
						}
					}
				}
			});
			return refreshCount;
		}
		public void Update()
		{
			if( Enabled != false)
			{
				m_Animator?.EvaluateTime( m_AnimationSeek);
			}
		}
		[SerializeField]
		AnimationClip m_Clip = default;
		[SerializeField]
		int m_ClipIndex = default;
		[SerializeField]
		float m_AnimationSeek = 0.0f;
		[System.NonSerialized]
		SimpleAnimator m_Animator;
	}
}