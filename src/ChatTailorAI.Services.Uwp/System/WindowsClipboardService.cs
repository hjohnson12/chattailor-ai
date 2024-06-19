
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Services.Common;

namespace ChatTailorAI.Services.Uwp.System
{
    public class WindowsClipboardService : IWindowsClipboardService
    {
		public WindowsClipboardService() { }

        public void CopyToClipboard(ChatMessageDto message)
        {
			var dataPackage = new DataPackage();
			var stringBuilder = new StringBuilder();

			// Content is null for image messages, copy all urls instead
			if (message.Content != null)
			{
				dataPackage.SetText(message.Content);
			}
			else if (message is ChatImageMessageDto)
			{
                var imageUrls = (message as ChatImageMessageDto).Images.Select(i => i.Url);
                if (imageUrls != null)
				{
                    foreach (var url in imageUrls)
					{
                        stringBuilder.AppendLine(url);
                    }
                    dataPackage.SetText(stringBuilder.ToString());
                }
            }

			Clipboard.SetContent(dataPackage);
		}

        public async Task<byte[]> GetImageFromClipboardAsync()
        {
            if (Clipboard.GetContent().Contains(StandardDataFormats.Bitmap))
            {
                var bitmap = await Clipboard.GetContent().GetBitmapAsync();
                using (IRandomAccessStream stream = await bitmap.OpenReadAsync())
                {
                    var reader = new DataReader(stream.GetInputStreamAt(0));
                    await reader.LoadAsync((uint)stream.Size);
                    byte[] buffer = new byte[stream.Size];
                    reader.ReadBytes(buffer);

                    return buffer;
                }
            }
            return null;
        }

        public async Task<byte[]> GetImageFromClipboardAsPngAsync()
        {
            if (Clipboard.GetContent().Contains(StandardDataFormats.Bitmap))
            {
                var bitmap = await Clipboard.GetContent().GetBitmapAsync();
                using (IRandomAccessStream stream = await bitmap.OpenReadAsync())
                {
                    var decoder = await BitmapDecoder.CreateAsync(stream);

                    using (var memStream = new InMemoryRandomAccessStream())
                    {
                        // Create an encoder for the PNG format
                        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, memStream);
                        encoder.SetSoftwareBitmap(await decoder.GetSoftwareBitmapAsync());
                        
                        // Flush the data to the in-memory stream
                        await encoder.FlushAsync();

                        // Read the encoded image data from the in-memory stream
                        var reader = new DataReader(memStream.GetInputStreamAt(0));
                        var bytes = new byte[memStream.Size];
                        await reader.LoadAsync((uint)memStream.Size);
                        reader.ReadBytes(bytes);

                        return bytes;
                    }
                }
            }
            return null;
        }
    }
}