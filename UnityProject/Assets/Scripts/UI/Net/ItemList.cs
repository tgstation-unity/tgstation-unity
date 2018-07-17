﻿using System;
using UnityEngine;
using Util;
/// prefab-based for now
/// all server only
public class ItemList : NetUIDynamicList {

	public bool AddItem( string prefabName ) {
		GameObject prefab = Resources.Load( prefabName ) as GameObject;
		return AddItem( prefab );
		
	}

	public bool AddItem( GameObject prefab ) 
	{
		if ( !prefab || !prefab.GetComponent<ItemAttributes>() ) {
			Logger.LogWarning( $"No valid prefab found: {prefab}",Categories.ItemList );
			return false;
		}

		var entryArray = Entries;
		for ( var i = 0; i < entryArray.Length; i++ ) {
			DynamicEntry entry = entryArray[i];
			var item = entry as ItemEntry;
			if ( !item || !item.Prefab || item.Prefab.Equals( prefab ) ) {
				Logger.Log( $"Item {prefab} already exists in ItemList",Categories.ItemList );
				return false;
			}
		}

		//add new entry
		ItemEntry newEntry = Add() as ItemEntry;
		if ( !newEntry ) {
			Logger.LogWarning( $"Added {newEntry} is not an ItemEntry!",Categories.ItemList );
			return false;
		}
		//set its elements
		newEntry.Prefab = prefab;
		Logger.Log( $"ItemList: Item add success! newEntry={newEntry}",Categories.ItemList );

		//rescan elements  and notify
		NetworkTabManager.Instance.Rescan( MasterTab.NetTabDescriptor );
		UpdatePeepers(); 
		
		return true;
	}

	public bool RemoveItem( GameObject prefab ) {
		return RemoveItem( prefab.name );
	}
	public bool RemoveItem( string prefabName ) {
		foreach ( var pair in EntryIndex ) {
			if ( String.Equals( ( (ItemEntry) pair.Value )?.Prefab.name, prefabName,
				StringComparison.CurrentCultureIgnoreCase ) ) 
			{
				Remove( pair.Key );
				return true;
			}
		}
		Logger.LogWarning( $"Didn't find any prefabs called '{prefabName}' in the list",Categories.ItemList);
		return false;
	}
}