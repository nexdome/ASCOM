```puml
@startuml
Title ASCOM Dome Server Block Functional Diagram
!include skin.puml


package "Reactive Communications for ASCOM" {
    component ICommunicationsChannel
}

package "ASCOM Server" {
    component [Settings] 
    component [Client Connection Manager] as CCM
    component [Status Display GUI] as GUI
    [Shared Resources] --> CCM
    GUI --> CCM
    CCM --> ICommunicationsChannel
}

package "Device Interface Layer" {
    component [Device Controller] as [Device]
    component [Controller Actions] as Actions
    component [Rotator State Machine] as Rotator
    component [Shutter State Machine] as Shutter

    Actions ..> ICommunicationsChannel

    Device --> Rotator
    Device --> Shutter
    Device - ViewModel
    Device <-- CCM
    GUI --> ViewModel

    Rotator --> Actions
    Rotator <.. ICommunicationsChannel : IObservable<char>
    Shutter --> Actions
    Shutter <.. ICommunicationsChannel : IObservable<char>

}
package "ASCOM Dome Driver" {
    component IDomeV2 as Dome
    Dome --> [Device]
    Dome --> Settings
    Dome --> [Shared Resources]
}

[Rotator Hardware] <.r.> [Shutter Hardware] : XBee PAN wireless link
ICommunicationsChannel <..> [Rotator Hardware]

@enduml
```

