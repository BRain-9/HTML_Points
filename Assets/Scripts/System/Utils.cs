using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Utils
{
	public static class Logger
	{
		public static void PrintLog(this object obj, object msg, bool isDebug = true, MsgType msgType = MsgType.Info)
		{
			string tempMsg = "[" + obj + "]:" + "\n" + msg;

			bool show = !isDebug ? Debug.isDebugBuild : true;
			switch (msgType)
			{
				case MsgType.Info:
					if (show)
						Debug.Log(tempMsg);
					break;
				case MsgType.Error:
					if (show)
						Debug.LogError(tempMsg);
					break;
				case MsgType.Warning:
					if (show)
						Debug.LogWarning(tempMsg);
					break;
			}
		}
	}

    public enum MsgType {
        Info,
        Error,
        Warning
    }
}
