using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MeetingsLibrary;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Program pg = new Program();
            pg.Run(@"../../../../MeetingsLibrary/");
            pg.ReadPo(@"../../../../MeetingsLibrary/");
        }

        private void Run(string path)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.Schemas.Add("http://www.example.org/meetings",
                path + "Meetings.xsd");
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationEventHandler += ValidationHandler;

            XmlReader reader = XmlReader
                .Create(path + "pasture-example.xml",
                settings);
            while (reader.Read())
            {
            }
            reader.Close();
        }
        private static void ValidationHandler(Object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                Console.WriteLine("Warning: {0}", args.Message);
            }
            else 
            {
                Console.WriteLine("Error: {0}", args.Message);
            }
        }

        private void ReadPo(string filename)           
        {
            filename = filename + "pasture-example.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(pastures));

            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            FileStream fs = new FileStream(filename, FileMode.Open);
            pastures pa;

            pa = (MeetingsLibrary.pastures)serializer.Deserialize(fs);

            meeting[] meetings = pa.meetings;
            foreach (meeting meeting in meetings)
            {
                Console.Write($"{meeting} in pasture of area {meeting.location} participants:");
                foreach (string participant in meeting.participant)
                {
                    Console.Write($"{participant}, ");
                }
                Console.WriteLine();
            }
        }
        private void serializer_UnknownNode
        (object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }
        private void serializer_UnknownAttribute
       (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}
