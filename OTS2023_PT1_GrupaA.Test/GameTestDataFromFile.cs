using NUnit.Framework;
using OTS2026_PT1_GrupaA.Models;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS2026_PT1_GrupaA.Test
{
    internal class GameTestDataFromFile
    {
        public static IEnumerable Get_ValidatePosition_TestData(string filename)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            { 
                string[] parts = line.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string positionType = parts[0];
                bool hasBoat = parts[1] == "yes";
                bool expected = parts[2] == "yes";

                Position position = MapPosition(positionType);

                yield return new TestCaseData(position, hasBoat, expected);
            }
        }

        private static Position MapPosition(string type)
        {
            switch (type)
            {
                case "null": return null;

                case "outside": return new Position(-1, -1);

                case "invalid": return new Position(0, 0);

                case "land": return new Position(5, 10);

                case "pond": return new Position(25, 10);

                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
