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
        Quality

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
        Tutorial
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
