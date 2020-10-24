
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Experimental.Rendering;
using System.Collections.Generic;

namespace Subtexture
{
	public sealed partial class Project
	{
		void Export( ExportFormat exportFormat, string filename)
		{
			Export( exportFormat, (extension) =>
			{
				return $"{filename}.{extension}";
			});
		}
		void Export( ExportFormat exportFormat)
		{
			Export( exportFormat, (extension) =>
			{
				return EditorUtility.SaveFilePanel( "Subtexture", Application.dataPath, string.Empty, extension);
			});
		}
		void Export( ExportFormat exportFormat, System.Func<string, string> callback)
		{
			string extension = string.Empty;
			
			switch( exportFormat)
			{
				case ExportFormat.kExr16:
				case ExportFormat.kExr16ZIP:
				case ExportFormat.kExr16RLE:
				case ExportFormat.kExr32:
				case ExportFormat.kExr32ZIP:
				case ExportFormat.kExr32RLE:
				{
					extension = "exr";
					break;
				}
				default: /* case ExportFormat.kPng: */
				{
					extension = "png";
					break;
				}
			}
			
			string path = callback?.Invoke( extension) ?? string.Empty;
			if( string.IsNullOrEmpty( path) == false)
			{
				Texture2D texture = null;
				
				switch( exportFormat)
				{
					case ExportFormat.kExr16:
					case ExportFormat.kExr16ZIP:
					case ExportFormat.kExr16RLE:
					{
						texture = new Texture2D( previewTexture.width, previewTexture.height, TextureFormat.RGBAHalf, false, false);
						break;
					}
					case ExportFormat.kExr32:
					case ExportFormat.kExr32ZIP:
					case ExportFormat.kExr32RLE:
					{
						texture = new Texture2D( previewTexture.width, previewTexture.height, TextureFormat.RGBAFloat, false, false);
						break;
					}
					default: /* case ExportFormat.kPng: */
					{
						texture = new Texture2D( previewTexture.width, previewTexture.height, TextureFormat.RGBA32, false, false);
						break;
					}
				}
				if( texture != null)
				{
					var current = RenderTexture.active;
					RenderTexture.active = previewTexture;
					texture.ReadPixels( new Rect( 0, 0, previewTexture.width, previewTexture.height), 0, 0);
					texture.Apply();
					RenderTexture.active = current;
					byte[] bytes = null;
					
					if( exportFormat == ExportFormat.kPng)
					{
						bytes = texture.EncodeToPNG();
					}
					else
					{
						Texture2D.EXRFlags exrFlags = Texture2D.EXRFlags.None;
						
						switch( exportFormat)
						{
							case ExportFormat.kExr16ZIP:
							{
								exrFlags |= Texture2D.EXRFlags.CompressZIP;
								break;
							}
							case ExportFormat.kExr16RLE:
							{
								exrFlags |= Texture2D.EXRFlags.CompressRLE;
								break;
							}
							case ExportFormat.kExr32:
							{
								exrFlags |= Texture2D.EXRFlags.OutputAsFloat;
								break;
							}
							case ExportFormat.kExr32ZIP:
							{
								exrFlags |= Texture2D.EXRFlags.OutputAsFloat;
								exrFlags |= Texture2D.EXRFlags.CompressZIP;
								break;
							}
							case ExportFormat.kExr32RLE:
							{
								exrFlags |= Texture2D.EXRFlags.OutputAsFloat;
								exrFlags |= Texture2D.EXRFlags.CompressRLE;
								break;
							}
						}
						bytes = ImageConversion.EncodeToEXR( texture, exrFlags);
					}
					Texture.DestroyImmediate( texture);
					
					if( bytes != null)
					{
						System.IO.File.WriteAllBytes( path, bytes);
					}
				}
			}
		}
	}
}
