using System;
using UnityEngine;

namespace Slash.Unity.Common.Images
{
    public static class ImageUtils
    {
        private const int DdsHeaderSize = 128;

        private const int EctHeaderSize = 16;

        /// <summary>
        ///     Create a texture from the specified bytes which are considered to be in DDS format (either DXT5 or DXT1
        ///     compressed).
        ///     Taken from http://answers.unity3d.com/questions/555984/can-you-load-dds-textures-during-runtime.html
        /// </summary>
        /// <param name="ddsBytes">File bytes.</param>
        /// <param name="textureFormat">Format (DXT1 or DXT5 expected).</param>
        /// <returns>Texture created from the specified bytes.</returns>
        /// <exception cref="ArgumentException">Thrown if texture format is not DXT1 or DXT5.</exception>
        /// <exception cref="ArgumentException">Thrown if header size is invalid.</exception>
        public static Texture2D LoadTextureDXT(byte[] ddsBytes, TextureFormat textureFormat)
        {
            if (textureFormat != TextureFormat.DXT1 && textureFormat != TextureFormat.DXT5)
            {
                throw new ArgumentException(
                    "Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");
            }

            var ddsSizeCheck = ddsBytes[4];

            // This header byte should be 124 for DDS image files
            if (ddsSizeCheck != 124)
            {
                throw new ArgumentException("Invalid DDS DXTn texture. Unable to read");
            }

            var height = ddsBytes[13]*256 + ddsBytes[12];
            var width = ddsBytes[17]*256 + ddsBytes[16];

            var dxtBytes = new byte[ddsBytes.Length - DdsHeaderSize];
            Buffer.BlockCopy(ddsBytes, DdsHeaderSize, dxtBytes, 0, ddsBytes.Length - DdsHeaderSize);

            var texture = new Texture2D(width, height, textureFormat, true);
            texture.LoadRawTextureData(dxtBytes);
            texture.Apply();

            return texture;
        }

        /// <summary>
        ///     Create a texture from the specified bytes which are considered to be in ECT1 format.
        /// </summary>
        /// <param name="bytes">File bytes.</param>
        /// <param name="textureFormat">Format.</param>
        /// <returns>Texture created from the specified bytes.</returns>
        public static Texture2D LoadTextureETC1(byte[] bytes, TextureFormat textureFormat = TextureFormat.ETC_RGB4)
        {
            var height = bytes[12]*256 + bytes[13];
            var width = bytes[14]*256 + bytes[15];

            const int headerSize = EctHeaderSize;
            var imageBytes = new byte[bytes.Length - headerSize];
            Buffer.BlockCopy(bytes, headerSize, imageBytes, 0, bytes.Length - headerSize);

            var texture = new Texture2D(width, height, textureFormat, true);
            texture.LoadRawTextureData(imageBytes);
            texture.Apply();

            return texture;
        }
    }
}