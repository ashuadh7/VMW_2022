using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Egamea.ConjureScape.CustomLODSystem;

#if UNITY_EDITOR
namespace Egamea.ConjureScape
{
	[ExecuteInEditMode]
	public class BillboardHelper : MonoBehaviour
	{
		/// <summary>
		/// The selection of which a billboard needs to be created.
		/// </summary>
		private GameObject _selectedTree;
		
		/// <summary>
		/// The shader of the billboard.
		/// </summary>
		private Shader _billboardShader;
		
		/// <summary>
		/// The main path where the textures will be saved to.
		/// </summary>
		private string _mainPath;
		
		/// <summary>
		/// The current texture ID of the texture being created.
		/// </summary>
		public int CurrentTextureId = 0;
		
		/// <summary>
		/// The camera stand used to rotate around the tree.
		/// </summary>
		private GameObject _cameraStand;
		
		/// <summary>
		/// Is the texture creation process in progress?
		/// </summary>
		[HideInInspector]
		public bool TextureCreationInProgress = false;
		
		/// <summary>
		/// Methos used to create the variations of the billboard textures.
		/// </summary>
		/// <param name="texturePath">The path where the texture needs to be save to.</param>
		/// <param name="selectedObject">The selected tree object of which a billboard is being created.</param>
		public void CreateTextures(string texturePath, GameObject selectedObject)
		{
			_selectedTree = selectedObject;
			_mainPath = texturePath;
			_cameraStand = GameObject.Find("CameraStand");
			CurrentTextureId = 0;
			StartCoroutine(CreateTextureIEnumerator(texturePath, selectedObject, CurrentTextureId));
		}

		/// <summary>
		/// IEnumerator used to create the texture. We need to wait for the texture to arrive.
		/// </summary>
		/// <param name="texturePath">The path where the texture needs to be save to.</param>
		/// <param name="selectedObject">The selected tree object of which a billboard is being created.</param>
		/// <param name="textureID">The current texture ID that needs to be processed.</param>
		/// <returns></returns>
		public IEnumerator CreateTextureIEnumerator(string texturePath, GameObject selectedObject, int textureID)
		{
			TextureCreationInProgress = true;
			
			string path = texturePath + selectedObject.gameObject.name + "_" + CurrentTextureId.ToString() +
			              "_Billboard.png";

			//Creating the actual image.
			ScreenCapture.CaptureScreenshot(path, 1);			
			
			while (!File.Exists(path))
			{
				yield return null;
			}
			
			//Rotate Camera
			Quaternion tempRot = _cameraStand.transform.rotation;
			Vector3 tempEuler = tempRot.eulerAngles;
			tempEuler.y += (360 / 24.0f);
			tempRot.eulerAngles = tempEuler;
			_cameraStand.transform.rotation = tempRot;

			++CurrentTextureId;
			if (CurrentTextureId != 24)
			{
				//Restart the process with the next ID index.
				StartCoroutine(CreateTextureIEnumerator(texturePath, selectedObject, CurrentTextureId));
			}
			else
			{
				TextureCreationInProgress = false;
				
				//When done, Rotate Camera back to 0.0f.
				tempEuler.y = 0.0f;
				tempRot.eulerAngles = tempEuler;
				_cameraStand.transform.rotation = tempRot;
				
				//Lets Refresh the asset database one more time to be safe. 
				AssetDatabase.Refresh();
				AssetDatabase.SaveAssets();
			}
		}

		/// <summary>
		/// Method used to create the actual quad to be used as the billboard.
		/// </summary>
		public void CreatePlane()
		{
			//Create the quad.
			GameObject planesPrefab =
				InstantiatePrefabFromPath("Assets/Egamea/BillboardCreator/Prefabs/Billboard.prefab", "Billboard");
			planesPrefab.gameObject.transform.parent = gameObject.transform;
			planesPrefab.transform.localPosition = Vector3.zero;
			planesPrefab.transform.localScale = Vector3.one;

			BillboardSwitcher switcher = planesPrefab.GetComponent<BillboardSwitcher>();

			for (int i = 0; i < 24; i++)
			{
				//Assign the textures
				string texturePath = _mainPath + _selectedTree.gameObject.name + "_" + i.ToString() + "_Billboard.png";
				Texture2D texture = (Texture2D) AssetDatabase.LoadMainAssetAtPath(texturePath);

				switcher.BillboardTextures[i] = texture;
			}
			
			//Create Material and set up the default values.
			AssetDatabase.CopyAsset("Assets/Egamea/BillboardCreator/Prefabs/DefaultBillboardMaterial.mat", _mainPath + _selectedTree.gameObject.name + "_Billboard.mat");
			AssetDatabase.SaveAssets();

			//Assign the material to the quad and set up the material settings.
			Material createdMaterial = AssetDatabase.LoadAssetAtPath<Material>(_mainPath + _selectedTree.gameObject.name + "_Billboard.mat") as Material;
			createdMaterial.mainTexture = switcher.BillboardTextures[0];
			createdMaterial.SetTexture("_EmissionMap",switcher.BillboardTextures[0]);
			switcher.Quad.GetComponent<Renderer>().material = createdMaterial;
		}
		
		/// <summary>
		/// Method used to instantiate a prefab from a path.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="name"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static GameObject InstantiatePrefabFromPath(string path, string name, Transform parent = null)
		{
			var itemToLoad = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
			var instance = PrefabUtility.InstantiatePrefab(itemToLoad) as GameObject;
			if (parent != null)
				instance.transform.SetParent(parent, false);
			instance.name = name;
			return instance;
		}
	}
}
#endif
