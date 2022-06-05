using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using System;

namespace SpeedCameras
{
    public class SpeedCameras : BaseScript
    {
        public static Camera[] FixedSpeedCameras;
        public static bool BlipsCreated = false;
        public static float CameraRadius = 25f;
        public static float SpeedingTolerance = 12;
        public SpeedCameras()
        {
            EventHandlers["SpeedCameras:BlipsToggle"] += new Action<dynamic>((dynamic) =>
            {
                if (BlipsCreated)
                {
                    RemoveCameraBlips();
                }
                else
                {
                    CreateCameraBlips();
                }
            });

            EventHandlers["SpeedCameras:Flash"] += new Action<float, float, float, float>((float x, float y, float z, float heading) =>
            {
                if (Game.PlayerPed != null)
                {
                    Flash(x, y, z, heading);
                }
            });

            EventHandlers["SpeedCameras:CameraHit"] += new Action<string>((string msg) =>
            {
                Screen.ShowNotification(msg);
                TriggerEvent("chatMessage", "SpeedCam", new int[] { 255, 255, 0 }, msg);

            });

            string resourceName = API.GetCurrentResourceName();
            string fixedcamerajson = API.LoadResourceFile(resourceName, "cameras.json");
            FixedSpeedCameras = JsonConvert.DeserializeObject<Camera[]>(fixedcamerajson);
            Main();
        }

        public static float mod(float x, float m)
        {
            float r = x % m;
            return r < 0 ? r + m : r;
        }

        public static string DegreesToCardinal(float degrees)
        {
            string[] caridnals = { "N", "NW", "W", "SW", "S", "SE", "E", "NE", "N" };
            return caridnals[(int)Math.Round(mod(degrees, 360) / 45)];
        }
        
        public static bool IsWithinRange(float testAngle, float min, float max)
        {
            testAngle = mod(testAngle, 360);
            min = mod(min, 360);
            max = mod(max, 360);

            if (min > max)
            {
                return testAngle >= min || testAngle <= max;
            }
            else
            {
                return testAngle >= min && testAngle <= max;
            }
        }

        public static float MPStoMPH(float mps)
        {
            return mps * 2.236936f;
        }

        public async void Main()
        {
            DateTime timeLastTriggeredCamera = DateTime.Now;
            Camera lastTriggeredCamera = null;

            while (true)
            {
                await Delay(5);
                if (Game.Player != null && Game.Player.Character != null && Game.Player.Character.Exists() && Game.Player.Character.IsInVehicle() && Game.Player.Character.CurrentVehicle.Exists())
                {
                    Vehicle playerVeh = Game.Player.Character.CurrentVehicle;
                    if (playerVeh.Driver == Game.Player.Character && playerVeh.Speed > 0.2f)
                    {
                        foreach (Camera cam in FixedSpeedCameras)
                        {
                            float ZDiff = Math.Abs(cam.Z - playerVeh.Position.Z);
                            string dir = DegreesToCardinal(playerVeh.Heading);
                            float speed = MPStoMPH(playerVeh.Speed);

                            if (speed >= cam.SpeedLimit + SpeedingTolerance && Vector3.Distance(cam.Location, playerVeh.Position) <= CameraRadius && IsWithinRange(playerVeh.Heading, cam.Direction - 45, cam.Direction + 45) && ZDiff < 2.5f && (lastTriggeredCamera != cam || (DateTime.Now - timeLastTriggeredCamera).Seconds > 5))
                            {
                                //flash
                                string modelName = API.GetDisplayNameFromVehicleModel((uint)API.GetEntityModel(playerVeh.Handle));
                                string plate = API.GetVehicleNumberPlateText(playerVeh.Handle).Replace(" ", string.Empty);
                                string sirenOn = playerVeh.IsSirenActive.ToString();

                                
                                lastTriggeredCamera = cam;
                                timeLastTriggeredCamera = DateTime.Now;
                                int closestCamera = API.GetClosestObjectOfType(playerVeh.Position.X, playerVeh.Position.Y, playerVeh.Position.Z, 30f, 1868764591, false, false, false);
                                if (API.DoesEntityExist(closestCamera))
                                {
                                    Vector3 pos = API.GetEntityCoords(closestCamera, false);
                                    TriggerServerEvent("SpeedCameras:svFlash", pos.X, pos.Y, pos.Z, cam.Direction);
                                }
                                else
                                {
                                    TriggerServerEvent("SpeedCameras:svFlash", cam.X, cam.Y, cam.Z, cam.Direction);
                                }
                                string msg = "SpeedCamHit(" + LocalPlayer.ServerId + "). " + cam.Name + "(" + cam.SpeedLimit + "). " + Math.Round(speed, 1) + " MPH " + modelName + " (" + plate + ") EWS " + sirenOn + ".";
                                TriggerServerEvent("SpeedCameras:svCameraHit", LocalPlayer.ServerId, msg);
                                TriggerServerEvent("SpeedCameras:SpeedCameraHit", plate, cam.Name + " (" + dir + ")", cam.SpeedLimit, Math.Round(speed, 1), playerVeh.IsSirenActive);
                            }
                        }                                                                          
                    }
                }
            }
        }

        public async void Flash(float x, float y, float z, float heading, int time = 15000)
        {
            await Delay(200);
            DateTime dt = System.DateTime.Now;
            Vector3 dirVector = GameMath.HeadingToDirection(heading);
            while (true)
            {

                await Delay(0);

                double timediff = (System.DateTime.Now - dt).TotalMilliseconds;
                if (timediff < 200)
                {
                    API.DrawSpotLight(x, y, z + 2, dirVector.X, dirVector.Y, dirVector.Z, 255, 255, 255, 80f, 100f, 0, 40, 100f);
                } else if (timediff > 600 && timediff < 800)
                {
                    API.DrawSpotLight(x, y, z + 2, dirVector.X, dirVector.Y, dirVector.Z, 255, 255, 255, 80f, 100f, 0, 40, 100f);
                } else if (timediff > 800)
                {
                    break;
                }

            }
        }

        public static void CreateCameraBlips()
        {
            RemoveCameraBlips();
            foreach (Camera cam in FixedSpeedCameras)
            {
                Blip blip = World.CreateBlip(cam.Location);
                blip.Sprite = BlipSprite.Camera;
                blip.Color = BlipColor.White;
                blip.Scale = 0.7f;
                blip.IsShortRange = true;
                blip.Name = "SpeedCam";

                cam.blip = blip;
            }
            BlipsCreated = true;
        }

        public static void RemoveCameraBlips()
        {
            foreach (Camera cam in FixedSpeedCameras)
            {
                if (cam.blip != null && cam.blip.Exists())
                {
                    cam.blip.ShowRoute = false;
                    cam.blip.Delete();
                }
                cam.blip = null;
            }
            BlipsCreated = false;
        }
    }
}
