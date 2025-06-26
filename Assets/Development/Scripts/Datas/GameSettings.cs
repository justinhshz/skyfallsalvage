using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Game Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    public float movementSpeed = 2f;
    public float countdownDuration = 5f;
    public float defeatTime = 1.5f;
    public float fadeTime = 2f;
    public int scoreThreshold = 100;
}
