using Kwytto.Interfaces;
using Kwytto.Utils;
using System.IO;

namespace ElectricRoads
{
    public class MainController : BaseController<ModInstance, MainController>
    {
        public static readonly string FOLDER_PATH = KFileUtils.BASE_FOLDER_PATH + "ElectricRoads";
        public static readonly string FOLDER_NAME = "ElectricRoads";
        public static readonly string DEFAULT_CONFIG_FILE = $"{FOLDER_PATH}{Path.DirectorySeparatorChar}DefaultConfiguration.xml";
    }
}
