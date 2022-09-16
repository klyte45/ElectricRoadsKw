using ElectricRoads.UI;
using ICities;
using Klyte.Localization;
using Kwytto.Interfaces;
using Kwytto.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnifiedUI.Helpers;
using UnityEngine;

[assembly: AssemblyVersion("3.0.0.1")]
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

        protected override void OnLevelLoadedInherit(LoadMode mode)
        {
            base.OnLevelLoadedInherit(mode);
            Instance.RegisterMod();
        }

        private UUICustomButton m_modButton;
        public void RegisterMod()
        {
            m_modButton = UUIHelpers.RegisterCustomButton(
             name: SimpleName,
             groupName: "Klyte45",
             tooltip: Name,
             onToggle: (value) => { if (value) { Open(); } else { Close(); } },
             onToolChanged: null,
             icon: KResourceLoader.LoadTexture($"UI.Images.{IconName}.png"),
             hotkeys: new UUIHotKeys { }

             );
            Close();
        }
        internal void Close()
        {
            m_modButton.IsPressed = false;
            MainWindow.Instance.Visible = false;
            m_modButton.Button?.Unfocus();
            ApplyButtonColor();
        }

        internal void ApplyButtonColor() => m_modButton.Button.color = Color.Lerp(Color.gray, m_modButton.IsPressed ? Color.white : Color.black, 0.5f);
        internal void Open()
        {
            m_modButton.IsPressed = true;
            MainWindow.Instance.Visible = true;
            MainWindow.Instance.transform.position = new Vector3(25, 50);
            ApplyButtonColor();
        }
    }
}
