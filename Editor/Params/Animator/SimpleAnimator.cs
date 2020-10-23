
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
			graph = PlayableGraph.Create();
			output = AnimationPlayableOutput.Create( graph, "Simple", animator);
			SetClips( clips, index);
		}
		public void Dispose()
		{
			if( playable.IsValid() != false)
			{
				playable.Destroy();
			}
			if( graph.IsValid() != false)
			{
				graph.Destroy();
			}
		}
		public void SetTimeUpdateMode( DirectorUpdateMode value)
		{
			graph.SetTimeUpdateMode( value);
		}
		public void SetOutput( Animator animator)
		{
			output.SetTarget( animator);
		}
		public void SetClips( AnimationClip[] clips, int index)
		{
			animationClips = clips;
			SetClip( index);
		}
		public void SetClip( int index)
		{
			if( animationClips != null)
			{
				if( index >= 0 && index < animationClips.Length)
				{
					SetClip( animationClips[ index]);
				}
			}
		}
		public void SetClip( AnimationClip clip)
		{
			if( playable.IsValid() != false)
			{
				playable.Destroy();
			}
			playable = AnimationClipPlayable.Create( graph, clip);
			output.SetSourcePlayable( playable);
			currentAnimationClip = clip;
		}
		public void Play()
		{
			if( graph.IsValid() != false)
			{
				graph.Play();
			}
		}
		public void Stop()
		{
			if( graph.IsValid() != false)
			{
				graph.Stop();
			}
		}
		public void EvaluateTime( float time)
		{
			if( playable.IsValid() != false)
			{
				playable.SetTime( time);
			}
			if( graph.IsValid() != false)
			{
				graph.Evaluate();
			}
		}
		public AnimationClip CurrentClip
		{
			get => currentAnimationClip;
		}
		public AnimationClip[] Clips
		{
			get => animationClips;
		}
		
		const string kName = "SimpleAnimator";
		
		[SerializeField]
		PlayableGraph graph;
		[SerializeField]
		AnimationClipPlayable playable;
		[SerializeField]
		AnimationPlayableOutput output;
		[SerializeField]
		AnimationClip[] animationClips;
		[SerializeField]
		AnimationClip currentAnimationClip;
	}
}