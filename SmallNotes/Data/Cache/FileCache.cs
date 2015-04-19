using Common.Data.Async;
using log4net;
using Newtonsoft.Json;
using SmallNotes.UI.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Core.Entities;

namespace SmallNotes.Data.Cache
{
	public class FileCache
	{
		private ILog Logger { get; set; }

		private const string CacheDataDir = "data";
		private const string CacheDataDescFile = "cache_desc.json";

		private string _CachePath;
		private ConcurrentDictionary<string, CacheObject> _CachedObjects;

		private EventWaitHandle _ReadyEventWaitHandle;

		// TODO Build cache mechanism
		// Cache needs to keep track of URL of file and expiration date
		// Load the file from disk if the expiration time is not passed (cache hit)
		// Load the file from network if expired or not found in the cache (cache miss)

		public FileCache(string cachePath)
		{
			Logger = LogManager.GetLogger(GetType());
			_CachePath = cachePath;

			// Load cache metadata into memory
			_ReadyEventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
			Task.Run(() => {
				try
				{
					JsonSerializer ser = new JsonSerializer();
					using (JsonTextReader reader = new JsonTextReader(new StreamReader(Path.Combine(_CachePath, CacheDataDescFile), Encoding.UTF8)))
					{
						_CachedObjects = ser.Deserialize<ConcurrentDictionary<string, CacheObject>>(reader);
					}
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to deserialize cache metadata", ex);
					_CachedObjects = new ConcurrentDictionary<string, CacheObject>();
				}
				_ReadyEventWaitHandle.Set();
			});
		}

		#region Event handlers

		#region ImageLoad event handlers

		public void ImageLoadHandler(object sender, HtmlImageLoadEventArgs args)
		{
			Uri uri;
			if (Uri.TryCreate(args.Src, UriKind.Absolute, out uri))
			{
				// Make sure the cache is ready
				_ReadyEventWaitHandle.WaitOne();

				// Load the image synchronously
				ImageLoad(uri, (img) => args.Callback(img));
			}
		}

		public void AsyncImageLoadHandler(object sender, HtmlImageLoadEventArgs args)
		{
			Uri uri;
			if (Uri.TryCreate(args.Src, UriKind.Absolute, out uri))
			{
				args.Handled = true;
				Task.Run(() =>
				{
					// Make sure the cache is ready
					_ReadyEventWaitHandle.WaitOne();

					// Call asynchronous cache processing
					ImageLoad(uri, (img) => args.Callback(img));
				});
			}
		}

		#endregion

		#region StylesheetLoad event handlers

		public void StylesheetLoadHandler(object sender, HtmlStylesheetLoadEventArgs args)
		{

		}

		public void AsyncStylesheetLoadHandler(object sender, HtmlStylesheetLoadEventArgs args)
		{

		}

		#endregion

		#endregion

		#region Utility methods

		private void SaveMetadata()
		{
			try
			{
				JsonSerializer ser = new JsonSerializer();
				using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(Path.Combine(_CachePath, CacheDataDescFile), false, Encoding.UTF8)))
				{
					ser.Serialize(writer, _CachedObjects);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to serialize cache metadata", ex);
			}
		}

		private void ImageLoad(Uri src, Action<Image> callback)
		{
			Stream stream = GetUrlToStream(src);
			if (src.Segments.Last().EndsWith(".svg"))
			{
				callback(ImageUtil.LoadSVG(stream));
			}
			else
			{
				callback(Bitmap.FromStream(stream));
			}
		}

		private void StylesheetLoad(HtmlStylesheetLoadEventArgs args)
		{
			// TODO Implement stylesheet caching
		}

		private Stream GetUrlToStream(Uri uri)
		{
			if ("http".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase) || "https".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
			{
				// Check the cache for the file
				if (_CachedObjects.ContainsKey(uri.ToString()) && _CachedObjects[uri.ToString()].Expires > DateTime.Now)
				{
					string cacheFilePath = Path.Combine(_CachePath, CacheDataDir, _CachedObjects[uri.ToString()].CacheFileName);
					if (File.Exists(cacheFilePath))
					{
						return new FileStream(cacheFilePath, FileMode.Open, FileAccess.Read);
					}
				}

				// TODO Load from HTTP

			}
			return new FileStream(uri.ToString(), FileMode.Open, FileAccess.Read); ;
		}

		#endregion

		class CacheObject
		{
			public DateTime Expires { get; set; }
			public string Url { get; set; }
			public string CacheFileName { get; set; }
		}
	}
}
