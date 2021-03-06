﻿using System;
using System.IO;
using System.Xml;

namespace GpxSplit
{
    class Program
    {
        static void Main(string[] args)
        {
            int unkownFileCounter = 0;

            const string fileHeader = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n<gpx xmlns:gpxx=\"http://www.garmin.com/xmlschemas/GpxExtensions/v3\" xmlns:gpxtpx=\"http://www.garmin.com/xmlschemas/TrackPointExtension/v2\" xmlns=\"http://www.topografix.com/GPX/1/1\" xmlns:trp=\"http://www.garmin.com/xmlschemas/TripExtensions/v1\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:nmea=\"http://trekbuddy.net/2009/01/gpx/nmea\" version=\"1.1\" creator=\"Generated by Christian Pesch's RouteConverter. See http://www.routeconverter.com\">";
            const string fileFooter = "</gpx>";

            XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
            try
            {
                xmlDoc.Load(args[0]);
            }
            catch
            {
                Console.WriteLine("Invalid file or filename specified.");
                return;
            }

            // xmlDoc = RemoveXmlns(xmlDoc);
            XmlNodeList trackList = xmlDoc.GetElementsByTagName("trk");

            foreach (XmlNode element in trackList)
            {
                string fileName = "";
                try
                {
                    // Get the <name> element irrespective of the namespace.
                    fileName = element.SelectSingleNode("*[local-name() = 'name']").InnerText;
                }
                catch
                {
                    fileName = "Unknown_" + (unkownFileCounter++).ToString();
                }

                // Sanitize the <name> element before turning it into a file name.
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }
                Console.WriteLine("Writing to " + fileName);

                // Write the file to disk.
                using (StreamWriter outputFile = new StreamWriter(fileName + ".gpx"))
                {
                    outputFile.WriteLine(fileHeader);
                    outputFile.WriteLine(element.OuterXml);
                    outputFile.WriteLine(fileFooter);
                }
            }
        }
    }
}
