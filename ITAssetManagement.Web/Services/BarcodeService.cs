using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.Common;
using ZXing.ImageSharp;
using ZXing.ImageSharp.Rendering;

namespace ITAssetManagement.Web.Services
{
    public interface IBarcodeService
    {
        MemoryStream GenerateBarcode(string content);
    }

    public class BarcodeService : IBarcodeService
    {
        public MemoryStream GenerateBarcode(string content)
        {
            var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Width = 300,
                    Height = 150,
                    Margin = 10,
                    PureBarcode = false
                }
            };

            using var image = barcodeWriter.Write(content);
            var memoryStream = new MemoryStream();
            image.Save(memoryStream, new PngEncoder());
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
