// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace Kopernicus
{
	public class KopernicusUtility
	{
		// Print out a tree containing all the objects in the game
		public static void PerformObjectDump()
		{
			Debug.Log ("--------- Object Dump -----------");
			foreach (GameObject b in GameObject.FindObjectsOfType(typeof (GameObject))) 
			{
				// Essentially, we iterate through all game objects currently alive and search for 
				// the ones without a parent.  Extrememly inefficient and terrible, but its just for
				// exploratory purposes
				if(b.transform.parent == null)
				{
					// Print out the tree of child objects
					GameObjectWalk(b, "");
				}
			}
			Debug.Log ("---------------------------------");
		}
		
		public static void GameObjectWalk(GameObject o, String prefix = "")
		{
			// Print this object
			Debug.Log (prefix + o);
			Debug.Log (prefix + " >>> Components <<< ");
			foreach (Component c in o.GetComponents(typeof(Component))) 
			{
				Debug.Log(prefix + " " + c);
			}
			Debug.Log (prefix + " >>> ---------- <<< ");
			
			// Game objects are related to each other via transforms in Unity3D.
			foreach (Transform b in o.transform) 
			{
				GameObjectWalk(b.gameObject, "    " + prefix);
			}
		}
	}
}
