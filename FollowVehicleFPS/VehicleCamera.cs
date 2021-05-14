using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FollowVehicleFPS
{
    class VehicleCamera : MonoBehaviour
    {
        public bool following = false;
        private ushort followingVehicleId;
        private Camera camera;
        private VehicleManager vehicleMgr;

        void Awake()
        {
            camera = GetComponent<Camera>();
            vehicleMgr = VehicleManager.instance;
        }
        void Start()
        {
            Debug.Log("VehicleCamera Start");
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                if (following)
                {
                    StopFollowing();
                }
                else
                {
                    ushort vehicleid = GetRandomVehicle();
                    if (vehicleid != 0 )
                        StartFollowing(vehicleid);
                }
            }
        }
        void LateUpdate()
        {
            if (following)
            {
                Vehicle vehicle = vehicleMgr.m_vehicles.m_buffer[followingVehicleId];
                Vector3 pos;
                Quaternion oritation;
                vehicle.GetSmoothPosition(followingVehicleId, out pos, out oritation);
                camera.transform.position = pos;
                camera.transform.rotation = oritation;
            }
        }

        public void StartFollowing(ushort vehicleid) {
            following = true;
            followingVehicleId = vehicleid;
            Debug.Log("StartFollowing " + vehicleid);
        }
        public void StopFollowing() {
            following = false;
            Debug.Log("StopFollowing ");
        }

        private ushort GetRandomVehicle()
        {
            for(ushort i = 0; i < vehicleMgr.m_vehicleCount - 1; i++)
            {
                Vehicle vehicle = vehicleMgr.m_vehicles.m_buffer[i];
                if ((vehicle.m_flags & (Vehicle.Flags.Created | Vehicle.Flags.Deleted))!= Vehicle.Flags.Created)
                {
                    continue;
                }
                if(vehicle.Info.m_vehicleAI is CarTrailerAI)
                {
                    continue;
                }
                return i;
            }
            return 0;
        }
    }
}
