namespace EdAssistant.Helpers;

static class ResourceHelper
{
    public static async Task SaveResourceToFileAsync(string resourceUri, string outputPath)
    {
        try
        {
            var uri = new Uri(resourceUri);
            await using var stream = AssetLoader.Open(uri);
            
            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            await using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
        }
        catch (Exception exception)
        {
            var message = string.Format(Localization.Instance["Exceptions.FailedToSaveResource"], resourceUri, 
                outputPath, exception.Message);
            throw new InvalidOperationException(message, exception);
        }
    }
}