using AOT;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;


public struct AudioDataJSON
{
    public string name;
    public string url;
    public string type;
    public AudioClip clip;
}


public class UploadHandler : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ImageUploaderCaptureClick();

    public UnityEvent<AudioClip> OnAudioLoaded;

    private IEnumerator LoadTexture(AudioDataJSON data)
    {
        AudioType type = GetType(data);

        var www = UnityWebRequestMultimedia.GetAudioClip(data.url, type);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            clip.name = data.name;
            HandleData(clip);
        }
        else
        {
            Debug.LogError($"Could not get clip from \"{data.url}\"! Error: {www.error}", this);
        }
    }


    private AudioType GetType(AudioDataJSON data)
    {
        switch (data.type)
        {
            case "audio/wav": return AudioType.WAV;
            case "audio/ogg": return AudioType.OGGVORBIS;
            case "audio/mpeg": return AudioType.MPEG;
        }

        Debug.Log($"Error: Invalid Audio type: {data.type} name: {data.name}");
        return AudioType.UNKNOWN;
    }

    private void HandleData(AudioClip audioData)
    {
        OnAudioLoaded.Invoke(audioData);
    }


    private void FileSelected(string json)
    {
        print(json);
        AudioDataJSON data = JsonUtility.FromJson<AudioDataJSON>(json);
        print(data);
        StartCoroutine(LoadTexture(data));
    }

    public void OnButtonPointerDown()
    {
#if UNITY_EDITOR
        string path = UnityEditor.EditorUtility.OpenFilePanel("Open Audio", "", "mp3");
        if (!System.String.IsNullOrEmpty(path))
        {
            //UnityEditor.EditorUtility.FileSelected("file:///" + path);
            AudioDataJSON data = new()
            {
                name = Path.GetFileNameWithoutExtension(path),
                url = path,
                type = "audio/mpeg"
            };

            StartCoroutine(LoadTexture(data));
        }
#else
        ImageUploaderCaptureClick ();
#endif
    }

    [MonoPInvokeCallback(typeof(Action<string>))]
    public static void CSSharpCallback(string message)
    {
        Debug.Log($"C# callback received \"{message}\"");
    }
}
