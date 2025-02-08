using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;



namespace Manament_Store_API.Service
{
    public class ProductCode
    {
        public string GenerateBarcodeWithZXing(string barcodeData)
        {
            var write = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 100,
                    Width = 300,
                    Margin = 10,
                }
            };
            using(Bitmap bitmap = write.Write(barcodeData))
            {
                string fileName = barcodeData + ".png";
                string path = HttpContext.Current.Server.MapPath("~/Barcodes/" + fileName);
                bitmap.Save(path, ImageFormat.Png);
                return "/Barcodes/" + fileName;
            }

        }
    }
}