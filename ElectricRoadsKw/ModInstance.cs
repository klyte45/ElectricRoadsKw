using ElectricRoads.UI;
using ElectricRoads.Localization;
using Kwytto.Interfaces;
using Kwytto.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

[assembly: AssemblyVersion("2023.0.0.*")]
namespace ElectricRoads
{
    public class ModInstance : BasicIUserMod<ModInstance, MainController>
    {
        public override string SimpleName { get; } = "Electric Roads";

        public override string Description { get; } = Str.root_modDescription;

        protected override void SetLocaleCulture(CultureInfo culture) => Str.Culture = culture;

        protected override Dictionary<ulong, string> IncompatibleModList => new Dictionary<ulong, string>
        {
        };

        public override string SafeName => "ElectricRoads";

        public override string Acronym => "ER";

        public override Color ModColor { get; } = ColorExtensions.FromRGB("a38b00");

        public enum PatchFlags
        {
            RegularGame = 0x1,
            BP81TilesGame = 0x2,
            Algernon81TilesGame = 0x4,
            Regular_Algernon81TilesGame = 0x5
        }
        internal static PatchFlags m_currentPatched;

        private IUUIButtonContainerPlaceholder[] cachedUUI;
        public override IUUIButtonContainerPlaceholder[] UUIButtons => cachedUUI ?? (cachedUUI = new[]
        {
            new UUIWindowButtonContainerPlaceholder(
                buttonName: Instance.SimpleName,
                tooltip: Instance.GeneralName,
                iconPath: "ModIcon",
                windowGetter: ()=>MainWindow.Instance
             )
        });
        protected override void DoOnLevelUnloading()
        {
            base.DoOnLevelUnloading();
            cachedUUI = null;
        }
    }
}
