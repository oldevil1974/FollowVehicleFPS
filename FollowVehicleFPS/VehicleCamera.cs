using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.ImageEffects;


namespace FollowVehicleFPS
{
    class VehicleCamera : MonoBehaviour
    {
        public bool following = false;
        private ushort followingVehicleId = 0;
        private Camera camera;
        private VehicleManager vehicleMgr;
        private DepthOfField effect;

        void Awake()
        {
            camera = GetComponent<Camera>();
            //camera.fieldOfView = 1.0f;
            vehicleMgr = VehicleManager.instance;

            CameraController cameraController = GetComponent<CameraController>();
            effect = cameraController.GetComponent<DepthOfField>();
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
                    GetNextVehicle();
                    Debug.Log("ywq" + followingVehicleId);
                    if (followingVehicleId != 0 )
                        StartFollowing();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Period))
            {
                if (following)
                {
                    GetNextVehicle();
                    Debug.Log("ywq" + followingVehicleId);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Comma))
            {
                if (following)
                {
                    GetPreVehicle();
                    Debug.Log("ywq" + followingVehicleId);
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
                camera.transform.position = pos + Vector3.up * 3.0f;
                camera.transform.rotation = oritation;

                if(effect)
                    effect.enabled = false;

            }
        }

        public void StartFollowing() {
            following = true;
            Debug.Log("StartFollowing");
        }
        public void StopFollowing() {
            following = false;
            followingVehicleId = 0;
            Debug.Log("StopFollowing ");
        }

        private void GetNextVehicle()
        {
            for(ushort i = (ushort)(followingVehicleId + 1); i < vehicleMgr.m_vehicleCount; i++)
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
                followingVehicleId = i;
                return;
            }
        }
        private void GetPreVehicle()
        {
            for (ushort i = (ushort)(followingVehicleId - 1); i > 0; i--)
            {
                Vehicle vehicle = vehicleMgr.m_vehicles.m_buffer[i];
                if ((vehicle.m_flags & (Vehicle.Flags.Created | Vehicle.Flags.Deleted)) != Vehicle.Flags.Created)
                {
                    continue;
                }
                if (vehicle.Info.m_vehicleAI is CarTrailerAI)
                {
                    continue;
                }
                followingVehicleId = i;
                return;
            }
        }
    }
}
