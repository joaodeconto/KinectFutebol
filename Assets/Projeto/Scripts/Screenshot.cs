using UnityEngine;
using System.Collections;

[AddComponentMenu("VisioramaKinect/Screenshot")]
public class Screenshot : MonoBehaviour {
	
	public string diretorio;
	
    string GetScreenshotFilename()
    {
		if (diretorio == null)
			Debug.LogError("Não foi definido um diretório para a Screenshot");
		
        System.IO.Directory.CreateDirectory(diretorio);
        int i=1;
        while (System.IO.File.Exists(System.IO.Path.Combine(diretorio, "Screenshot" + i + ".png"))) {
            i++;
        }
        return System.IO.Path.Combine(diretorio, "Screenshot" + i + ".png");
    }

    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("escape"))) {
            print("Quitting");
            Application.Quit();
        }

        if (Event.current.Equals(Event.KeyboardEvent("f10"))) {
            string filename = GetScreenshotFilename();
            print("Writing screenshot to " + filename);
            Application.CaptureScreenshot(filename);
        }
    }
}
