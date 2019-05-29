// Copyright © Tigra Astronomy, all rights reserved.
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Specifications.Builders
    {
    class RotatorStatusBuilder
        {
        IRotatorStatus status = A.Fake<IRotatorStatus>();
        public IRotatorStatus Build() => status;
        public RotatorStatusBuilder Azimuth(int azimuth)
            {
            A.CallTo(() => status.Azimuth).Returns(azimuth);
            return this;
            }

        public RotatorStatusBuilder Circumference(int steps)
            {
            A.CallTo(() => status.DomeCircumference).Returns(steps);
            return this;
            }

        public RotatorStatusBuilder AtHome(int expectedAzimuth)
            {
            A.CallTo(() => status.AtHome).Returns(true);
            A.CallTo(() => status.HomePosition).Returns(expectedAzimuth);
            return this.Azimuth(expectedAzimuth);
            }
        }
    }