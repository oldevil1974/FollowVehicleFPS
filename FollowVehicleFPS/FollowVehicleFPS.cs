using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FollowVehicleFPS
{
    public class FollowVehicleFPS : IUserMod
    {
        public String Name => "FollowVehicleFPS";
        public String Description => "Following Vehicle FPS";
    }

    public class ModLoad : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            Debug.Log("FollowVehicleFPS Mod OnLevelLoaded");
            var controller = GameObject.FindObjectOfType<CameraController>();
            controller.gameObject.AddComponent<VehicleCamera>();
        }
        public override void OnLevelUnloading()
        {
            Debug.Log("FollowVehicleFPS Mod OnLevelUnloading");
        }
    }
}
