﻿using System.IO;
using Tilemaps.Scripts.Tiles;
using UnityEditor;
using UnityEngine;

namespace Tilemaps.Editor.Utils
{
    public static class TileBuilder
    {
        public static void CreateTile<T>(LayerType layer, string tileName, string path = null) where T : LayerTile
        {
            CreateAsset(CreateTile<T>(layer), tileName, path);
        }

        public static T CreateTile<T>(LayerType layer) where T : LayerTile
        {
            var tile = ScriptableObject.CreateInstance<T>();
            tile.LayerType = layer;
            return tile;
        }

        public static void CreateAsset(Object asset, string tileName, string path = null)
        {
            var assetPath = Path.Combine(path ?? GetPath(), tileName + ".asset");

            var i = 1;
            while (File.Exists(assetPath))
            {
                assetPath = Path.Combine(path ?? GetPath(), tileName + "_" + i++ + ".asset");
            }

            var folder = Path.GetDirectoryName(assetPath);

            if (folder != null)
            {
                Directory.CreateDirectory(folder);

                AssetDatabase.CreateAsset(asset, assetPath);
            }
        }

        private static string GetPath()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(path), "");
            }

            return path;
        }
    }
}