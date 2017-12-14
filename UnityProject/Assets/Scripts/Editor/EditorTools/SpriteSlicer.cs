﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteSlicer : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Slice Sprites")]
    private static void SliceSpritesTest()
    {
        var ignoredSB = new StringBuilder();

        var textures = new List<Texture2D>(Resources.LoadAll<Texture2D>("icons"));
        foreach (var spr in textures)
        {
            var path = AssetDatabase.GetAssetPath(spr);
            //cutting off extension:
            var filename = path.Substring(
                path.LastIndexOf("/", StringComparison.Ordinal) + 1
            );

            var SliceWidth = 32;
            var SliceHeight = 32;

            //ignore files starting with digit and not dividable by w/h
            if (char.IsDigit(filename[0]) | (spr.width % SliceWidth != 0) | (spr.height % SliceHeight != 0))
            {
                ignoredSB.AppendLine(path);
                continue;
            }
            var ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;
            ti.spriteImportMode = SpriteImportMode.Multiple;

            var newData = new List<SpriteMetaData>();


            var offset = -1;
            for (var j = spr.height; j > 0; j -= SliceHeight)
            {
                for (var i = 0; i < spr.width; i += SliceWidth)
                {
                    offset++;
                    var smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0.5f);
                    smd.alignment = 9;
                    smd.name = filename.Substring(0, filename.IndexOf(".", StringComparison.Ordinal)) + "_" + offset;
                    smd.rect = new Rect(i, j - SliceHeight, SliceWidth, SliceHeight);

                    newData.Add(smd);
                }
            }

            ti.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        if (ignoredSB.Length != 0)
        {
            ignoredSB.Insert(0, "Following icons were ignored, help yourself and slice them manually:\n");
        }
        ignoredSB.Insert(0, "Done Slicing!\n");
        Debug.Log(ignoredSB.ToString());
    }
#endif
}