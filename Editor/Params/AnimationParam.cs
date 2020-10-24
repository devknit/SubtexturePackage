
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
			if( animator != null)
			{
				animator.Dispose();
				animator = null;
			}
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			int refreshCount = 0;
			
			if( Enabled != false)
			{
				if( param[ (int)PreParamType.kMesh] is MeshParam meshParam)
				{
					if( meshParam.meshType != MeshType.kPrefab)
					{
						return 0;
					}
					if( meshParam.gameObject == null)
					{
						return 0;
					}
					if( animator == null)
					{
						var output = meshParam.gameObject.GetComponent<Animator>();
						
						if( output == null)
						{
							output = meshParam.gameObject.AddComponent<Animator>();
						}
						if( output != null && object.ReferenceEquals( output, null) == false)
						{
							AnimationClip[] clips = null;
							clipIndex = 0;
						
							if( output.runtimeAnimatorController != null)
							{
								clips = output.runtimeAnimatorController.animationClips;
							}
							animator = new SimpleAnimator( output, clips, clipIndex);
							animator.SetTimeUpdateMode( DirectorUpdateMode.Manual);
							
							if( clip != null)
							{
								animator.SetClip( clip);
							}
							if( animator.CurrentClip != null)
							{
								if( animationSeek > animator.CurrentClip.length)
								{
									animationSeek = animator.CurrentClip.length;
								}
								animator.EvaluateTime( animationSeek);
								refreshCount = 2;
							}
						}
					}
				}
			}
			OnPUI( "Animation", true, () =>
			{
				var clipValue = EditorGUILayout.ObjectField( "Clip", clip, typeof( AnimationClip), false) as AnimationClip;
				if( object.ReferenceEquals( clip, clipValue) == false)
				{
					Record( "Change Clip");
					animator?.SetClip( clipValue);
					clip = clipValue;
				}
				if( animator != null)
				{
					if( (animator.Clips?.Length ?? 0) > 0 && clip == null)
					{
						int clipIndexValue = EditorGUILayout.Popup( "Clips", clipIndex, animator.Clips.Select( x => x.name).ToArray());
						if( clipIndex.Equals( clipIndexValue) == false)
						{
							Record( "Change Clips");
							clipIndex = clipIndexValue;
							animator.SetClip( clipIndex);
							
							if( animationSeek > animator.CurrentClip.length)
							{
								animationSeek = animator.CurrentClip.length;
							}
							refreshCount = 2;
						}
					}
					if( animator.CurrentClip != null)
					{
						float animationSeekValue = EditorGUILayout.Slider( "Seconds", animationSeek, 0.0f, animator.CurrentClip.length);
						if( animationSeek.Equals( animationSeekValue) == false)
						{
							Record( "Change Seconds");
							animationSeek = animationSeekValue;
						}
					}
				}
			});
			return refreshCount;
		}
		public void Update()
		{
			if( Enabled != null)
			{
				animator?.EvaluateTime( animationSeek);
			}
		}
		
		[SerializeField]
		AnimationClip clip = default;
		[SerializeField]
		int clipIndex = default;
		[SerializeField]
		float animationSeek = 0.0f;
		[System.NonSerialized]
		SimpleAnimator animator;
	}
}