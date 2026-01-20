
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace Subtexture
{
	[System.Serializable]
	public sealed class SimpleAnimator
	{
		public SimpleAnimator() : this( null, null, 0)
		{
		}
		public SimpleAnimator( Animator animator) : this( animator, null, 0)
		{
		}
		public SimpleAnimator( Animator animator, AnimationClip[] clips, int index)
		{
			m_Graph = PlayableGraph.Create();
			m_Output = AnimationPlayableOutput.Create( m_Graph, "Simple", animator);
			SetClips( clips, index);
		}
		public void Dispose()
		{
			if( m_Playable.IsValid() != false)
			{
				m_Playable.Destroy();
			}
			if( m_Graph.IsValid() != false)
			{
				m_Graph.Destroy();
			}
		}
		public void SetTimeUpdateMode( DirectorUpdateMode value)
		{
			m_Graph.SetTimeUpdateMode( value);
		}
		public void SetOutput( Animator animator)
		{
			m_Output.SetTarget( animator);
		}
		public void SetClips( AnimationClip[] clips, int index)
		{
			m_AnimationClips = clips;
			SetClip( index);
		}
		public void SetClip( int index)
		{
			if( m_AnimationClips != null)
			{
				if( index >= 0 && index < m_AnimationClips.Length)
				{
					SetClip( m_AnimationClips[ index]);
				}
			}
		}
		public void SetClip( AnimationClip clip)
		{
			if( m_Playable.IsValid() != false)
			{
				m_Playable.Destroy();
			}
			m_Playable = AnimationClipPlayable.Create( m_Graph, clip);
			m_Output.SetSourcePlayable( m_Playable);
			m_CurrentAnimationClip = clip;
		}
		public void Play()
		{
			if( m_Graph.IsValid() != false)
			{
				m_Graph.Play();
			}
		}
		public void Stop()
		{
			if( m_Graph.IsValid() != false)
			{
				m_Graph.Stop();
			}
		}
		public void EvaluateTime( float time)
		{
			if( m_Playable.IsValid() != false)
			{
				m_Playable.SetTime( time);
			}
			if( m_Graph.IsValid() != false)
			{
				m_Graph.Evaluate();
			}
		}
		public AnimationClip CurrentClip
		{
			get => m_CurrentAnimationClip;
		}
		public AnimationClip[] Clips
		{
			get => m_AnimationClips;
		}
		
		const string kName = "SimpleAnimator";
		
		[SerializeField]
		PlayableGraph m_Graph;
		[SerializeField]
		AnimationClipPlayable m_Playable;
		[SerializeField]
		AnimationPlayableOutput m_Output;
		[SerializeField]
		AnimationClip[] m_AnimationClips;
		[SerializeField]
		AnimationClip m_CurrentAnimationClip;
	}
}