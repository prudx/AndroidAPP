using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace TimetableAppTest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .EnableLocalScreenshots()
                    .ApkFile("Desktop/TimetableApp.apk")
                    .StartApp();
                //ConfigureApp.Android.StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}

/*
 .EnableLocalScreenshots()
 .ApkFile ("../../../Countr.Droid/bin/Release/<your package name>‐Signed.apk")
 .StartApp();
     */
