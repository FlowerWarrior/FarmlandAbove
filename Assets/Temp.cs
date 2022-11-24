using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
	Vector3 touchStartCamPos;
	Vector2 touchStart;

	[SerializeField] float ratioCorrection = 48f;
	[SerializeField] float scalingCorrection = 1.9f;
	[SerializeField] float sensitivity = 3;
	[SerializeField] float zoomOutMin = 30;
	[SerializeField] float zoomOutMax = 60;
	[SerializeField] bound xBounds = new bound(-207, 233);
	[SerializeField] bound zBounds = new bound(-221, -125);	

	[System.Serializable]
	struct bound
    {
		public float min;
		public float max;

        public bound(int v1, int v2) : this()
        {
            min = v1;
            max = v2;
        }
    }

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			touchStartCamPos = Camera.main.transform.position;
			touchStart = Input.mousePosition;
		}

		if (Input.GetMouseButton(0))
		{
			Vector2 touchCurrent = Input.mousePosition;
			Vector3 camPosChange = Vector3.zero;

			camPosChange.x = touchStart.x - touchCurrent.x;
			camPosChange.z = touchStart.y - touchCurrent.y;

			Vector3 newPos = touchStartCamPos + camPosChange * sensitivity;

			Camera.main.transform.position = GetClampedPos(newPos);
		}

		// Handle zoom
		if (Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

			float difference = currentMagnitude - prevMagnitude;

			zoom(difference * 0.1f);
		}
		zoom(Input.GetAxis("Mouse ScrollWheel") * 50);
	}

	private void zoom(float increment)
	{
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
		Camera.main.transform.position = GetClampedPos(Camera.main.transform.position);
	}

	private Vector3 GetClampedPos(Vector3 pos)
    {
		float aspectRatio = Camera.main.aspect;
		float currentSize = Camera.main.orthographicSize;
		bound xBoundsTemp;
		xBoundsTemp.min = xBounds.min - (zoomOutMax - currentSize) * scalingCorrection + (aspectRatio-1) * ratioCorrection;
		xBoundsTemp.max = xBounds.max + (zoomOutMax - currentSize) * scalingCorrection - (aspectRatio-1) * ratioCorrection;

		xBoundsTemp.min = Mathf.Clamp(xBoundsTemp.min, -Mathf.Infinity, xBoundsTemp.max);
		xBoundsTemp.max = Mathf.Clamp(xBoundsTemp.max, xBoundsTemp.min, Mathf.Infinity);

		bound zBoundsTemp;
		zBoundsTemp.min = zBounds.min - (zoomOutMax - currentSize) * scalingCorrection;
		zBoundsTemp.max = zBounds.max + (zoomOutMax - currentSize) * scalingCorrection;

		zBoundsTemp.min = Mathf.Clamp(zBoundsTemp.min, -Mathf.Infinity, xBoundsTemp.max);
		zBoundsTemp.max = Mathf.Clamp(zBoundsTemp.max, zBoundsTemp.min, Mathf.Infinity);

		pos.x = Mathf.Clamp(pos.x, xBoundsTemp.min, xBoundsTemp.max);
		pos.z = Mathf.Clamp(pos.z, zBoundsTemp.min, zBoundsTemp.max);
		return pos;
	}
}