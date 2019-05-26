```puml
@startuml
title Shutter State Machine
!include skin.puml
skinparam linetype arc

[*] --> Offline

state Offline <<Warning>>
Offline --> Closed : __XBee->Online && ClosedSensorActive__
Offline --> Open : __XBee->Online && NOT ClosedSensorActive__

state Closed <<Safe>>
Closed: OnEnter {Shutter motor = stopped}
Closed --> Opening : __ShutterOpeningReceived__
Closed --> Opening : __ShutterPositionReceived__
Closed --> Opening : __OpenShutterRequested__\nactions.OpenShutter
Closed --> Offline : __XBee offline__

state Open
Open: OnEnter {Shutter motor = stopped}
Open --> Closing : __ShutterClosingReceived__
Open --> Closing : __ShutterPositionReceived__
Open --> Closing : __CloseShutterRequested__\nactions.CloseShutter
Open --> Offline : __XBee offline__

state Opening
Opening: OnEnter {Shutter motor = active; direction = Opening}
Opening --> Open : __ShutterStatusReceived && OpenSensorActive__
Opening --> Opening : __ShutterPositionReceived__
Opening --> Offline : __XBee offline__

state Closing
Closing: OnEnter {Shutter motor = active; direction = Closing}
Closing --> Closed : __ShutterStatusReceived && ClosedSensorActive__
Closing --> Closing : __ShutterPositionReceived__
Closing --> Offline : __XBee offline__


@enduml
```