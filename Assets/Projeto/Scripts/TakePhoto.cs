using UnityEngine;
using System.Collections;

public class TakePhoto : MonoBehaviour
{
	private int resWidth;
	private int resHeight;
	private static float m_tempo = 0;
	private float m_tempoTotal = 15;
	private static bool takePhoto = true;
	public Camera camera;

	void Start () {
		if (null == camera)
			camera = GetComponent<Camera>();
		resWidth  = 800;
		resHeight = 800;
	}

	public static string ScreenShotName ()	{
		//return string.Format ("Screenshots/screen_-_{1}_{2}.png", count, System.DateTime.Now.ToString ("dd-MM-yyyy_HH-mm-ss"));
		return "Screenshot/screen.png";
	}

	public void Update () {
		if (m_tempo > m_tempoTotal)	{
			if (takePhoto) {
				RenderTexture rt = new RenderTexture (resWidth, resHeight, 24);
				camera.targetTexture = rt;
				Texture2D screenShot = new Texture2D (resWidth, resHeight, TextureFormat.RGB24, false);
				camera.Render ();
				RenderTexture.active = rt;
				screenShot.ReadPixels (new Rect (0, 0, resWidth * 2, resHeight), 0, 0);
				camera.targetTexture = null;
				RenderTexture.active = null;
				// JC: added to avoid errors
				Destroy (rt);
				byte[] bytes = screenShot.EncodeToPNG ();
				string filename = ScreenShotName ();
				if(!System.IO.Directory.Exists("Screenshot"))
					System.IO.Directory.CreateDirectory("Screenshot");
				System.IO.File.WriteAllBytes (filename, bytes);
				takePhoto = false;
			}
		}
		else
			m_tempo += Time.deltaTime;
	}
	
	public static void ResetValues () {
		m_tempo = 0;
		takePhoto = true;
	}
}
