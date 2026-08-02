using UnityEngine;

[CreateAssetMenu(menuName = "KaijuPlaza/SceneData")]
public class SceneData : ScriptableObject
{
    public string sceneName;
    public string displayName;
    public Sprite previewImage;
    public AudioClip music;
    public string description;
}
