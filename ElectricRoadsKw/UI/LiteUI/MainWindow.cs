using ColossalFramework.UI;
using ElectricRoads.Data;
using ElectricRoads.Localization;
using Kwytto.LiteUI;
using Kwytto.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ElectricRoads.UI
{
    internal class MainWindow : GUIOpacityChanging
    {
        protected override bool showOverModals => false;

        protected override bool requireModal => false;

        protected override float FontSizeMultiplier => .9f;
        public static MainWindow Instance
        {
            get
            {
                if (instance is null)
                {
                    var parent = UIView.GetAView();
                    var container = new GameObject();
                    container.transform.SetParent(parent.transform);
                    instance = container.AddComponent<MainWindow>();
                    instance.Init();
                }
                return instance;
            }
        }

        private Dictionary<ItemClass, List<NetInfo>> m_allClasses;

        private void Init()
        {
            Init($"{ModInstance.Instance.Name} - Patch: {ModInstance.m_currentPatched}", new Rect(200, 200, 400, 550), true, true, new Vector2(400, 550));

            m_allClasses = ((FastList<PrefabCollection<NetInfo>.PrefabData>)typeof(PrefabCollection<NetInfo>).GetField("m_scenePrefabs", RedirectorUtils.allFlags).GetValue(null))
            .m_buffer
            .Select(x => x.m_prefab)
            .Where(x => x?.m_class != null && (x.m_class.m_layer == ItemClass.Layer.Default || x.m_class.m_layer == ItemClass.Layer.MetroTunnels || x.m_class.m_layer == ItemClass.Layer.WaterPipes || x.m_class.m_layer == ItemClass.Layer.WaterStructures))
            .GroupBy(x => x.m_class.name)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.First().m_class, x => x.ToList());
            Visible = false;
        }

        public void Start() => Visible = false;

        public static Texture2D BgTextureSubgroup;
        public static Texture2D BgTextureNote;

        public static Color bgSubgroup;
        public static Color bgNote;


        static MainWindow()
        {
            bgSubgroup = ModInstance.Instance.ModColor.SetBrightness(.20f);

            BgTextureSubgroup = new Texture2D(1, 1);
            BgTextureSubgroup.SetPixel(0, 0, new Color(bgSubgroup.r, bgSubgroup.g, bgSubgroup.b, 1));
            BgTextureSubgroup.Apply();


            bgNote = ModInstance.Instance.ModColor.SetBrightness(.60f);
            BgTextureNote = new Texture2D(1, 1);
            BgTextureNote.SetPixel(0, 0, new Color(bgNote.r, bgNote.g, bgNote.b, 1));
            BgTextureNote.Apply();

        }

        protected override void OnWindowClosed()
        {
            base.OnWindowClosed();
        }


        private GUIStyle m_greenButton;
        private GUIStyle m_redButton;
        internal GUIStyle GreenButton
        {
            get
            {
                if (m_greenButton is null)
                {
                    m_greenButton = new GUIStyle(Skin.button)
                    {
                        normal = new GUIStyleState()
                        {
                            background = GUIKwyttoCommons.darkGreenTexture,
                            textColor = Color.white
                        },
                        hover = new GUIStyleState()
                        {
                            background = GUIKwyttoCommons.greenTexture,
                            textColor = Color.black
                        },
                    };
                }
                return m_greenButton;
            }
        }



        internal GUIStyle RedButton
        {
            get
            {
                if (m_redButton is null)
                {
                    m_redButton = new GUIStyle(Skin.button)
                    {
                        normal = new GUIStyleState()
                        {
                            background = GUIKwyttoCommons.darkRedTexture,
                            textColor = Color.white
                        },
                        hover = new GUIStyleState()
                        {
                            background = GUIKwyttoCommons.redTexture,
                            textColor = Color.white
                        },
                    };
                }
                return m_redButton;
            }
        }


        public int CurrentTab { get; private set; } = 0;
        public VehicleInfo CurrentInfo
        {
            get => m_currentInfo; set
            {
                if (m_currentInfo != value)
                {
                    m_currentInfo = value;
                    if (!(value is null))
                    {
                        m_currentInfoList = new List<VehicleInfo>()
                        {
                            m_currentInfo
                        };
                        if (!(m_currentInfo.m_trailers is null))
                        {
                            m_currentInfoList.AddRange(m_currentInfo.m_trailers.Select(x => x.m_info).Distinct().Where(x => x != m_currentInfo));
                        }
                        CurrentTab = 0;
                    }
                }
            }
        }


        private List<VehicleInfo> m_currentInfoList;
        private VehicleInfo m_currentInfo;

        private Texture2D texLoad = KResourceLoader.LoadTextureMod("Load");
        private Texture2D texAll = KResourceLoader.LoadTextureMod("All");
        private Texture2D texNone = KResourceLoader.LoadTextureMod("None");
        private Texture2D texReload = KResourceLoader.LoadTextureMod("Reload");
        private Texture2D texSave = KResourceLoader.LoadTextureMod("Save");

        protected override void DrawWindow(Vector2 area)
        {
            var topArea = new Rect(0, 0, area.x, 42);
            var bottomArea = new Rect(0, 42, area.x, 20);
            var listArea = new Rect(0, 58, area.x, area.y - 62);
            using (new GUILayout.AreaScope(topArea))
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent(texSave, Str.ER_EXPORT_DEFAULT_BTN)))
                    {
                        ClassesData.Instance.SaveAsDefault();
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(new GUIContent(texLoad, Str.ER_IMPORT_DEFAULT_BTN)))
                    {
                        ClassesData.Instance.LoadDefaults(null);
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(new GUIContent(texAll, Str.ER_SELECT_ALL_BTN)))
                    {
                        ClassesData.Instance.SelectAll();
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(new GUIContent(texNone, Str.ER_SELECT_NONE_BTN)))
                    {
                        ClassesData.Instance.UnselectAll();
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(new GUIContent(texReload, Str.ER_RESET_BTN)))
                    {
                        ClassesData.Instance.SafeCleanAll(m_allClasses.Keys);
                    }
                }
            }
            using (new GUILayout.AreaScope(listArea))
            {
                using (var scroll = new GUILayout.ScrollViewScope(scrollPosition))
                {
                    using (new GUILayout.VerticalScope())
                    {
                        foreach (var item in m_allClasses)
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                var oldVal = ClassesData.Instance.GetConductibility(item.Key);
                                var newVal = GUILayout.Toggle(oldVal, item.Key.name);
                                if (oldVal != newVal)
                                {
                                    ClassesData.Instance.SetConductibility(item.Key, newVal);
                                }
                                GUILayout.FlexibleSpace();
                                if (GUILayout.Button("?"))
                                {
                                    var clazz = item.Key;
                                    KwyttoDialog.ShowModal(new KwyttoDialog.BindProperties()
                                    {
                                        showClose = true,
                                        buttons = new[]{
                                                KwyttoDialog.SpaceBtn,
                                                new KwyttoDialog.ButtonDefinition{
                                                    title=Str.ER_ACTIVATE_CLASS_BTN,
                                                    onClick = () =>
                                                    {
                                                        ClassesData.Instance.SetConductibility(clazz, true);
                                                        return true;
                                                    }
                                                },
                                                new KwyttoDialog.ButtonDefinition{
                                                    title=Str.ER_DEACTIVATE_CLASS_BTN,
                                                    onClick = () =>
                                                    {
                                                        ClassesData.Instance.SetConductibility(clazz, false);
                                                        return true;
                                                    }
                                                },
                                                new KwyttoDialog.ButtonDefinition{
                                                    title=Str.ER_RETURN_BTN,
                                                    onClick=()=>true
                                                },
                                            },
                                        title = string.Format(Str.ER_TITLE_NET_LIST_WINDOW, clazz.name),
                                        message = string.Format(Str.ER_PATTERN_NET_LIST_TITLE, Mathf.Min(30, item.Value.Count), item.Value.Count),
                                        scrollText = string.Join("\n", item.Value.Take(30).Select(x => $"\t- {x.GetUncheckedLocalizedTitle()}").ToArray()),
                                    });
                                }
                            }
                        }
                    }
                    scrollPosition = scroll.scrollPosition;
                }
            }
            using (new GUILayout.AreaScope(bottomArea))
            {
                GUILayout.Label(GUI.tooltip);
            }
        }



        private Vector2 scrollPosition;
        private static MainWindow instance;
    }
}
