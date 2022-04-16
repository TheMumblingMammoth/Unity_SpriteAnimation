using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Clip))]
public class ClipEditor : Editor {
	static string lastPath;
	[SerializeField] byte delay = 20;
	public override void OnInspectorGUI(){
		Clip clip = (Clip)target;
		if(GUILayout.Button("LoadFromFolder")){
			string path;
			if(lastPath == null)
				path = EditorUtility.OpenFolderPanel("Choose Folder",Application.dataPath+"/Sprites","");	
			else
				path = EditorUtility.OpenFolderPanel("Choose Folder", lastPath ,""); 
			if(path != null)
				if(path != ""){
					clip.LoadFrames(FrameImporter.ImportSprites(path));
					clip.name = path.Split('/')[path.Split('/').Length - 1];
					lastPath = path.Remove(path.Length - clip.name.Length);
				}
					
		}
		
		DrawDefaultInspector();	
		delay  =  (byte) EditorGUILayout.IntField("Delay", delay);
		if(GUILayout.Button("Set Delay"))
			clip.SetDelay(delay);		

		if (GUI.changed){
            EditorUtility.SetDirty(clip);
            EditorSceneManager.MarkSceneDirty(clip.gameObject.scene);
        }
	}

	public static Clip Load(string path, Clip clip){
		if(path != null)
			if(path != ""){
				clip.LoadFrames(FrameImporter.ImportSprites(path));
				clip.name = path.Split('/')[path.Split('/').Length - 1];
				lastPath = path.Remove(path.Length - clip.name.Length);
			}
		return clip;
	}

}

