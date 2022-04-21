using eIght.Arm.Entities;
using eIght.Arm.Alghoritms;
using System;
using System.Linq;
using System.Numerics;
using netDxf;
using netDxf.Entities;
using System.Diagnostics;

namespace ConsoleTEST
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AbstractArmNode node1 = new ArmNodeZ("NodeZ 1");
            AbstractArmNode node2 = new ArmNodeX("NodeX 2");
            AbstractArmNode node3 = new ArmNodeX("NodeX 3");
            AbstractArmNode node4 = new ArmNodeY("NodeY 4");
            AbstractArmNode node5 = new ArmNodeX("NodeX 5");

            //node1.Rotation.MinAngle = 90.0f;
            //node1.Rotation.MaxAngle = 180.0f;

            //node2.Rotation.MinAngle = 15.0f;
            //node2.Rotation.MaxAngle = 90.0f;

            //node4.Rotation.MinAngle = 150.0f;
            //node4.Rotation.MaxAngle = 360.0f;

            Arm arm = new Arm();
            arm.AddNode(0, node1);
            arm.AddNode(2, node2);
            arm.AddNode(1, node3);
            arm.AddNode(4, node4);
            arm.AddNode(3, node5);

            float ang = 90;

            node1.BaseTranslation = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
            node2.BaseTranslation = new System.Numerics.Vector3(0.0f, 0.0f, 250.0f);
            node3.BaseTranslation = new System.Numerics.Vector3(0.0f, 0.0f, 250.0f);
            node4.BaseTranslation = new System.Numerics.Vector3(0.0f, 0.0f, 250.0f);
            node5.BaseTranslation = new System.Numerics.Vector3(0.0f, 0.0f, 150.0f);

            //node1.BaseRotation = new Vector3(0.0f, 0.0f, ang);
            //node3.BaseRotation = new Vector3(ang, 0.0f, 0.0f);
            //node4.BaseRotation = new Vector3(0.0f, ang, 0.0f);

            Console.WriteLine($"=========== Ordered arm nodes by inverse kinematic priority");
            foreach (var item in arm.GetArmNodesIKPriorityOrdered())
            {
                Console.WriteLine($"{item.node.Name} priority {item.inverseKinematicPriority}");

            }

            Console.WriteLine();
            Console.WriteLine($"=========== Before calculations");
            Console.WriteLine($"Number of nodes: {arm.Count()}");
            foreach (IArmNode armNode in arm)
            {
                System.Numerics.Vector3 vector = armNode.Transformation.Translation;
                Console.WriteLine($"{armNode.Name}: {vector.X:0.00}, {vector.Y:0.00}, {vector.Z:0.00}");

            }

            IAlghoritmIK CCD = new CCD(1f);

            Console.WriteLine();
            Console.WriteLine($"=========== Calculated");

            //Vector3<double> vector3;

            System.Numerics.Vector3 point = new System.Numerics.Vector3(100.0f, 100.0f, 100.0f);
            CCD.Calculate(arm, point);
            Console.WriteLine($"Destination point: ({point}), reached: ({arm.LastNode.Transformation.Translation})");

            foreach (IArmNode armNode in arm)
            {
                System.Numerics.Vector3 vector = armNode.Transformation.Translation;
                float angle = armNode.Rotation.Angle;

                Console.WriteLine($"{armNode.Name}: {vector.X:0.00}, {vector.Y:0.00}, {vector.Z:0.00}, angle: {angle}");

            }

            Random random = new Random();
            DxfDocument dxf = new DxfDocument();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int n = 10;

            for (int i = 0; i < n; i++)
            {
                //point = new System.Numerics.Vector3(random.Next(-800,800), random.Next(-400, 400), random.Next(0, 400));
                point = new System.Numerics.Vector3((float)(Math.Sin(i) * 500.0f), (float)(Math.Cos(i) * 500.0f), random.Next(-250, 650));
                CCD.Calculate(arm, point);

                Point destPoint = new Point(point.X, point.Y, point.Z);
                destPoint.Color = AciColor.Red;

                dxf.AddEntity(destPoint);

                foreach (IArmNode armNode in arm)
                {
                    System.Numerics.Vector3 transl = armNode.Transformation.Translation;
                    netDxf.Vector3 startPoint = new netDxf.Vector3(transl.X, transl.Y, transl.Z);

                    Point nodePoint = new Point(startPoint);
                    nodePoint.Color = AciColor.Yellow;

                    dxf.AddEntity(nodePoint);

                    if (armNode.ChildNode != null)
                    {
                        System.Numerics.Vector3 translChild = armNode.ChildNode.Transformation.Translation;
                        netDxf.Vector3 endPoint = new netDxf.Vector3(translChild.X, translChild.Y, translChild.Z);

                        Line line = new Line(startPoint, endPoint);
                        dxf.AddEntity(line);

                    }

                }
            }

            stopWatch.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
           ts.Hours, ts.Minutes, ts.Seconds,
           ts.Milliseconds / 10);

            Console.WriteLine($"Elapsed time for {n} points calculation: " + elapsedTime);

            dxf.Save("D:\\arm.dxf");

            Console.ReadKey();
        }
    }
}
