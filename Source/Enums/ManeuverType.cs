namespace BingMapsRESTToolkit
{
    /// <summary>
    /// Maneuver type values that are returned by the Routes API.
    /// </summary>
    public enum ManeuverType
    {
        ///<summary>
        ///Arrive at the final destination.
        ///</summary>
        ArriveFinish,

        ///<summary>
        ///Arrive at an intermediate waypoint.
        ///</summary>
        ArriveIntermediate,

        ///<summary>
        ///Bear left.
        ///</summary>
        BearLeft,

        ///<summary>
        ///Bear left and then bear left again.
        ///</summary>
        BearLeftThenBearLeft,

        ///<summary>
        ///Bear left and then bear right.
        ///</summary>
        BearLeftThenBearRight,

        ///<summary>
        ///Bear left and then turn left.
        ///</summary>
        BearLeftThenTurnLeft,

        ///<summary>
        ///Bear left and then turn right.
        ///</summary>
        BearLeftThenTurnRight,

        ///<summary>
        ///Bear right.
        ///</summary>
        BearRight,

        ///<summary>
        ///Bear right and then bear left.
        ///</summary>
        BearRightThenBearLeft,

        ///<summary>
        ///Bear right and then bear right again.
        ///</summary>
        BearRightThenBearRight,

        ///<summary>
        ///Bear right and then turn left.
        ///</summary>
        BearRightThenTurnLeft,

        ///<summary>
        ///Bear right and then turn right.
        ///</summary>
        BearRightThenTurnRight,

        ///<summary>
        ///Bear instruction and then a keep instruction
        ///</summary>
        BearThenKeep,

        ///<summary>
        ///Bear instruction and then a merge instruction.
        ///</summary>
        BearThenMerge,

        ///<summary>
        ///Continue on the current road.
        ///</summary>
        Continue,

        ///<summary>
        ///Leave an intermediate waypoint in a different direction and road than you arrived on.
        ///</summary>
        DepartIntermediateStop,

        ///<summary>
        ///Leave an intermediate waypoint in the same direction and on the same road that you arrived on.
        ///</summary>
        DepartIntermediateStopReturning,

        ///<summary>
        ///Leave the starting point.
        ///</summary>
        DepartStart,

        ///<summary>
        ///Enter a roundabout.
        ///</summary>
        EnterRoundabout,

        ///<summary>
        ///Exit a roundabout.
        ///</summary>
        ExitRoundabout,

        ///<summary>
        ///Enter and exit a roundabout.
        ///</summary>
        EnterThenExitRoundabout,

        ///<summary>
        ///Keep left onto a different road.
        ///</summary>
        KeepLeft,

        ///<summary>
        ///Keep left and continue onto ramp.
        ///</summary>
        KeepOnRampLeft,

        ///<summary>
        ///Keep right and continue onto ramp.
        ///</summary>
        KeepOnRampRight,

        ///<summary>
        ///Keep straight and continue onto ramp.
        ///</summary>
        KeepOnRampStraight,

        ///<summary>
        ///Keep right onto a different road.
        ///</summary>
        KeepRight,

        ///<summary>
        ///Keep straight onto a different road.
        ///</summary>
        KeepStraight,

        ///<summary>
        ///Keep left to stay on the same road.
        ///</summary>
        KeepToStayLeft,

        ///<summary>
        ///Keep right to stay on the same road.
        ///</summary>
        KeepToStayRight,

        ///<summary>
        ///Keep straight to stay on the same road.
        ///</summary>
        KeepToStayStraight,

        ///<summary>
        ///Merge onto a highway.
        ///</summary>
        Merge,

        ///<summary>
        ///No instruction.
        ///</summary>
        None,

        ///<summary>
        ///Take left ramp onto highway. This is part of a combined instruction.
        ///</summary>
        RampThenHighwayLeft,

        ///<summary>
        ///Take right ramp onto highway. This is part of a combined instruction.
        ///</summary>
        RampThenHighwayRight,

        ///<summary>
        ///Stay straight to take ramp onto highway. This is part of a combined instruction.
        ///</summary>
        RampThenHighwayStraight,

        ///<summary>
        ///Road name changes.
        ///</summary>
        RoadNameChange,

        ///<summary>
        ///Take the road. This instruction is used when you are entering or exiting a ferry.
        ///</summary>
        Take,

        ///<summary>
        ///Take ramp to the left.
        ///</summary>
        TakeRampLeft,

        ///<summary>
        ///Take ramp to the right.
        ///</summary>
        TakeRampRight,

        ///<summary>
        ///Stay straight to take ramp.
        ///</summary>
        TakeRampStraight,

        ///<summary>
        ///Take transit.
        ///</summary>
        TakeTransit,

        ///<summary>
        ///Transfer between public transit at transit stop.
        ///</summary>
        Transfer,

        ///<summary>
        ///Get off public transit at transit stop.
        ///</summary>
        TransitArrive,

        ///<summary>
        ///Get on public transit at transit stop.
        ///</summary>
        TransitDepart,

        ///<summary>
        ///Turn back sharply.
        ///</summary>
        TurnBack,

        ///<summary>
        ///Turn left.
        ///</summary>
        TurnLeft,

        ///<summary>
        ///Turn left and then bear left.
        ///</summary>
        TurnLeftThenBearLeft,

        ///<summary>
        ///Turn left and then bear right.
        ///</summary>
        TurnLeftThenBearRight,

        ///<summary>
        ///Turn left and then turn left again.
        ///</summary>
        TurnLeftThenTurnLeft,

        ///<summary>
        ///Turn left and then turn right.
        ///</summary>
        TurnLeftThenTurnRight,

        ///<summary>
        ///Turn right.
        ///</summary>
        TurnRight,

        ///<summary>
        ///Turn right and then bear left.
        ///</summary>
        TurnRightThenBearLeft,

        ///<summary>
        ///Turn right and then bear right.
        ///</summary>
        TurnRightThenBearRight,

        ///<summary>
        ///Turn right and then turn left.
        ///</summary>
        TurnRightThenTurnLeft,

        ///<summary>
        ///Turn right and then turn right again
        ///</summary>
        TurnRightThenTurnRight,

        ///<summary>
        ///Turn instruction followed by a merge instruction.
        ///</summary>
        TurnThenMerge,

        ///<summary>
        ///Turn left to stay on the same road.
        ///</summary>
        TurnToStayLeft,

        ///<summary>
        ///Turn right to stay on the same road.
        ///</summary>
        TurnToStayRight,

        ///<summary>
        ///The instruction is unknown.
        ///</summary>
        Unknown,

        ///<summary>
        ///Make a u-turn to go in the opposite direction.
        ///</summary>
        UTurn,

        ///<summary>
        ///Wait at a transit stop.
        ///</summary>
        Wait,

        ///<summary>
        ///Walk.
        ///</summary>
        Walk
    }
}
