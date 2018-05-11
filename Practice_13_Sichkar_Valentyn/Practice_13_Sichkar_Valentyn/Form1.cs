// File: Form1.cs
// Description: System programming in C# for SPARQL querying through KB with interface development by creating html files
// Environment: Visual Studio 2015 and C#
//
// MIT License
// Copyright (c) 2017 Valentyn N Sichkar
// github.com/sichkar-valentyn
//
// Reference to:
// [1] Valentyn N Sichkar. System programming in C# for SPARQL querying through KB with interface development by creating html files // GitHub platform [Electronic resource]. URL: https://github.com/sichkar-valentyn/System_programming_for_SPARQL_querying_with_interface_development_by_html_files (date of access: XX.XX.XXXX)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace Practice_13_Sichkar_Valentyn
{
    public partial class Form1 : Form
    {
        string standartWay = "KB/";
        string[] derictories = { "classes/", "properties/", "instances/" };

        private const string Query1 = @"
prefix carp: <http://car-assistant.ru/properties#>
prefix cari: <http://car-assistant.ru/instances#>
prefix carc: <http://car-assistant.ru/classes#>
SELECT ?name
WHERE {
?name a carc:AccidentFactors.
}";

        private const string Query2 = @"
prefix carp: <http://car-assistant.ru/properties#>
prefix cari: <http://car-assistant.ru/instances#>
prefix carc: <http://car-assistant.ru/classes#>
SELECT ?name
WHERE {
cari:BadWeather carp:ConsistOf ?name.
}";

        private const string Query3 = @"
prefix carp: <http://car-assistant.ru/properties#>
prefix cari: <http://car-assistant.ru/instances#>
prefix carc: <http://car-assistant.ru/classes#>
SELECT ?name
WHERE {
?name a carc:DangerousSituations.
}";

        void sparqgSend(string Factors, string description)
        {
            var parser = new Notation3Parser();
            var graph = new Graph();

            //Console.WriteLine("Loading Notation-3 file.");
            richTextBox1.Text += "Loading Notation-3 file:" + Environment.NewLine;
            richTextBox1.Text += "KB.n3" + Environment.NewLine;
            richTextBox1.Text += Environment.NewLine;
            richTextBox1.Text += Environment.NewLine;
            parser.Load(graph, @"KB\KB.n3");

            //Console.WriteLine("Nodes:");
            richTextBox1.Text += "Nodes:" + Environment.NewLine;
            foreach (Triple triple in graph.Triples)
            {
                richTextBox1.Text += GetNodeString(triple.Subject) + GetNodeString(triple.Predicate) + GetNodeString(triple.Object) + Environment.NewLine;
                //Console.WriteLine("{0} {1} {2}", GetNodeString(triple.Subject), GetNodeString(triple.Predicate), GetNodeString(triple.Object));
            }

            //Console.WriteLine();
            richTextBox1.Text += Environment.NewLine;
            //Console.WriteLine();
            richTextBox1.Text += Environment.NewLine;

            SparqlResultSet resultSet = graph.ExecuteQuery(Factors) as SparqlResultSet;
            if (resultSet != null)
            {
                //Console.WriteLine("Querying results for variable 'name':");
                richTextBox1.Text += description + Environment.NewLine;
                for (int i = 0; i < resultSet.Count; i++)
                {
                    SparqlResult result = resultSet[i];
                    //Console.WriteLine("{0}. {1}", i + 1, result["name"]);
                    richTextBox1.Text += Convert.ToString(i + 1) + ". " + Convert.ToString(result["name"]) + Environment.NewLine;
                }
            }
        }

        void UsersparqgSend(string Factors, string description)
        {
            var parser = new Notation3Parser();
            var graph = new Graph();

            //Console.WriteLine("Loading Notation-3 file.");
            richTextBox2.Text += "Loading Notation-3 file:" + Environment.NewLine;
            richTextBox2.Text += "KB.n3" + Environment.NewLine;
            parser.Load(graph, @"KB\KB.n3");

            //Console.WriteLine();
            richTextBox2.Text += Environment.NewLine;

            SparqlResultSet resultSet = graph.ExecuteQuery(Factors) as SparqlResultSet;
            if (resultSet != null)
            {
                //Console.WriteLine("Querying results for variable 'name':");
                richTextBox2.Text += description + Environment.NewLine;
                for (int i = 0; i < resultSet.Count; i++)
                {
                    SparqlResult result = resultSet[i];
                    //Console.WriteLine("{0}. {1}", i + 1, result["name"]);
                    richTextBox2.Text += Convert.ToString(i + 1) + ". " + Convert.ToString(result["name"]) + Environment.NewLine;
                }
            }
        }

        void htmlFilesGeneration(string Way, string FileName)
        {
            //Reading all information from *.n3 file and putting it into the infostring
            FileStream reading_n3_file = new FileStream(Way + FileName + ".n3", FileMode.Open, FileAccess.Read);
            StreamReader reader_n3 = new StreamReader(reading_n3_file, Encoding.UTF8);
            string n3_string = reader_n3.ReadToEnd();
            reader_n3.Close();

            //Reading all information from *info.html file and putting it into the htmlstring
            FileStream reading_html_file = new FileStream(Way + FileName + "info" + ".html", FileMode.Open, FileAccess.Read);
            StreamReader reader_html = new StreamReader(reading_html_file, Encoding.UTF8);
            string html_string = reader_html.ReadToEnd();
            reader_html.Close();

            //handling string, preparing it for writing into the *.html file
            string sub_n3_string = n3_string.Substring(255); //deleting an unnecessary information
            string[] array_n3 = sub_n3_string.Split(';'); //splitting into few strings by semicolons
            int n = Convert.ToInt32(array_n3[0].IndexOf(" "));
            array_n3[0] = array_n3[0].Substring(n+1); //deleting unnecessary first letters before rdf:type
            for (int i = 1; i < array_n3.Length; i++) array_n3[i] = array_n3[i].Substring(1); //deleting unnecessary first gaps
            string html_result = "<table border = '1'>" + Environment.NewLine;

            for (int i = 0; i < array_n3.Length; i++)
            {
                html_result += "<tr>" + Environment.NewLine;
                string[] sub_array_n3 = array_n3[i].Split(' '); //splitting into few strings by gap

                for (int j = 0; j < sub_array_n3.Length; j++)
                {
                    if (sub_array_n3[j].IndexOf(",") > 0) sub_array_n3[j] = sub_array_n3[j].Replace(",", "");
                    if (sub_array_n3[j].IndexOf(".") > 0) sub_array_n3[j] = sub_array_n3[j].Replace(".", "");

                    string[] sub_sub_array_n3 = sub_array_n3[j].Split(':'); //splitting into few strings by colon
                                        
                    if (sub_sub_array_n3[0] == "rdf") sub_array_n3[j] = "<a href = 'http://www.w3.org/1999/02/22-rdf-syntax-ns#" + sub_sub_array_n3[1] + "'>" + sub_array_n3[j] + "</a>";
                    if (sub_sub_array_n3[0] == "rdfs") sub_array_n3[j] = "<a href = 'http://www.w3.org/2000/01/rdf-schema#" + sub_sub_array_n3[1] + "'>" + sub_array_n3[j] + "</a>";
                    if (sub_sub_array_n3[0] == "owl") sub_array_n3[j] = "<a href = 'http://www.w3.org/2002/07/owl#" + sub_sub_array_n3[1] + "'>" + sub_array_n3[j] + "</a>";
                    if (sub_sub_array_n3[0] == "carc") sub_array_n3[j] = "<a href = '" + Application.StartupPath + "/KB/classes/" + sub_sub_array_n3[1] + ".html" + "'>" + sub_array_n3[j] + "</a>";
                    if (sub_sub_array_n3[0] == "carp") sub_array_n3[j] = "<a href = '" + Application.StartupPath + "/KB/properties/" + sub_sub_array_n3[1] + ".html" + "'>" + sub_array_n3[j] + "</a>";
                    if (sub_sub_array_n3[0] == "cari") sub_array_n3[j] = "<a href = '" + Application.StartupPath + "/KB/instances/" + sub_sub_array_n3[1] + ".html" + "'>" + sub_array_n3[j] + "</a>";

                    if (j == 0) html_result += "<td>" + sub_array_n3[j] + "</td><td>";
                    if (j == 1) html_result += sub_array_n3[j];
                    if (j > 1) html_result += "<br>" + sub_array_n3[j];

                }

                html_result += "</td></tr>" + Environment.NewLine;
            }

            html_result += "</table>";

            //creating the *.html file and writing inormation from the string into it
            FileStream creatingAfile = new FileStream(Way + FileName + ".html", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(creatingAfile, Encoding.UTF8);
            writer.Write(html_string + html_result);
            writer.Close();
        }

        static string GetNodeString(INode node)
        {
            string s = node.ToString();
            switch (node.NodeType)
            {
                case NodeType.Uri:
                    int lio = s.LastIndexOf('#');
                    if (lio == -1)
                        return s;
                    else
                        return s.Substring(lio + 1);
                case NodeType.Literal:
                    return string.Format("\"{0}\"", s);
                default:
                    return s;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sparqgSend(Query1, "Accident factors are:");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sparqgSend(Query2, "Bad weather might consist of:");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sparqgSend(Query3, "Dangerous situations are:");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int sum_of_files = 0;
            int nc = 0, np = 0, ni = 0;

            for (int i = 0; i < derictories.Length; i++)
            {
                DirectoryInfo info = new DirectoryInfo(standartWay + derictories[i]);

                FileInfo[] files = info.GetFiles("*.n3");

                foreach (FileInfo file in files)
                {
                    string[] filename = file.Name.Split('.');
                    htmlFilesGeneration(standartWay + derictories[i], filename[0]);
                    if (i == 0) nc++;
                    if (i == 1) np++;
                    if (i == 2) ni++;
                }
            }

            sum_of_files = nc + np + ni;

            label6.Text = sum_of_files.ToString();
            label8.Text = nc.ToString();
            label9.Text = np.ToString();
            label11.Text = ni.ToString();
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string index_html_result = "<h1>Knowledge base of CarAssistant</h1>" + Environment.NewLine + "<table border = '1'>" + Environment.NewLine;
            index_html_result += "<h2><a href = '" + Application.StartupPath + "/KB/" + "KB.n3" + "'>Download current version in form of *.n3 - file</a></h2>" + Environment.NewLine;

                   string n3_result = @"@prefix carc: <http://car-assistant.ru/classes#>.
@prefix carp: <http://car-assistant.ru/properties#>.
@prefix cari: <http://car-assistant.ru/instances#>.
@prefix owl: <http://www.w3.org/2002/07/owl#>.
@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>.
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#>." + Environment.NewLine + Environment.NewLine;

            for (int i = 0; i < derictories.Length; i++)
            {
                DirectoryInfo info = new DirectoryInfo(standartWay + derictories[i]);

                FileInfo[] files = info.GetFiles("*.n3");

                if (i == 0) index_html_result += "<tr><td>Classes</td><td><ul>";
                if (i == 1) index_html_result += "<tr><td>Properties</td><td><ul>";
                if (i == 2) index_html_result += "<tr><td>Instances</td><td><ul>";

                foreach (FileInfo file in files)
                {
                    string[] filename = file.Name.Split('.');

                    if (i == 0) index_html_result += "<li>" + "<a href = '" + Application.StartupPath + "/KB/classes/" + filename[0] + ".html" + "'>" + filename[0] + "</a>" + Environment.NewLine;
                    if (i == 1) index_html_result += "<li>" + "<a href = '" + Application.StartupPath + "/KB/properties/" + filename[0] + ".html" + "'>" + filename[0] + "</a>" + Environment.NewLine;
                    if (i == 2) index_html_result += "<li>" + "<a href = '" + Application.StartupPath + "/KB/instances/" + filename[0] + ".html" + "'>" + filename[0] + "</a>" + Environment.NewLine;

                    //Reading all information from *.n3 file and putting it into the infostring
                    FileStream reading_n3_file = new FileStream(Application.StartupPath + "/KB/" + derictories[i] + filename[0] + ".n3", FileMode.Open, FileAccess.Read);
                    StreamReader reader_n3 = new StreamReader(reading_n3_file, Encoding.UTF8);
                    string n3_string = reader_n3.ReadToEnd();
                    reader_n3.Close();

                    n3_result += n3_string.Substring(255) + Environment.NewLine + Environment.NewLine; //deleting an unnecessary information
                }

                if (i == 0) index_html_result += "</ul></td></tr>";
                if (i == 1) index_html_result += "</ul></td></tr>";
                if (i == 2) index_html_result += "</ul></td></tr>";
            }

            index_html_result += "</table>";

            //creating the index.html file and writing inormation from the string into it
            FileStream creatingAfile = new FileStream(Application.StartupPath + "/KB/" + "index.html", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(creatingAfile, Encoding.UTF8);
            writer.Write(index_html_result);
            writer.Close();

            label14.Text = "Index.html file was created!";

            //creating the index.html file and writing inormation from the string into it
            FileStream creatingNfile = new FileStream(Application.StartupPath + "/KB/" + "KB.n3", FileMode.Create, FileAccess.Write);
            StreamWriter writerN = new StreamWriter(creatingNfile, Encoding.UTF8);
            writerN.Write(n3_result);
            writerN.Close();

            label15.Text = "KB.n3 file was created!";

            //Definding the number of triples
            int ntriples = 0;
            var parser = new Notation3Parser();
            var graph = new Graph();
            parser.Load(graph, @"KB\KB.n3");
            foreach (Triple triple in graph.Triples)
            {
                ntriples++;
            }

            label16.Text = ntriples.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string query_text1 = comboBox1.Text;
            string query_text2 = comboBox2.Text;
            string query_text3 = comboBox3.Text;

            if (query_text1 == "?") query_text1 = "?name";
            if (query_text1 == "consist of") query_text1 = "carp:ConsistOf";
            if (query_text1 == "can cause") query_text1 = "carp:CanCause";
            if (query_text1 == "causes") query_text1 = "carp:Causes";
            if (query_text1 == "sensors") query_text1 = "carc:Sensors";
            if (query_text1 == "safety driving") query_text1 = "carc:SafetyDriving";
            if (query_text1 == "low visibility") query_text1 = "carc:LowVisibility";
            if (query_text1 == "low observability") query_text1 = "carc:LowObservability";
            if (query_text1 == "dangerous situations") query_text1 = "carc:DangerousSituations";
            if (query_text1 == "dangerous road") query_text1 = "carc:DangerousRoad";
            if (query_text1 == "accident factors") query_text1 = "carc:AccidentFactors";
            if (query_text1 == "3D Digital Map") query_text1 = "cari:3DDigitalMap";
            if (query_text1 == "Bad Health") query_text1 = "cari:BadHealth";
            if (query_text1 == "Bad Service") query_text1 = "cari:BadService";
            if (query_text1 == "Bad Weather") query_text1 = "cari:BadWeather";
            if (query_text1 == "Blinds Spots") query_text1 = "cari:BlindsSpots";
            if (query_text1 == "Bright Lights") query_text1 = "cari:BrightLights";
            if (query_text1 == "Bright Sun") query_text1 = "cari:BrightSun";
            if (query_text1 == "Broken Car") query_text1 = "cari:BrokenCar";
            if (query_text1 == "Cameras") query_text1 = "cari:Cameras";
            if (query_text1 == "Concentrating") query_text1 = "cari:Concentrating";
            if (query_text1 == "Crossing Bridge") query_text1 = "cari:CrossingBridge";
            if (query_text1 == "Crossing Pedestrian Zone") query_text1 = "cari:CrossingPedestrianZone";
            if (query_text1 == "Crossing Railway Zone") query_text1 = "cari:CrossingRailwayZone";
            if (query_text1 == "Crossing Tunnel") query_text1 = "cari:CrossingTunnel";
            if (query_text1 == "Dark Place") query_text1 = "cari:DarkPlace";
            if (query_text1 == "Distractions") query_text1 = "cari:Distractions";
            if (query_text1 == "Down Hill") query_text1 = "cari:DownHill";
            if (query_text1 == "Drank Driver") query_text1 = "cari:DrankDriver";
            if (query_text1 == "Driving Down") query_text1 = "cari:DrivingDown";
            if (query_text1 == "Driving In The Fog") query_text1 = "cari:DrivingInTheFog";
            if (query_text1 == "Driving License") query_text1 = "cari:DrivingLicense";
            if (query_text1 == "Driving Up") query_text1 = "cari:DrivingUp";
            if (query_text1 == "Dusk") query_text1 = "cari:Dusk";
            if (query_text1 == "Eating") query_text1 = "cari:Eating";
            if (query_text1 == "Experienced Driver") query_text1 = "cari:ExperiencedDriver";
            if (query_text1 == "Fasten Belts") query_text1 = "cari:FastenBelts";
            if (query_text1 == "Fast Trains") query_text1 = "cari:FastTrains";
            if (query_text1 == "Following Rules") query_text1 = "cari:FollowingRules";
            if (query_text1 == "FoodIn The Car") query_text1 = "cari:FoodInTheCar";
            if (query_text1 == "Good Health") query_text1 = "cari:GoodHealth";
            if (query_text1 == "Good Service") query_text1 = "cari:GoodService";
            if (query_text1 == "Good Weather") query_text1 = "cari:GoodWeather";
            if (query_text1 == "Grad") query_text1 = "cari:Grad";
            if (query_text1 == "Heavy Traffic") query_text1 = "cari:HeavyTraffic";
            if (query_text1 == "High Temperature") query_text1 = "cari:HighTemperature";
            if (query_text1 == "Hill") query_text1 = "cari:Hill";
            if (query_text1 == "Ice") query_text1 = "cari:Ice";
            if (query_text1 == "Laser Scanner") query_text1 = "cari:LaserScanner";
            if (query_text1 == "Low Reaction") query_text1 = "cari:LowReaction";
            if (query_text1 == "Low Temperature") query_text1 = "cari:LowTemperature";
            if (query_text1 == "Low Traffic") query_text1 = "cari:LowTraffic";
            if (query_text1 == "Many Cars") query_text1 = "cari:ManyCars";
            if (query_text1 == "Many People") query_text1 = "cari:ManyPeople";
            if (query_text1 == "Narrow Place") query_text1 = "cari:NarrowPlace";
            if (query_text1 == "Not Fasten Belts") query_text1 = "cari:NotFastenBelts";
            if (query_text1 == "Overtaking Bicycle") query_text1 = "cari:OvertakingBicycle";
            if (query_text1 == "Overtaking Car") query_text1 = "cari:OvertakingCar";
            if (query_text1 == "Passengers In The Car") query_text1 = "cari:PassengersInTheCar";
            if (query_text1 == "Phone Calling") query_text1 = "cari:PhoneCalling";
            if (query_text1 == "Radar Transmitters") query_text1 = "cari:RadarTransmitters";
            if (query_text1 == "Rain") query_text1 = "cari:Rain";
            if (query_text1 == "Road Quality") query_text1 = "cari:RoadQuality";
            if (query_text1 == "Sand") query_text1 = "cari:Sand";
            if (query_text1 == "Sleepy Driver") query_text1 = "cari:SleepyDriver";
            if (query_text1 == "Slippery Surface") query_text1 = "cari:SlipperySurface";
            if (query_text1 == "Speech Concentrating") query_text1 = "cari:SpeechConcentrating";
            if (query_text1 == "Speeding") query_text1 = "cari:Speeding";
            if (query_text1 == "Storm") query_text1 = "cari:Storm";
            if (query_text1 == "Taking Antibiotics") query_text1 = "cari:TakingAntibiotics";
            if (query_text1 == "Talking Passengers") query_text1 = "cari:TalkingPassengers";
            if (query_text1 == "Thunder") query_text1 = "cari:Thunder";
            if (query_text1 == "Traffic Lights") query_text1 = "cari:TrafficLights";
            if (query_text1 == "Ultrasonic Sensors") query_text1 = "cari:UltrasonicSensors";
            if (query_text1 == "Viscous Ground") query_text1 = "cari:ViscousGround";
            if (query_text1 == "Zero Alcohol") query_text1 = "cari:ZeroAlcohol";

            if (query_text2 == "?") query_text2 = "?name";
            if (query_text2 == "label") query_text2 = "rdfs:label";
            if (query_text2 == "this is") query_text2 = "a";
            if (query_text2 == "consist of") query_text2 = "carp:ConsistOf";
            if (query_text2 == "can cause") query_text2 = "carp:CanCause";
            if (query_text2 == "causes") query_text2 = "carp:Causes";

            if (query_text3 == "?") query_text3 = "?name";
            if (query_text3 == "sensors") query_text3 = "carc:Sensors";
            if (query_text3 == "safety driving") query_text3 = "carc:SafetyDriving";
            if (query_text3 == "low visibility") query_text3 = "carc:LowVisibility";
            if (query_text3 == "low observability") query_text3 = "carc:LowObservability";
            if (query_text3 == "dangerous situations") query_text3 = "carc:DangerousSituations";
            if (query_text3 == "dangerous road") query_text3 = "carc:DangerousRoad";
            if (query_text3 == "accident factors") query_text3 = "carc:AccidentFactors";
            if (query_text3 == "3D Digital Map") query_text3 = "cari:3DDigitalMap";
            if (query_text3 == "Bad Health") query_text3 = "cari:BadHealth";
            if (query_text3 == "Bad Service") query_text3 = "cari:BadService";
            if (query_text3 == "Bad Weather") query_text3 = "cari:BadWeather";
            if (query_text3 == "Blinds Spots") query_text3 = "cari:BlindsSpots";
            if (query_text3 == "Bright Lights") query_text3 = "cari:BrightLights";
            if (query_text3 == "Bright Sun") query_text3 = "cari:BrightSun";
            if (query_text3 == "Broken Car") query_text3 = "cari:BrokenCar";
            if (query_text3 == "Cameras") query_text3 = "cari:Cameras";
            if (query_text3 == "Concentrating") query_text3 = "cari:Concentrating";
            if (query_text3 == "Crossing Bridge") query_text3 = "cari:CrossingBridge";
            if (query_text3 == "Crossing Pedestrian Zone") query_text3 = "cari:CrossingPedestrianZone";
            if (query_text3 == "Crossing Railway Zone") query_text3 = "cari:CrossingRailwayZone";
            if (query_text3 == "Crossing Tunnel") query_text3 = "cari:CrossingTunnel";
            if (query_text3 == "Dark Place") query_text3 = "cari:DarkPlace";
            if (query_text3 == "Distractions") query_text3 = "cari:Distractions";
            if (query_text3 == "Down Hill") query_text3 = "cari:DownHill";
            if (query_text3 == "Drank Driver") query_text3 = "cari:DrankDriver";
            if (query_text3 == "Driving Down") query_text3 = "cari:DrivingDown";
            if (query_text3 == "Driving In The Fog") query_text3 = "cari:DrivingInTheFog";
            if (query_text3 == "Driving License") query_text3 = "cari:DrivingLicense";
            if (query_text3 == "Driving Up") query_text3 = "cari:DrivingUp";
            if (query_text3 == "Dusk") query_text3 = "cari:Dusk";
            if (query_text3 == "Eating") query_text3 = "cari:Eating";
            if (query_text3 == "Experienced Driver") query_text3 = "cari:ExperiencedDriver";
            if (query_text3 == "Fasten Belts") query_text3 = "cari:FastenBelts";
            if (query_text3 == "Fast Trains") query_text3 = "cari:FastTrains";
            if (query_text3 == "Following Rules") query_text3 = "cari:FollowingRules";
            if (query_text3 == "FoodIn The Car") query_text3 = "cari:FoodInTheCar";
            if (query_text3 == "Good Health") query_text3 = "cari:GoodHealth";
            if (query_text3 == "Good Service") query_text3 = "cari:GoodService";
            if (query_text3 == "Good Weather") query_text3 = "cari:GoodWeather";
            if (query_text3 == "Grad") query_text3 = "cari:Grad";
            if (query_text3 == "Heavy Traffic") query_text3 = "cari:HeavyTraffic";
            if (query_text3 == "High Temperature") query_text3 = "cari:HighTemperature";
            if (query_text3 == "Hill") query_text3 = "cari:Hill";
            if (query_text3 == "Ice") query_text3 = "cari:Ice";
            if (query_text3 == "Laser Scanner") query_text3 = "cari:LaserScanner";
            if (query_text3 == "Low Reaction") query_text3 = "cari:LowReaction";
            if (query_text3 == "Low Temperature") query_text3 = "cari:LowTemperature";
            if (query_text3 == "Low Traffic") query_text3 = "cari:LowTraffic";
            if (query_text3 == "Many Cars") query_text3 = "cari:ManyCars";
            if (query_text3 == "Many People") query_text3 = "cari:ManyPeople";
            if (query_text3 == "Narrow Place") query_text3 = "cari:NarrowPlace";
            if (query_text3 == "Not Fasten Belts") query_text3 = "cari:NotFastenBelts";
            if (query_text3 == "Overtaking Bicycle") query_text3 = "cari:OvertakingBicycle";
            if (query_text3 == "Overtaking Car") query_text3 = "cari:OvertakingCar";
            if (query_text3 == "Passengers In The Car") query_text3 = "cari:PassengersInTheCar";
            if (query_text3 == "Phone Calling") query_text3 = "cari:PhoneCalling";
            if (query_text3 == "Radar Transmitters") query_text3 = "cari:RadarTransmitters";
            if (query_text3 == "Rain") query_text3 = "cari:Rain";
            if (query_text3 == "Road Quality") query_text3 = "cari:RoadQuality";
            if (query_text3 == "Sand") query_text3 = "cari:Sand";
            if (query_text3 == "Sleepy Driver") query_text3 = "cari:SleepyDriver";
            if (query_text3 == "Slippery Surface") query_text3 = "cari:SlipperySurface";
            if (query_text3 == "Speech Concentrating") query_text3 = "cari:SpeechConcentrating";
            if (query_text3 == "Speeding") query_text3 = "cari:Speeding";
            if (query_text3 == "Storm") query_text3 = "cari:Storm";
            if (query_text3 == "Taking Antibiotics") query_text3 = "cari:TakingAntibiotics";
            if (query_text3 == "Talking Passengers") query_text3 = "cari:TalkingPassengers";
            if (query_text3 == "Thunder") query_text3 = "cari:Thunder";
            if (query_text3 == "Traffic Lights") query_text3 = "cari:TrafficLights";
            if (query_text3 == "Ultrasonic Sensors") query_text3 = "cari:UltrasonicSensors";
            if (query_text3 == "Viscous Ground") query_text3 = "cari:ViscousGround";
            if (query_text3 == "Zero Alcohol") query_text3 = "cari:ZeroAlcohol";
        


            string User_Query = @"
prefix carp: <http://car-assistant.ru/properties#>
prefix cari: <http://car-assistant.ru/instances#>
prefix carc: <http://car-assistant.ru/classes#>
prefix owl: <http://www.w3.org/2002/07/owl#>
prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#>
SELECT ?name
WHERE { " + query_text1 + " " + query_text2 + " " + query_text3 + ". }";

            UsersparqgSend(User_Query, "Result of your request:");

        }
    }
}
