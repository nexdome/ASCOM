// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.Specifications.Builders
    {
    using FakeItEasy;

    class RotatorStatusBuilder
        {
        readonly IRotatorStatus status = A.Fake<IRotatorStatus>();

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
            return Azimuth(expectedAzimuth);
            }
        }

    class ShutterStatusBuilder
        {
        const int PresumedFullShutterTravel = 500;

        readonly IShutterStatus status = A.Fake<IShutterStatus>();

        public IShutterStatus Build() => status;

        public ShutterStatusBuilder Position(int steps)
            {
            A.CallTo(() => status.Position).Returns(steps);
            return this;
            }

        public ShutterStatusBuilder FullyOpen()
            {
            A.CallTo(() => status.OpenSensorActive).Returns(true);
            A.CallTo(() => status.ClosedSensorActive).Returns(false);
            A.CallTo(() => status.Position).Returns(PresumedFullShutterTravel);
            return this;
            }

        public ShutterStatusBuilder FullyClosed()
            {
            A.CallTo(() => status.OpenSensorActive).Returns(false);
            A.CallTo(() => status.ClosedSensorActive).Returns(true);
            A.CallTo(() => status.Position).Returns(0);
            return this;
            }

        public ShutterStatusBuilder PartiallyOpen()
            {
            A.CallTo(() => status.OpenSensorActive).Returns(false);
            A.CallTo(() => status.ClosedSensorActive).Returns(false);
            A.CallTo(() => status.Position).Returns(PresumedFullShutterTravel / 2);
            return this;
            }
        }
    }