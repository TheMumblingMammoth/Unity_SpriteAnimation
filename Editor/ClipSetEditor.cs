using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
[CustomEditor(typeof(ClipSet))]
public class ClipSetEditor : Editor {
	
	public override void OnInspectorGUI(){
		ClipSet clipSet = (ClipSet)target;

		DrawDefaultInspector();

		
		if(GUILayout.Button("AddClip")){
			string path = EditorUtility.OpenFilePanel("Choose Clip",Application.dataPath+"/Resources/Clips", "");	
			if(path != null)
				if(path != ""){
					path = path.Remove(0, (Application.dataPath + "/Resources/").Length);
					path = path.Replace(".prefab","");
					clipSet.AddClip( Resources.Load<Clip>(path));
				}
		}

		if(GUILayout.Button("AddClipsFromFolder")){
			string path = EditorUtility.OpenFolderPanel("Choose Folder", Application.dataPath+"/Resources/Clips", "");	
			if(path != null)
				if(path != ""){
					DirectoryInfo d = new DirectoryInfo(path);//Assuming Text is your Folder
					FileInfo[] Files = d.GetFiles("*.prefab"); //Getting Text files
					path = path.Remove(0, (Application.dataPath + "/Resources/").Length);
					if(Files.Length > 0) clipSet.ClearClips();
					else return;
					foreach(FileInfo file in Files )
						LoadClip(path, file.Name);
							
				}
		}

		if(GUILayout.Button("CreateFromFolder")){
			string path = FindFolder(Application.dataPath+"/Sprites", clipSet.name.Replace("ClipSet",""));
			if(path== null)
				EditorUtility.OpenFolderPanel("Choose Folder", Application.dataPath+"/Sprites", "");
			if(path != null)
				if(path != ""){
					DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
					DirectoryInfo[] directories = d.GetDirectories(); //Getting Text files
					foreach(DirectoryInfo directory in directories )
						if(!clipSet.HasClipFor(directory.Name))
							CreateClip(path, directory.Name);
							
				}
		}
		if (GUI.changed){
            EditorUtility.SetDirty(clipSet);
            //EditorSceneManager.MarkSceneDirty(clipSet.gameObject.scene);
        }
	}

	void CreateClip(string path, string file_name){
		ClipSet clipSet = (ClipSet)target;
		Clip temp = ClipEditor.Load(path + "/" + file_name, Instantiate<Clip>(Resources.Load<Clip>("Clips/NullClip"))); 
		bool success;
		path = path.Replace("Sprites", "Resources/Clips");
		Clip clip = PrefabUtility.SaveAsPrefabAsset(temp.gameObject, path + "/" + temp.name+".prefab" , out success).GetComponent<Clip>();
		if(success)
			clipSet.AddClip(clip);
		else
			Debug.Log("Failed to create a clip");
		DestroyImmediate(temp.gameObject);
	}

	void LoadClip(string path, string file_name){
		ClipSet clipSet = (ClipSet)target;
		if(!file_name.EndsWith("ClipSet")){
			string temp = file_name.Replace(".prefab","");
			Clip clip = Resources.Load<Clip>(path+"/"+temp);
			if(temp.StartsWith("Stand"))
				clip.SetDelay(100);
			clipSet.AddClip(clip);
		}
	}

	string FindFolder(string root_path, string folder_name){
		DirectoryInfo root = new DirectoryInfo(root_path);
		DirectoryInfo[] folders = root.GetDirectories();
		string path;
		foreach(DirectoryInfo folder in folders){
			if(folder.Name.Equals(folder_name))
				return folder.FullName;
			path = FindFolder(folder.FullName, folder_name);
			if(path!=null)
				return path;
		}
		return null;
	}
}

