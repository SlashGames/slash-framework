namespace Slash.Unity.Common.Streaming
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using UnityEngine;

    [Serializable]
    public class ExternalResource
    {
        public enum ResourceLocation
        {
            StreamingAssets,

            PersistentDataFolder,

            FileSystem,

            Web
        }

        public const string FilePrefix = "file://";

        public const string WebPrefix = "http://";

        public ResourceLocation Location;

        public string Path;

        public string FullPath
        {
            get { return this.GetFullPath(true); }
        }

        public bool IsLocalResource
        {
            get
            {
                switch (this.Location)
                {
                    case ResourceLocation.StreamingAssets:
                    case ResourceLocation.PersistentDataFolder:
                    case ResourceLocation.FileSystem:
                        return true;
                    case ResourceLocation.Web:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool Exists()
        {
            switch (this.Location)
            {
                case ResourceLocation.StreamingAssets:
                case ResourceLocation.PersistentDataFolder:
                case ResourceLocation.FileSystem:
                    return File.Exists(this.GetFullPath(false));
                case ResourceLocation.Web:
                    HttpWebResponse response = null;
                    var request = (HttpWebRequest) WebRequest.Create(this.GetFullPath(true));
                    request.Method = "HEAD";

                    try
                    {
                        response = (HttpWebResponse) request.GetResponse();
                        return response.StatusCode == HttpStatusCode.OK;
                    }
                    catch (WebException)
                    {
                        /* A WebException will be thrown if the status of the response is not `200 OK` */
                    }
                    finally
                    {
                        // Don't forget to close your response.
                        if (response != null)
                        {
                            response.Close();
                        }
                    }

                    return false;

                default:
                    throw new ArgumentOutOfRangeException("Invalid location", (Exception) null);
            }
        }

        public ExternalResource GetResource(string path)
        {
            return new ExternalResource
            {
                Location = this.Location,
                Path = System.IO.Path.Combine(this.Path, path)
            };
        }

        public IEnumerator Load(Action<WWW> onLoaded, Action<string> onError)
        {
            var www = new WWW(this.FullPath);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                onLoaded(www);
            }
            else
            {
                onError(string.Format("Error loading external resource from '{0}': {1}", this.FullPath, www.error));
            }
        }

        public IEnumerator LoadBytes(Action<byte[]> onLoaded, Action<string> onError)
        {
            yield return this.Load(www => onLoaded(www.bytes), onError);
        }

        public IEnumerator LoadImageIntoTexture(TextureFormat textureFormat, bool mipmap, Action<Texture2D> onLoaded,
            Action<string> onError)
        {
            yield return this.Load(www =>
            {
                var texture = new Texture2D(1701, 2754, textureFormat, mipmap);
                www.LoadImageIntoTexture(texture);
                onLoaded(texture);
            }, onError);
        }

        public IEnumerator LoadText(Action<string> onLoaded, Action<string> onError)
        {
            yield return this.Load(www => onLoaded(www.text), onError);
        }

        public override string ToString()
        {
            return string.Format("Location: {0}, Path: {1}", this.Location, this.Path);
        }

        private string GetFullPath(bool prependPrefix)
        {
            string fullPath;
            switch (this.Location)
            {
                case ResourceLocation.StreamingAssets:
                    fullPath = Application.streamingAssetsPath + "/" + this.Path;
#if UNITY_EDITOR || UNITY_STANDALONE
                    if (prependPrefix)
                    {
                        fullPath = FilePrefix + "/" + fullPath;
                    }
#endif

                    break;
                case ResourceLocation.PersistentDataFolder:
                    fullPath = Application.persistentDataPath + "/" + this.Path;
                    if (prependPrefix)
                    {
#if UNITY_EDITOR || UNITY_STANDALONE
                        fullPath = "/" + fullPath;
#endif
                        fullPath = FilePrefix + fullPath;
                    }

                    break;
                case ResourceLocation.FileSystem:
                    fullPath = this.Path;
                    if (prependPrefix)
                    {
#if UNITY_EDITOR || UNITY_STANDALONE
                        fullPath = "/" + fullPath;
#endif
                        fullPath = FilePrefix + fullPath;
                    }

                    break;
                case ResourceLocation.Web:
                    fullPath = this.Path;
                    if (prependPrefix)
                    {
                        fullPath = WebPrefix + fullPath;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid location", (Exception) null);
            }

            return fullPath;
        }
    }
}