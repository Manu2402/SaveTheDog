using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    struct CameraLimits
    {
        public float MaxX;
        public float MaxY;
        public float MinX;
        public float MinY;

        public CameraLimits(float maxX, float minX, float maxY, float minY)
        {
            MaxX = maxX;
            MaxY = maxY;
            MinX = minX;
            MinY = minY;
        }
    }

    static class CameraMgr
    {
        private static Dictionary<string, Tuple<Camera, float>> cameras;

        private static Vector2 targetOffset;

        public static GameObject Target;
        public static float CameraSpeed = 5f;
        public static CameraLimits CameraLimits;
        public static Camera MainCamera;

        //public static float HalfDiagonalSquared { get { return MainCamera.pivot.LengthSquared; } }

        public static void Init(CameraLimits cameraLimits)
        {
            MainCamera = new Camera(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            MainCamera.pivot = new Vector2(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);

            targetOffset = new Vector2(0, 0);
            CameraLimits = cameraLimits;

            cameras = new Dictionary<string, Tuple<Camera, float>>();
        }

        public static void SetTarget(GameObject target)
        {
            Target = target;
        }

        public static void Update()
        {
            MainCamera.position = Target.Position; //Position locking
            Vector2 oldCameraPos = MainCamera.position;
            FixPosition();

            Vector2 cameraDelta = MainCamera.position - oldCameraPos;

            if (cameraDelta != Vector2.Zero)
            {
                foreach (var item in cameras)
                {
                    item.Value.Item1.position += cameraDelta * item.Value.Item2;
                }
            }
        }

        public static void AddCamera(string cameraName, Camera camera = null, float cameraSpeed = 0)
        {
            if (camera == null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }

            cameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }

        public static Camera GetCamera(string cameraName)
        {
            if (cameras.ContainsKey(cameraName))
            {
                return cameras[cameraName].Item1;
            }
            return null;
        }

        private static void FixPosition()
        {
            MainCamera.position.X = MathHelper.Clamp(MainCamera.position.X, CameraLimits.MinX, CameraLimits.MaxX);
            MainCamera.position.Y = MathHelper.Clamp(MainCamera.position.Y, CameraLimits.MinY, CameraLimits.MaxY);
        }

        public static void ClearAll()
        {
            cameras.Clear();
        }

    }
}
