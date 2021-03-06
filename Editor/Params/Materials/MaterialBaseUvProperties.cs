﻿
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialBaseUvProperties : MaterialBase
	{
		public override bool OnGUI()
		{
			bool ret = false;
			
			if( AxisSyncVector2Field( "UV Scale", ref uvScale, ref uvScaleAxisSync) != false)
			{
				ret = true;
			}
			if( AxisSyncVector2Field( "UV Offset", ref uvOffset, ref uvOffsetAxisSync) != false)
			{
				ret = true;
			}
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			materialCache.SetVector( "_UVScale", new Vector4( uvScale.x, uvScale.y, 1, 1));
			materialCache.SetVector( "_UVOffset", new Vector4( uvOffset.x, uvOffset.y, 0, 0));
		}
		
		[SerializeField]
		protected bool uvScaleAxisSync = true;
		[SerializeField]
		protected Vector2 uvScale = new Vector2( 8, 8);
		[SerializeField]
		protected bool uvOffsetAxisSync = false;
		[SerializeField]
		protected Vector2 uvOffset = Vector2.zero;
	}
}