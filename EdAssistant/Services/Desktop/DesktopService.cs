using System;
using System.Text;

namespace EdAssistant.Services.Desktop;

class DesktopService : IDesktopService
{
    private readonly StringBuilder _builder = new();

    public void Save()
    {
        //! TODO: implement on linux
        throw new NotImplementedException();
    }

    public void CreateDesktopFile()
    {
        //! TODO: implement on Linux platform
        _builder.AppendLine("[Desktop Entry]")
            .AppendLine("Name=Elite Dangerous assistant")
            .AppendLine("Comment=Elite Dangerous assistant tool")
            .AppendLine("Exec=")
            .AppendLine("Icon=")
            .AppendLine("Terminal=false")
            .AppendLine("Type=Application")
            .AppendLine("Categories=Games");
    }
}