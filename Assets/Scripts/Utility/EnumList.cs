using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public enum SettingType
    {
        Music,
        Sfx,
        Vibration,
        Quality,
        LevelReached,
        Camera

    }
    public enum SceneName
    {
        MainMenu,
        Loading,
        Play,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        Level10,
        Level11,
        Level12,
        Level13,
        Level14,
        Level15,
        Level16,
        Level17,
        Level18,
        Level19,
        Level20,
        Level21,
        Level22,
        Level23,
        Level24,
        Level25,
        Level26,
        Level27,
        Level28,
        Level29,
        Level30,
        Level31,
        Level32,
        Level33,
        Level34,
        Level35,
        Level36,
        Level37,
        Level38,
        Level39,
        Level40,
        Level41,
        None
    }

    public enum ScreenType
    {
        MainMenu,
        HUD,
        Levels,
        Credits,
        Settings,
        GameOver,
        GameWin,
        Pause,
        None,
        Levels2,
        Levels3,
        Levels4,
        Levels5,
        levels6,
        Tutorial,
        LeaderBoard
    }
    public enum SimpleDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    public enum UIAnimation
    {
        None,
        Slide,
        PopUp
    }

    public enum AudioIdentifier
    {
        audio1,
        audio2,
        None

    }

    public enum PitchType
    {
        defaultPitch,
        tuned,
        random
    }

}
