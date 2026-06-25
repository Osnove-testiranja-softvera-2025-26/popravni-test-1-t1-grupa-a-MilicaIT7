using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS2026_PT1_GrupaA.Test
{
    internal class GameTestCaseData
    {
        public static IEnumerable CalculateIncome_TestData
        {
            get
            {
                yield return new TestCaseData(
                    13, 0, false,
                    Game.Score.Good);

                yield return new TestCaseData(
                    7, 10, true,
                    Game.Score.Good);

                yield return new TestCaseData(
                    6, 10, true,
                    Game.Score.Average);

                yield return new TestCaseData(
                    6, 9, true,
                    Game.Score.Bad);

                yield return new TestCaseData(
                    6, 10, false,
                    Game.Score.Bad);

                yield return new TestCaseData(
                    12, 0, false,
                    Game.Score.Bad);
            }
        }
    }
}
