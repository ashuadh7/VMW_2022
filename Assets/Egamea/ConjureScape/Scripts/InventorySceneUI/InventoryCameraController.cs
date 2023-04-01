using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Egamea.ConjureScape.CustomLODSystem;

namespace Egamea.ConjureScape
{
	public class InventoryCameraController : MonoBehaviour
	{
		// Reference to the LOD system tree replacer.
		private TreeReplacer _treeReplacer;

		public TreeReplacer TreeReplacer
		{
			get
			{
				if (!_treeReplacer)
				{
					_treeReplacer = FindObjectOfType<TreeReplacer>();
				}

				return _treeReplacer;
			}
		}

		// Reference to the main scene camera.
		private Camera _sceneCamera;

		public Camera SceneCamera
		{
			get
			{
				if (!_sceneCamera)
				{
					_sceneCamera = Camera.main;
				}

				return _sceneCamera;
			}
		}

		[SerializeField, Tooltip("Reference to the camera position slider.")]
		private Slider _cameraSlider;

		private Transform _treeParent;
		private float _startX = 0f;
		private float _endX = 0f;

		// Use this for initialization
		private IEnumerator Start()
		{
			// Wait for the tree replacer component to be found and for the trees to be placed.
			yield return new WaitUntil(() => TreeReplacer != null);
			yield return new WaitUntil(() => TreeReplacer.TreesHasBeenPlaced);

			_treeParent = GameObject.Find("Trees Parent").transform;
			
			InitialiseBounds();

			_cameraSlider.minValue = _startX;
			_cameraSlider.maxValue = _endX;
			_cameraSlider.value = _startX;
		}

		/// <summary>
		/// Initialise the min max bounds for the camera movement.
		/// </summary>
		private void InitialiseBounds()
		{
			_startX = 0f;
			_endX = 0f;

			for (int i = 0; i < _treeParent.transform.childCount - 1; i++)
			{
				Transform t = _treeParent.GetChild(i);
				if (i == 0)
				{
					_startX = t.position.x;
					_endX = _startX;
				}
				else
				{
					if (t.position.x < _startX)
					{
						_startX = t.position.x;
					}
					
					if (t.position.x > _endX)
					{
						_endX = t.position.x;
					}
				}
			}
		}

		/// <summary>
		/// Updated the the camera X position in the scene according the slider value;
		/// </summary>
		/// <param name="value">X position</param>
		public void UpdateCameraPosition(float value)
		{
			Vector3 currentPosition = SceneCamera.transform.position;
			SceneCamera.transform.position = new Vector3
			{
				x = value,
				y = currentPosition.y,
				z = currentPosition.z
			};
		}
	}
}
