using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Motorka
{
    class ImageConventer
    {
        ///<summary>
        ///<para>Переделывает путь картинки в биты для хранения в бд</para>
        ///</summary>
        public static async Task<byte[]> ImageToByte(StorageFile file)
        {
            using (var inputStream = await file.OpenSequentialReadAsync())
            {
                var readStream = inputStream.AsStreamForRead();
                byte[] buffer = new byte[readStream.Length];
                await readStream.ReadAsync(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        ///<summary>
        ///<para>Переделывает биты под картинку для отображения в проге</para>
        ///</summary>
        public static async Task<BitmapImage> ByteToImage(byte[] byteArray)
        {
            var bitmapImage = new BitmapImage();
            var stream = new InMemoryRandomAccessStream();
            await stream.WriteAsync(byteArray.AsBuffer());
            stream.Seek(0);
            bitmapImage.SetSource(stream);
            return bitmapImage;
        }

    }
}
