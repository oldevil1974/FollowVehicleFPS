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

        private Vector3 cameraRotateEulerAngle = Vector3.zero;
        private Vector3 cameraOffsetLocalPos = Vector3.zero;

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
            if (Input.GetKeyDown(KeyCode.F11))
            {
                if (following)
                {
                    StopFollowing();
                }
                else
                {
                    StartFollowing();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Period))
            {
                if (following)
                {
                    GetNextVehicle();
                    Debug.Log("ywq" + followingVehicleId);
                    cameraOffsetLocalPos = Vector3.zero;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Comma))
            {
                if (following)
                {
                    GetPreVehicle();
                    Debug.Log("ywq" + followingVehicleId);
                    cameraOffsetLocalPos = Vector3.zero;
                }
            }

            cameraRotateEulerAngle.x -= Input.GetAxis("Mouse Y");
            cameraRotateEulerAngle.y += Input.GetAxis("Mouse X");

            //使用局部坐标
            if (Input.GetKey(KeyCode.A))
                cameraOffsetLocalPos -= Vector3.right * 10 * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                cameraOffsetLocalPos += Vector3.right * 10 * Time.deltaTime;
            if (Input.GetKey(KeyCode.W))
                cameraOffsetLocalPos += Vector3.forward * 10 * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                cameraOffsetLocalPos -= Vector3.forward * 10 * Time.deltaTime;
        }
        void LateUpdate()
        {
            if (following)
            {
                Vehicle vehicle = vehicleMgr.m_vehicles.m_buffer[followingVehicleId];
                Vector3 pos;
                Quaternion oritation;
                vehicle.GetSmoothPosition(followingVehicleId, out pos, out oritation);

                //建立局部坐标系
                camera.transform.position = pos + Vector3.up * 3.0f;
                camera.transform.rotation = oritation;

                //局部坐标转换到世界坐标
                Vector3 cameraWorldOffsetPos = camera.transform.TransformPoint(cameraOffsetLocalPos);

                camera.transform.position = cameraWorldOffsetPos;
                camera.transform.Rotate(cameraRotateEulerAngle);


                if (effect)
                    effect.enabled = false;

            }
        }

        public void StartFollowing() {
            followingVehicleId = 0;
            GetNextVehicle();
            Debug.Log("ywq" + followingVehicleId);
            if (followingVehicleId != 0)
            {
                following = true;
                cameraOffsetLocalPos = Vector3.zero;
                Debug.Log("StartFollowing");
            }
        }
        public void StopFollowing() {
            following = false;
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
