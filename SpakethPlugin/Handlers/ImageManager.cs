using FFXIVClientStructs.FFXIV.Client.UI;

namespace SpakethPlugin.Handlers;

public class ImageManager
{
    public static uint? GetKernelTexture(string imagePath)
    {
        var image = Plugin.TextureProvider.GetFromGame(imagePath);
        return (uint)Plugin.TextureProvider.ConvertToKernelTexture(image.GetWrapOrEmpty());
    }

    public unsafe static void ShowImage(string imagePath, int mode)
    {
        var image = ImageManager.GetKernelTexture(imagePath);
        UIModule.Instance()->ShowImage(image.Value, false, mode, false);
    }
}
