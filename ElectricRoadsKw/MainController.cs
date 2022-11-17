using ColossalFramework;
using ColossalFramework.UI;
using Kwytto.Interfaces;
using Kwytto.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ElectricRoads
{
    public class MainController : BaseController<ModInstance, MainController>
    {
        public static readonly string FOLDER_PATH = KFileUtils.BASE_FOLDER_PATH + "ElectricRoads";
        public static readonly string FOLDER_NAME = "ElectricRoads";
        public static readonly string DEFAULT_CONFIG_FILE = $"{FOLDER_PATH}{Path.DirectorySeparatorChar}DefaultConfiguration.xml";

        private readonly HashSet<ushort> m_segmentsToUpdate = new HashSet<ushort>();

        internal void AddSegmentToUpdateRendererQueue(ushort v)
        {
            if (v != 0)
            {
                m_segmentsToUpdate.Add(v);
            }
        }

        private const int EFFECT_LAYER_LIGHT = 24;

        public void FixedUpdate()
        {
            if (SimulationManager.instance.m_currentTickIndex % 5 == 0)
            {
                var buffS = NetManager.instance.m_segments.m_buffer;
                var buffN = NetManager.instance.m_nodes.m_buffer;
                m_segmentsToUpdate.Select(s =>
                {
                    ref NetSegment ns = ref buffS[s];

                    ushort startNode = ns.m_startNode;
                    ushort endNode = ns.m_endNode;
                    Vector3 position = buffN[startNode].m_position;
                    Vector3 position2 = buffN[endNode].m_position;
                    Vector3 vector = (position + position2) * 0.5f;
                    int num = Mathf.Clamp((int)(vector.x / 64f + 135f), 0, 269);
                    int num2 = Mathf.Clamp((int)(vector.z / 64f + 135f), 0, 269);
                    int x = num * 45 / 270;
                    int z = num2 * 45 / 270;
                    return new Vector2(x, z);
                }).GroupBy(x => x).ForEach(x => Singleton<RenderManager>.instance.UpdateGroup((int)x.Key.x, (int)x.Key.y, EFFECT_LAYER_LIGHT));
                m_segmentsToUpdate.Clear();
            }
        }
    }
}
