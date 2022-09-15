using ElectricRoads.Overrides;
using Klyte._commons.Localization;
using Klyte.Localization;
using Kwytto.Interfaces;
using Kwytto.LiteUI;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using static Kwytto.LiteUI.KwyttoDialog;

[assembly: AssemblyVersion("3.0.0.*")]
namespace ElectricRoads
{
    public class ModInstance : BasicIUserMod<ModInstance, MainController>
    {
        public override string SimpleName { get; } = "Electric Roads";

        public override string Description { get; } = Str.root_modDescription;

        protected override void SetLocaleCulture(CultureInfo culture) => Str.Culture = culture;

        protected override Dictionary<ulong, string> IncompatibleModList => new Dictionary<ulong, string>
        {
            [2862121823] = "81 Tiles 2"
        };

        public enum PatchFlags
        {
            RegularGame = 0x1,
            Mod81TilesGame = 0x2
        }
        internal static PatchFlags m_currentPatched;
    }
}
