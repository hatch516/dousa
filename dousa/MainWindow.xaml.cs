using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;

namespace dousa
{

    public partial class MainWindow : Window
    {
        KinectSensor kinect;
        public int ElbowLeft, ElbowRight, KneeLeft, KneeRight, ShoulderLeft, ShoulderRight;


        public MainWindow()
        {
            InitializeComponent();
            ElbowLeft = ElbowRight = KneeLeft = KneeRight = ShoulderLeft = ShoulderRight = 0;
            try
            {

                if (KinectSensor.KinectSensors.Count == 0)
                {
                    System.Windows.MessageBox.Show("Kinectが接続されていません");
                }

                kinect = KinectSensor.KinectSensors[0];

                kinect.DepthStream.Enable();
                kinect.ColorStream.Enable();
                kinect.SkeletonStream.Enable(new TransformSmoothParameters()
                {
                    Smoothing = 0.75f,
                    Correction = 0.0f,
                    Prediction = 0.0f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.04f
                });
                
                kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinect_AllFramesReady);
                kinect.Start();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame == null)
                {
                    return;
                }
                RGBcam.Source = colorFrame.ToBitmapSource();
            }

            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    skeletoncanvas.Children.Clear();

                    Skeleton[] skeletonData = new Skeleton[frame.SkeletonArrayLength];
                    
                    frame.CopySkeletonDataTo(skeletonData);

                    foreach (var skeleton in skeletonData)
                    {
                        if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            JointPoints jp = new JointPoints(skeleton, kinect);

                            SkeletonDraw(jp);
                            SkeletonTOPDraw(jp);
                            judge(jp);

                        }
                    }
                }
            }
        }

        private void judge(JointPoints jp)
        {
            double angle_ElbowLeft, angle_ElbowRight, angle_KneeLeft, angle_KneeRight, angle_ShoulderLeft, angle_ShoulderRight, haibu;

            
            
            if (CheckBox_ElbowLeft.IsChecked == true)
            {
                angle_ElbowLeft = calculation(jp.ShoulderLeft, jp.ElbowLeft, jp.WristLeft);
                EllipseDraw(jp.ElbowLeft);

                if (angle_ElbowLeft < 100)
                {
                    EllipseDraw_Fill(jp.ElbowLeft);
                }
            }
           
            if (CheckBox_ElbowRight.IsChecked == true)
            {
                angle_ElbowRight = calculation(jp.ShoulderRight, jp.ElbowRight, jp.WristRight);
                EllipseDraw(jp.ElbowRight);

                if (angle_ElbowRight < 100)
                {
                    EllipseDraw_Fill(jp.ElbowRight);
                }
            }

            if (CheckBox_KneeLeft.IsChecked == true)
            {
                angle_KneeLeft = calculation(jp.HipLeft, jp.KneeLeft, jp.AnkleLeft);
                EllipseDraw(jp.KneeLeft);

                if (angle_KneeLeft < 100)
                {
                    EllipseDraw_Fill(jp.KneeLeft);
                }
            }

            if (CheckBox_KneeRight.IsChecked == true)
            {
                angle_KneeRight = calculation(jp.HipRight, jp.KneeRight, jp.AnkleRight);
                EllipseDraw(jp.KneeRight);

                if (angle_KneeRight < 100)
                {
                    EllipseDraw_Fill(jp.KneeRight);
                }
            }

            if (CheckBox_ShoulderLeft.IsChecked == true)
            {
                angle_ShoulderLeft = calculation(jp.ShoulderCenter, jp.ShoulderLeft, jp.ElbowLeft);

                EllipseDraw(jp.ShoulderLeft);

                if (jp.ShoulderLeft.Y > jp.ElbowLeft.Y)
                {
                    EllipseDraw_Fill(jp.ShoulderLeft);
                }
            }

            if (CheckBox_ShoulderRight.IsChecked == true)
            {
                angle_ShoulderRight = calculation(jp.ShoulderCenter, jp.ShoulderRight, jp.ElbowRight);
                EllipseDraw(jp.ShoulderRight);

                if (jp.ShoulderRight.Y > jp.ElbowRight.Y)
                {
                    EllipseDraw_Fill(jp.ShoulderRight);
                }
            }
        }

        private double calculation(DepthImagePoint a, DepthImagePoint b, DepthImagePoint c)
        {
            double Ax, Ay, Bx, By, angle,d;

            Ax = (double)a.X - (double)b.X;
            Ay = (double)a.Y - (double)b.Y;
            Bx = (double)c.X- (double)b.X;
            By = (double)c.Y - (double)b.Y;

            angle = ((Math.Acos(((Ax * Bx) + (Ay * By) ) / Math.Sqrt(((Ax * Ax) + (Ay * Ay) ) * ((Bx * Bx) + (By * By))))) / Math.PI) * 180;
            d = (Ax * By) - (Ay * Bx);

            return angle;

        }

        private void SkeletonDraw(JointPoints jp)
        {
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.Head.X,
                Y1 = jp.Head.Y,
                X2 = jp.ShoulderCenter.X,
                Y2 = jp.ShoulderCenter.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.ShoulderLeft.X,
                Y1 = jp.ShoulderLeft.Y,
                X2 = jp.ShoulderCenter.X,
                Y2 = jp.ShoulderCenter.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.ShoulderRight.X,
                Y1 = jp.ShoulderRight.Y,
                X2 = jp.ShoulderCenter.X,
                Y2 = jp.ShoulderCenter.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.ShoulderLeft.X,
                Y1 = jp.ShoulderLeft.Y,
                X2 = jp.ElbowLeft.X,
                Y2 = jp.ElbowLeft.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.WristLeft.X,
                Y1 = jp.WristLeft.Y,
                X2 = jp.ElbowLeft.X,
                Y2 = jp.ElbowLeft.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.WristLeft.X,
                Y1 = jp.WristLeft.Y,
                X2 = jp.HandLeft.X,
                Y2 = jp.HandLeft.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.ShoulderRight.X,
                Y1 = jp.ShoulderRight.Y,
                X2 = jp.ElbowRight.X,
                Y2 = jp.ElbowRight.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.WristRight.X,
                Y1 = jp.WristRight.Y,
                X2 = jp.ElbowRight.X,
                Y2 = jp.ElbowRight.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.WristRight.X,
                Y1 = jp.WristRight.Y,
                X2 = jp.HandRight.X,
                Y2 = jp.HandRight.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.Spine.X,
                Y1 = jp.Spine.Y,
                X2 = jp.ShoulderCenter.X,
                Y2 = jp.ShoulderCenter.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.Spine.X,
                Y1 = jp.Spine.Y,
                X2 = jp.HipCenter.X,
                Y2 = jp.HipCenter.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.HipLeft.X,
                Y1 = jp.HipLeft.Y,
                X2 = jp.HipCenter.X,
                Y2 = jp.HipCenter.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.HipLeft.X,
                Y1 = jp.HipLeft.Y,
                X2 = jp.KneeLeft.X,
                Y2 = jp.KneeLeft.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.AnkleLeft.X,
                Y1 = jp.AnkleLeft.Y,
                X2 = jp.KneeLeft.X,
                Y2 = jp.KneeLeft.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.AnkleLeft.X,
                Y1 = jp.AnkleLeft.Y,
                X2 = jp.FootLeft.X,
                Y2 = jp.FootLeft.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.HipRight.X,
                Y1 = jp.HipRight.Y,
                X2 = jp.HipCenter.X,
                Y2 = jp.HipCenter.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.HipRight.X,
                Y1 = jp.HipRight.Y,
                X2 = jp.KneeRight.X,
                Y2 = jp.KneeRight.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.AnkleRight.X,
                Y1 = jp.AnkleRight.Y,
                X2 = jp.KneeRight.X,
                Y2 = jp.KneeRight.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.AnkleRight.X,
                Y1 = jp.AnkleRight.Y,
                X2 = jp.FootRight.X,
                Y2 = jp.FootRight.Y,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
        }

        private void SkeletonTOPDraw(JointPoints jp)
        {
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.Head.X,
                Y1 = jp.Head.Depth,
                X2 = jp.ShoulderCenter.X,
                Y2 = jp.ShoulderCenter.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.ShoulderLeft.X,
                Y1 = jp.ShoulderLeft.Depth,
                X2 = jp.ShoulderCenter.X,
                Y2 = jp.ShoulderCenter.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.ShoulderRight.X,
                Y1 = jp.ShoulderRight.Depth,
                X2 = jp.ShoulderCenter.X,
                Y2 = jp.ShoulderCenter.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.ShoulderLeft.X,
                Y1 = jp.ShoulderLeft.Depth,
                X2 = jp.ElbowLeft.X,
                Y2 = jp.ElbowLeft.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.WristLeft.X,
                Y1 = jp.WristLeft.Depth,
                X2 = jp.ElbowLeft.X,
                Y2 = jp.ElbowLeft.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.WristLeft.X,
                Y1 = jp.WristLeft.Depth,
                X2 = jp.HandLeft.X,
                Y2 = jp.HandLeft.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.ShoulderRight.X,
                Y1 = jp.ShoulderRight.Depth,
                X2 = jp.ElbowRight.X,
                Y2 = jp.ElbowRight.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.WristRight.X,
                Y1 = jp.WristRight.Depth,
                X2 = jp.ElbowRight.X,
                Y2 = jp.ElbowRight.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.WristRight.X,
                Y1 = jp.WristRight.Depth,
                X2 = jp.HandRight.X,
                Y2 = jp.HandRight.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.Spine.X,
                Y1 = jp.Spine.Depth,
                X2 = jp.ShoulderCenter.X,
                Y2 = jp.ShoulderCenter.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.Spine.X,
                Y1 = jp.Spine.Depth,
                X2 = jp.HipCenter.X,
                Y2 = jp.HipCenter.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.HipLeft.X,
                Y1 = jp.HipLeft.Depth,
                X2 = jp.HipCenter.X,
                Y2 = jp.HipCenter.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.HipLeft.X,
                Y1 = jp.HipLeft.Depth,
                X2 = jp.KneeLeft.X,
                Y2 = jp.KneeLeft.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.AnkleLeft.X,
                Y1 = jp.AnkleLeft.Depth,
                X2 = jp.KneeLeft.X,
                Y2 = jp.KneeLeft.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.AnkleLeft.X,
                Y1 = jp.AnkleLeft.Depth,
                X2 = jp.FootLeft.X,
                Y2 = jp.FootLeft.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.HipRight.X,
                Y1 = jp.HipRight.Depth,
                X2 = jp.HipCenter.X,
                Y2 = jp.HipCenter.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.HipRight.X,
                Y1 = jp.HipRight.Depth,
                X2 = jp.KneeRight.X,
                Y2 = jp.KneeRight.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.AnkleRight.X,
                Y1 = jp.AnkleRight.Depth,
                X2 = jp.KneeRight.X,
                Y2 = jp.KneeRight.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
            skeletoncanvas.Children.Add(new Line()
            {
                X1 = jp.AnkleRight.X,
                Y1 = jp.AnkleRight.Depth,
                X2 = jp.FootRight.X,
                Y2 = jp.FootRight.Depth,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 2
            });
        }

        private void EllipseDraw(DepthImagePoint a)
        {
            skeletoncanvas.Children.Add(new Ellipse()
            {
                //HorizontalAlignment = ,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(a.X - 25, a.Y - 25, 0, 0),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2.5,
                Width = 50,
                Height = 50,
            });
        }

        private void EllipseDraw_Fill(DepthImagePoint a)
        {
            skeletoncanvas.Children.Add(new Ellipse()
            {
                //HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(a.X - 23.75, a.Y - 23.75, 0, 0),
                Fill = new SolidColorBrush(Colors.Red),
                Width = 47.5,
                Height = 47.5,
            });
        }

        private void CheckBox_All_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBox_All.IsChecked == true)
            {
                CheckBox_ElbowLeft.IsChecked = true;
                CheckBox_ElbowRight.IsChecked = true;
                CheckBox_KneeLeft.IsChecked = true;
                CheckBox_KneeRight.IsChecked = true;
                CheckBox_ShoulderLeft.IsChecked = true;
                CheckBox_ShoulderRight.IsChecked = true;
            }
        }

        private void CheckBox_All_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CheckBox_All.IsChecked == false)
            {
                CheckBox_ElbowLeft.IsChecked = false;
                CheckBox_ElbowRight.IsChecked = false;
                CheckBox_KneeLeft.IsChecked = false;
                CheckBox_KneeRight.IsChecked = false;
                CheckBox_ShoulderLeft.IsChecked = false;
                CheckBox_ShoulderRight.IsChecked = false;
            }
        }

    }
}
