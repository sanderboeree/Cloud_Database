using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlineStore.Api.Domain.Tests;
using OnlineStore.Investors.Api.Domain.Tests;

namespace OnlineStore.Api.Tests
{
    [TestClass]
    public class TestTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MissedBench should not be less than zero")]
        public void CreatingLSPTTestWhereMissedBenchIsNegative()
        {
            var LSPT = new LSPTTest
            {
                MissedBench = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "HitWrongTarget should not be less than zero")]
        public void CreatingLSPTTestWhereHitWrongTargetIsNegative()
        {
            var LSPT = new LSPTTest
            {
                HitWrongTarget = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TouchedCone should not be less than zero")]
        public void CreatingLSPTTestWhereTouchedConeIsNegative()
        {
            var LSPT = new LSPTTest
            {
                TouchedCone = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "PassOutsideArea should not be less than zero")]
        public void CreatingLSPTTestWherePassOutsideAreaIsNegative()
        {
            var LSPT = new LSPTTest
            {
                PassOutsideArea = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MissedTargetArea should not be less than zero")]
        public void CreatingLSPTTestWhereMissedTargetAreaIsNegative()
        {
            var LSPT = new LSPTTest
            {
                MissedTargetArea = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "HitTenCMStrip should not be less than zero")]
        public void CreatingLSPTTestWhereHitTenCMStripIsNegative()
        {
            var LSPT = new LSPTTest
            {
                HitTenCMStrip = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "FinalTime should not be less than zero")]
        public void CreatingLSPTTestWhereFinalTimeIsNegative()
        {
            var LSPT = new LSPTTest
            {
                FinalTime = TimeSpan.FromSeconds(-1)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "EndTime should not be less than zero")]
        public void CreatingLSPTTestWhereEndTimeIsNegative()
        {
            var LSPT = new LSPTTest
            {
                EndTime = TimeSpan.FromSeconds(-1)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "SecondsOver43 should not be less than zero")]
        public void CreatingLSPTTestWhereSecondsOver43IsNegative()
        {
            var LSPT = new LSPTTest
            {
                SecondsOver = TimeSpan.FromSeconds(-1)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TimeSlalom should not be less than zero")]
        public void CreatingFSTTestWhereTimeSlalomIsNegative()
        {
            var FST = new FSTTest
            {
                TimeSlalom = TimeSpan.FromSeconds(-1)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TimeBackward should not be less than zero")]
        public void CreatingFSTTestWhereTimeBackwardIsNegative()
        {
            var FST = new FSTTest
            {
                TimeBackward = TimeSpan.FromSeconds(-1)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TimeDribble should not be less than zero")]
        public void CreatingFSTTestWhereTimeDribbleIsNegative()
        {
            var FST = new FSTTest
            {
                TimeDribble = TimeSpan.FromSeconds(-1)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TimeEightFigure should not be less than zero")]
        public void CreatingFSTTestWhereTimeEightFigureIsNegative()
        {
            var FST = new FSTTest
            {
                TimeEightFigure = TimeSpan.FromSeconds(-1)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TimePassing should not be less than zero")]
        public void CreatingFSTTestWhereTimePassingIsNegative()
        {
            var FST = new FSTTest
            {
                TimePassing = TimeSpan.FromSeconds(-1)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "HoppingSideways1 should not be less than zero")]
        public void CreatingKTK3plusTestWhereHoppingSideways1IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                HoppingSideways1 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "HoppingSideWays2 should not be less than zero")]
        public void CreatingKTK3plusTestWhereHoppingSideWays2IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                HoppingSideWays2 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MovingSideways1 should not be less than zero")]
        public void CreatingKTK3plusTestWhereMovingSideways1IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                MovingSideways1 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MovingSideways2 should not be less than zero")]
        public void CreatingKTK3plusTestWhereMovingSideways2IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                MovingSideways2 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "HandEye1 should not be less than zero")]
        public void CreatingKTK3plusTestWhereHandEye1IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                HandEye1 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "HandEye2 should not be less than zero")]
        public void CreatingKTK3plusTestWhereHandEye2IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                HandEye2 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Beam11 should not be less than zero")]
        public void CreatingKTK3plusTestWhereBeam11IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                Beam11 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Beam12 should not be less than zero")]
        public void CreatingKTK3plusTestWhereBeam12IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                Beam12 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Beam13 should not be less than zero")]
        public void CreatingKTK3plusTestWhereBeam13IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                Beam13 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Beam21 should not be less than zero")]
        public void CreatingKTK3plusTestWhereBeam21IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                Beam21 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Beam22 should not be less than zero")]
        public void CreatingKTK3plusTestWhereBeam22IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                Beam22 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Beam23 should not be less than zero")]
        public void CreatingKTK3plusTestWhereBeam23IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                Beam23 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Beam31 should not be less than zero")]
        public void CreatingKTK3plusTestWhereBeam31IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                Beam31 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Beam32 should not be less than zero")]
        public void CreatingKTK3plusTestWhereBeam32IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                Beam32 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Beam33 should not be less than zero")]
        public void CreatingKTK3plusTestWhereBeam33IsNegative()
        {
            var KTK3plus = new KTK3plusTest
            {
                Beam33 = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "RawMEMScore should not be less than zero")]
        public void CreatingTVPS3TestWhereRawMEMScoreIsNegative()
        {
            var TVPS3 = new TVPS3Test
            {
                RawMEMScore = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "RawSPAScore should not be less than zero")]
        public void CreatingTVPS3TestWhereRawSPAScoreIsNegative()
        {
            var TVPS3 = new TVPS3Test
            {
                RawSPAScore = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "RawSEQScore should not be less than zero")]
        public void CreatingTVPS3TestWhereRawSEQScoreIsNegative()
        {
            var TVPS3 = new TVPS3Test
            {
                RawSEQScore = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "ChronologigalAge should not be empty")]
        public void CreatingTVPS3TestWhereChronologigalAgeIsMinValue()
        {
            var TVPS3 = new TVPS3Test
            {
                ChronologigalAge = DateTime.MinValue
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "EndTime should not be less than zero")]
        public void CreatingVFMTTestWhereTimePassingIsNegative()
        {
            var VFMT = new VFMTTest
            {
                EndTime = TimeSpan.FromSeconds(-1)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Mistakes should not be less than zero")]
        public void CreatingVFMTTestWhereMistakesIsNegative()
        {
            var VFMT = new VFMTTest
            {
                Mistakes = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Count should not be less than zero")]
        public void CreatingInsaitJoyTestWhereCountIsNegative()
        {
            var InsaitJoy = new InsaitJoyTest
            {
                Count = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Frequency should not be less than zero")]
        public void CreatingInsaitJoyTestWhereFrequencyIsNegative()
        {
            var InsaitJoy = new InsaitJoyTest
            {
                Frequency = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Fluency should not be less than zero")]
        public void CreatingInsaitJoyTestWhereFluencyIsNegative()
        {
            var InsaitJoy = new InsaitJoyTest
            {
                Fluency = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TestID should not be empty")]
        public void CreatingInsaitJoyTestWhereTestIDIsEmpty()
        {
            var InsaitJoy = new InsaitJoyTest
            {
                TestId = Guid.Empty
            };
        }
    }
}
