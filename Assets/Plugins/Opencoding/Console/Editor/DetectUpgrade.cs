using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Opencoding.Console.Editor
{
	[InitializeOnLoad] 
	public class DetectUpgrade
	{
		static DetectUpgrade()
		{
			EditorApplication.delayCall += ShowDeleteMessage;
		}

		private static void ShowDeleteMessage()
		{
			bool refreshRequired = false;
			if (Directory.Exists("Assets/Opencoding/3rdParty/XcodeAPI"))
			{
				EditorUtility.DisplayDialog("Important TouchConsole Pro Upgrade Information",
					"This version of TouchConsole Pro changes the way Xcode projects are built to reduce conflicts with other plugins.\n\n" +
					"This upgrade process will automatically delete the old system.\n\n" +
					"You MUST now do a clean build of your Xcode project or you will get errors.",
					"Ok");
				AssetDatabase.DeleteAsset("Assets/Opencoding/3rdParty/XcodeAPI");
				refreshRequired = true;
			}

		    if (File.Exists("Assets/Plugins/Android/opencodingconsole.jar"))
		    {
                EditorUtility.DisplayDialog("Important TouchConsole Pro Upgrade Information",
                    "This version of TouchConsole Pro changes how the Android support code is supplied. The old opencodingconsole.jar file will be deleted automatically.",
                    "Ok");
                AssetDatabase.DeleteAsset("Assets/Plugins/Android/opencodingconsole.jar");
                refreshRequired = true;
		    }

		    if (Directory.Exists("Assets/Plugins/iOS/OpenCoding") && Directory.Exists("Assets/Opencoding/Console"))
		    {
			    AssetDatabase.MoveAsset("Assets/Plugins/iOS/OpenCoding", "Assets/Opencoding/Console/iOS");
			    refreshRequired = true;
		    }
		    
		    if (Directory.Exists("Assets/Plugins/Android/OpenCoding"))
		    {
			    EditorUtility.DisplayDialog("Important TouchConsole Pro Upgrade Information",
				    "This version of TouchConsole Pro now uses Google Play Services Resolver to help integrate with Android. To upgrade to this, this system will automatically delete the old library for doing this from Assets/Plugins/Android/OpenCoding",
				    "Ok");
			    AssetDatabase.DeleteAsset("Assets/Plugins/Android/OpenCoding");
			    refreshRequired = true;
		    }

		    if (File.Exists("Assets/Plugins/Android/android-support-v4.jar"))
		    {
			    if (EditorUtility.DisplayDialog("Important TouchConsole Pro Upgrade Information",
				    "This version of TouchConsole Pro no longer requires android-support-v4.jar to build for Android as this library is now provided by the Google Play Services Resolver. Keeping this file will cause build errors, but unfortunately it is a file commonly used by other plugins (e.g. ad & analytics SDKs) so deleting it can cause problems too.\n\nDo you want to delete it?",
				    "Yes", "No"))
			    {
				    AssetDatabase.DeleteAsset("Assets/Plugins/Android/android-support-v4.jar");
				    refreshRequired = true;
			    }
		    }

		    if (refreshRequired)
		    {
			    AssetDatabase.Refresh();
		    }
		}
	}
}