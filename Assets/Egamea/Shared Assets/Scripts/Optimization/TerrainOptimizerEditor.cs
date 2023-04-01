#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Egamea.ConjureScape
{
	[CustomEditor(typeof(TerrainOptimizer))]
	public class TerrainOptimizerEditor : Editor
	{
		/// <summary>
		/// The terrain optimizer MonoBehaviour.
		/// </summary>
		private TerrainOptimizer _terrainOptimizer;
		
		/// <summary>
		/// Method used to draw the Apply button and detail slider in the inspector.
		/// </summary>
		public override void OnInspectorGUI()
		{
			_terrainOptimizer = serializedObject.targetObject as TerrainOptimizer;

			_terrainOptimizer.TerrainDetail =
				EditorGUILayout.IntSlider("Terrain Detail", _terrainOptimizer.TerrainDetail, 1, 10);
			
			if (GUILayout.Button(new GUIContent("Apply")))
			{
				_terrainOptimizer.ApplyTerrainDetail(_terrainOptimizer.TerrainDetail);
			}
		}
	}
}
#endif
