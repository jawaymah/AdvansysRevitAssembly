using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvansysRevitAssembly
{
    public static class Globals
    {
        public static Document Doc { get; set; }

        public static readonly string TempFolder = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.User);
        public static string DNAFolderPath = Path.Combine(TempFolder, "DNA");
        public static string SettingsPath = Path.Combine(DNAFolderPath, "Settings");
        public static string ViewportsDataPath = Path.Combine(DNAFolderPath, "Viewports");
        public static string DNAAppBaseUrl = "https://dnabim.azurewebsites.net/";
        public static string SaveSpacesEndpoint = $"{DNAAppBaseUrl}api/saveForgeSpaces/";
        public static string LicenseUrl = $"{DNAAppBaseUrl}api/revitlicense/";

    }
}
