using UnityEngine;

public static class Define
{
    public static readonly string AxisHorizontal = "Horizontal";
    public static readonly string AxisVertical = "Vertical";

    public static readonly string BNT_Fire = "Fire1";

    public static readonly int ANI_Move = Animator.StringToHash("Move");
    public static readonly int ANI_Die = Animator.StringToHash("Die");

    public static readonly string TAG_Enemy = "Enemy";
    public static readonly string TAG_Player = "Player";
    public static readonly string TAG_GameManager = "GameController";

    public static readonly string SOUND_BGM = "musicVol";
    public static readonly string SOUND_SFX = "sfxVol";
}
