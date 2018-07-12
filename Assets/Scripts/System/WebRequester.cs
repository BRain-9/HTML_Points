using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Utils;

public class WebRequester : MonoBehaviour
{
	#region Params
	public string url;
	public static bool _loaded;
	#endregion

	private void Awake()
	{
		RequestMeth();
	}

	public string GetDirectoryListingRegexForUrl(string url)
	{
		if (url.Equals(this.url))
		{
			return "<a href=\".*\">(?<name>.*)</a>";
		}
		throw new NotSupportedException();
	}

	public void RequestMeth()
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
		{
			using (StreamReader reader = new StreamReader(response.GetResponseStream()))
			{
				string html = reader.ReadToEnd();
				Regex regex = new Regex(GetDirectoryListingRegexForUrl(url));
				MatchCollection matches = regex.Matches(html);
				if (matches.Count > 0)
				{
					foreach (Match match in matches)
					{
						if (match.Success && match.Groups["name"].Value != "../")
						{
							this.PrintLog(match.Groups["name"].Value);
							string systemPath =
#if UNITY_EDITOR

								Application.dataPath;
#elif UNITY_ANDROID && !UnityEd
								Application.persistentDataPath;
#endif
							string folder = Constants.GameTags.contentFolder;
							string path = string.Empty;
							if (!Directory.Exists(Path.Combine(systemPath, folder)))
								Directory.CreateDirectory(Path.Combine(systemPath,folder));

							path = Path.Combine(systemPath, folder);
							path = Path.Combine(path,match.Groups["name"].Value);
							this.PrintLog(path);
							WebClient client = new WebClient();
							client.DownloadFile(url +match.Groups["name"].Value
							                    ,path);
						}
					}
					_loaded = true;
				}
			}
		}
	}


}