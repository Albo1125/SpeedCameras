using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedCameras
{
    public class Camera
    {
        public string Name;
        public float X, Y, Z, Direction, SpeedLimit;
        public Vector3 Location => new Vector3(X, Y, Z);

        [JsonIgnore]
        public Blip blip;
    }
}
