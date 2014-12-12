using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace dousa
{
    class JointPoints
    {
        public DepthImagePoint Head;
        public DepthImagePoint ShoulderCenter;
        public DepthImagePoint ShoulderLeft;
        public DepthImagePoint ShoulderRight;
        public DepthImagePoint ElbowLeft;
        public DepthImagePoint ElbowRight;
        public DepthImagePoint WristLeft;
        public DepthImagePoint WristRight;
        public DepthImagePoint HandLeft;
        public DepthImagePoint HandRight;
        
        public DepthImagePoint Spine;
        public DepthImagePoint HipCenter;
        public DepthImagePoint HipLeft;
        public DepthImagePoint HipRight;

        public DepthImagePoint KneeLeft;
        public DepthImagePoint KneeRight;
        public DepthImagePoint AnkleLeft;
        public DepthImagePoint AnkleRight;
        public DepthImagePoint FootLeft;
        public DepthImagePoint FootRight;

        public JointPoints(Skeleton myskelton, KinectSensor kinect)
        {
            
            Head = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.Head].Position, kinect.DepthStream.Format);
            ShoulderCenter = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.ShoulderCenter].Position, kinect.DepthStream.Format);
            ShoulderLeft = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.ShoulderLeft].Position, kinect.DepthStream.Format);
            ShoulderRight = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.ShoulderRight].Position, kinect.DepthStream.Format);
            ElbowLeft = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.ElbowLeft].Position, kinect.DepthStream.Format);
            ElbowRight = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.ElbowRight].Position, kinect.DepthStream.Format);
            WristLeft = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.WristLeft].Position, kinect.DepthStream.Format);
            WristRight = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.WristRight].Position, kinect.DepthStream.Format);
            HandLeft = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.HandLeft].Position, kinect.DepthStream.Format);
            HandRight = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.HandRight].Position, kinect.DepthStream.Format);

            Spine = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.Spine].Position, kinect.DepthStream.Format);
            HipCenter = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.HipCenter].Position, kinect.DepthStream.Format);
            HipLeft = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.HipLeft].Position, kinect.DepthStream.Format);
            HipRight = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.HipRight].Position, kinect.DepthStream.Format);

            KneeLeft = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.KneeLeft].Position, kinect.DepthStream.Format);
            KneeRight = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.KneeRight].Position, kinect.DepthStream.Format);
            AnkleLeft = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.AnkleLeft].Position, kinect.DepthStream.Format);
            AnkleRight = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.AnkleRight].Position, kinect.DepthStream.Format);
            FootLeft = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.FootLeft].Position, kinect.DepthStream.Format);
            FootRight = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(myskelton.Joints[JointType.FootRight].Position, kinect.DepthStream.Format);

        }
    }
}
