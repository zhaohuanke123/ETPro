﻿using UnityEngine;

namespace ET
{
    public class PlatformUtil
    {

        public static int intPlatform
        {
            get
            {
                return GameUtility.GetIntPlatform();
            }
        }
        public static string GetStrPlatformIgnoreEditor()
        {
            if (IsIphone())
                return "ios";
            else if (IsAndroid())
                return "android";
            else if (IsWindows())
                return "pc";
#if UNITY_ANDROID
            return "android";
#elif UNITY_IOS
            return "ios";
#endif
            return "pc";
        }
        
        public static bool IsIphone()
        {
            return intPlatform == (int)RuntimePlatform.IPhonePlayer;
        }

        public static bool IsAndroid()
        {
            return intPlatform == (int)RuntimePlatform.Android;
        }

        public static bool IsWindows()
        {
            return intPlatform == (int)RuntimePlatform.WindowsPlayer;
        }

        public static bool IsMobile()
        {
            return IsAndroid() || IsIphone();
        }

        public static string GetAppChannel()
        {
            if (IsAndroid()) return "googleplay";
            else if (IsIphone()) return "applestore";
            else if (IsWindows()) return "steam";
#if UNITY_ANDROID
            return "googleplay";
#elif UNITY_IOS
            return "applestore";
#else 
            return "steam";
#endif
        }
    }
}
