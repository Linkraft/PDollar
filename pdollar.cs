using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDollarGestureRecognizer;

namespace pdollar {
    class pdollarApp {
        public static void PrintHelp() {
            Console.WriteLine("\n  -=- -=- -=- -=- -=- -=- -=- -=-   Help   -=- -=- -=- -=- -=- -=- -=- -=-  \n");
            Console.WriteLine("     Here are the commands and descriptions detailing their usage:\n");
            Console.WriteLine(" 1.) Add a gesture file to the list of gesture templates:");
            Console.WriteLine("     pdollar -t <gesturefile>\n");
            Console.WriteLine(" 2.) Prints the name of gestures when recognized from the event stream:");
            Console.WriteLine("     pdollar <eventstream>\n");
            Console.WriteLine(" 3.) Clears all stored templates:");
            Console.WriteLine("     pdollar -r\n");
        }

        public static Gesture ReadGestureFromFile(string gestureFile, bool store) {
            try {
                StreamReader reader = File.OpenText(gestureFile);
                List<Point> pointList = new List<Point>();

                int strokeID = 0;
                string gestureName, line;
                gestureName = reader.ReadLine();

                while ((line = reader.ReadLine()) != null) {
                    if (line == "BEGIN") continue;
                    else if (line == "END") strokeID++;
                    else {
                        int[] coords = line.Split(',').Select(int.Parse).ToArray();
                        pointList.Add(new Point(coords[0], coords[1], strokeID));
                    }
                }
                reader.Close();

                if (store) {
                    reader = File.OpenText(gestureFile);
                    StreamWriter writer = new StreamWriter("./templates/" + gestureName + ".txt");
                    while ((line = reader.ReadLine()) != null) writer.WriteLine(line);
                    writer.Close();
                }

                return new Gesture(pointList.ToArray(), gestureName); ;
            }
            catch (FileNotFoundException) {
                Console.WriteLine("File " + gestureFile + " not found!");
            }
            return null;
        }

        public static void RecognizeEvents(string eventStream) {
            List<Gesture> templates = new List<Gesture>();
            string[] files = Directory.GetFiles("./templates");
            foreach (string file in files) templates.Add(ReadGestureFromFile(file, false));
            try { 
                StreamReader reader = File.OpenText(eventStream);
                List<Point> pointList = new List<Point>();
                int strokeID = 0;
                string line;

                while ((line = reader.ReadLine()) != null) {
                    if (line == "MOUSEDOWN") continue;
                    else if (line == "MOUSEUP") strokeID++;
                    else if (line == "RECOGNIZE") {
                        Gesture gesture = new Gesture(pointList.ToArray());
                        string recognized = PointCloudRecognizer.Classify(gesture, templates.ToArray());
                        if (recognized != "") Console.WriteLine(recognized);
                        strokeID = 0;
                        pointList.Clear();
                    }
                    else {
                        int[] coords = line.Split(',').Select(int.Parse).ToArray();
                        pointList.Add(new Point(coords[0], coords[1], strokeID));
                    }
                }
            }
            catch (FileNotFoundException) {
                Console.WriteLine("File " + eventStream + " not found!");
            }
        }

        public static void ClearTemplates() {
            string[] files = Directory.GetFiles("./templates");
            foreach (string file in files) File.Delete(file);
            if (Directory.Exists("./templates")) Directory.Delete("./templates");
        }

        public static void Main(string[] args) {
            if (!Directory.Exists("./templates"))
                Directory.CreateDirectory("./templates");
            if (args.Length <= 1) PrintHelp();
            else if (args[0] == "pdollar") {
                if (args[1] == "-t" && args.Length == 3 && args[2].EndsWith(".txt"))
                    ReadGestureFromFile(args[2], true);
                else if (args.Length == 2 && args[1].EndsWith("eventfile.txt"))
                    RecognizeEvents(args[1]);
                else if (args.Length == 2 && args[1] == "-r")
                    ClearTemplates();
                else PrintHelp();
            }
            else {
                Console.WriteLine(args[0]);
                PrintHelp();
            }
        }
    }
}