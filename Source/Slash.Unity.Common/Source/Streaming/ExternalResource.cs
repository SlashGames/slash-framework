namespace Slash.Unity.Common.Streaming
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExternalResource
    {
        public enum ResourceLocation
        {
            StreamingAssets,

            PersistentDataFolder,

            FileSystem
        }

        public const string FilePrefix = "file:///";

        public ResourceLocation Location;

        public string Path;

        public string FullPath
        {
            get
            {
                string fullPath;
                switch (this.Location)
                {
                    case ResourceLocation.StreamingAssets:
                        fullPath = Application.streamingAssetsPath + "/" + this.Path;
#if UNITY_EDITOR || UNITY_STANDALONE
                        fullPath = FilePrefix + fullPath;
#endif
                        break;
                    case ResourceLocation.PersistentDataFolder:
                        fullPath = FilePrefix + Application.persistentDataPath + "/" + this.Path;
                        break;
                    case ResourceLocation.FileSystem:
                        fullPath = FilePrefix + this.Path;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Invalid location", (Exception) null);
                }

                return fullPath;
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
                onError(www.error);
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
    }
}