using ColossalFramework;
using ICities;
using ElectricRoads.Localization;
using Kwytto.LiteUI;
using Kwytto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElectricRoads.Data
{
    public class ClassesData : ExtensionInterfaceDictionaryStructValSimplImpl<ClassesData, string, bool>
    {
        public override ClassesData LoadDefaults(ISerializableData serializableData)
        {
            if (File.Exists(MainController.DEFAULT_CONFIG_FILE))
            {
                try
                {
                    if (Deserialize(typeof(ClassesData), File.ReadAllBytes(MainController.DEFAULT_CONFIG_FILE)) is ClassesData defaultData)
                    {
                        m_cachedDictDataSaved = defaultData.m_cachedDictDataSaved;
                        eventAllChanged?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    LogUtils.DoErrorLog($"EXCEPTION WHILE LOADING: {e.GetType()} - {e.Message}\n {e.StackTrace}");
                    var scrollText = string.Format(Str.ER_ERROR_LOADING_DEFAULTS_MESSAGE, MainController.DEFAULT_CONFIG_FILE, e.GetType(), e.Message, e.StackTrace);
                    KwyttoDialog.ShowModal(new KwyttoDialog.BindProperties()
                    {
                        icon = ModInstance.Instance.IconName,
                        title = Str.ER_ERROR_LOADING_DEFAULTS_TITLE,
                        scrollText = scrollText,
                        buttons = new[]
                        {
                            new KwyttoDialog.ButtonDefinition
                            {
                                isSpace = true,
                            },
                            new KwyttoDialog.ButtonDefinition
                            {
                                title = Str.ER_OPEN_FOLDER_ON_EXPLORER_BUTTON,
                                onClick = ()=> {
                            ColossalFramework.Utils.OpenInFileBrowser(MainController.FOLDER_PATH);
                                    return false;
                                },
                            },
                            new KwyttoDialog.ButtonDefinition
                            {
                                title = Str.ER_COPY_TO_CLIPBOARD,
                                onClick = ()=> {
                                    Clipboard.text=scrollText;
                                    return false;
                                },
                            },
                            new KwyttoDialog.ButtonDefinition
                            {
                                title = Str.ER_GO_TO_MOD_PAGE_BUTTON,
                                onClick = ()=> {
                                    ColossalFramework.Utils.OpenUrlThreaded("https://steamcommunity.com/sharedfiles/filedetails/?id=1689984220");
                                    return false;
                                },
                            },
                            new KwyttoDialog.ButtonDefinition
                            {
                                title = Str.ER_OK_BUTTON,
                                onClick = ()=>true,
                                style = KwyttoDialog.ButtonStyle.White
                            },
                        },
                    });

                }
            }
            return this;
        }

        public void SaveAsDefault() => File.WriteAllBytes(MainController.DEFAULT_CONFIG_FILE, Serialize());

        public bool GetConductibility(ItemClass clazz)
        {
            bool? val = SafeGet(clazz.name);
            if (val == null)
            {
                val = GetDefaultValueFor(clazz);
                SafeSet(clazz.name, val);
            }
            return val ?? false;
        }

        public void SetConductibility(ItemClass clazz, bool value) => SafeSet(clazz.name, value);

        private static bool GetDefaultValueFor(ItemClass m_class) => m_class.m_service == ItemClass.Service.Electricity
                || m_class.m_service == ItemClass.Service.Road
                || m_class.m_service == ItemClass.Service.Beautification
                || (m_class.m_service == ItemClass.Service.PublicTransport
                    && (m_class.m_subService == ItemClass.SubService.PublicTransportTrain
                        || m_class.m_subService == ItemClass.SubService.PublicTransportTram
                        || m_class.m_subService == ItemClass.SubService.PublicTransportMonorail
                        || m_class.m_subService == ItemClass.SubService.PublicTransportMetro
                        || m_class.m_subService == ItemClass.SubService.PublicTransportPlane
                        || m_class.m_subService == ItemClass.SubService.PublicTransportConcourse)
                    && (m_class.m_layer == ItemClass.Layer.Default || m_class.m_layer == ItemClass.Layer.MetroTunnels));
        public override string SaveId => "K45_ER_ClassesData";

        public event Action eventAllChanged;

        internal void SelectAll()
        {
            var keys = m_cachedDictDataSaved.Keys.ToList();
            foreach (string item in keys)
            {
                m_cachedDictDataSaved[item] = true;
            }
            eventAllChanged?.Invoke();
        }
        internal void UnselectAll()
        {
            var keys = m_cachedDictDataSaved.Keys.ToList();
            foreach (string item in keys)
            {
                m_cachedDictDataSaved[item] = false;
            }
            eventAllChanged?.Invoke();
        }
        internal void SafeCleanAll(IEnumerable<ItemClass> items)
        {
            foreach (ItemClass item in items)
            {
                m_cachedDictDataSaved[item.name] = GetDefaultValueFor(item);
            }
            eventAllChanged?.Invoke();

        }
    }

}
