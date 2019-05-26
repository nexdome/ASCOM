```puml
@startuml
Title State Machine Classes
!include skin.puml
skinparam linetype ortho

interface IState
interface IRotatorState
interface IShutterState

IRotatorState <|-r- IState
IShutterState <|-l- IState

class ControllerStateMachine {
    -RotatorState : IRotatorState
    -ShutterState : IShutterState
}

namespace Rotator {
abstract class RotatorStateBase
class ReadyState<<Ready>>
.IRotatorState <|-- RotatorStateBase
RotatorStateBase <|-- ReadyState
RotatorStateBase <|-- RotatingState
RotatorStateBase <|-- RequestStatusState
}

namespace Shutter {
abstract class ShutterStateBase
class OpenState<<Ready>>
class ClosedState<<Ready>>
class OfflineState<<Warning>>
.IShutterState <|-- ShutterStateBase
ShutterStateBase <|-- OfflineState
ShutterStateBase <|-- OpenState
ShutterStateBase <|-- ClosedState
ShutterStateBase <|-- OpeningState
ShutterStateBase <|-- ClosingState
}
@enduml

```