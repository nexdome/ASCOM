# ASCOM Server for NexDome - Instructions for Alpha Testers

## About the NexDome Control Suite

This software has been professionally produced by [Tigra Astronomy][Tigra] for [NexDome][NexDome].

![NexDome](assets/NexDome.png) 

Thanks for agreeing to test the software. As an alpha- or beta-tester, we have a few expectations of you beyond what we'd expect from normal end users. Both Alpha and Beta quality software are pre-release. Testing pre-release software comes with risks and responsibilities which you, the tester, must accept before using the software.

> `Alpha quality software` is computer software that is still in the early testing phase. It is functional enough to be used, but is unpolished and often lacks many of the features that will be included in the final version of the program. The "alpha phase" of software development follows the early programming and design stages, but precedes the "beta phase" in which the software closely resembles the final version.
>
> --- [Alpha Software. (2019, June 5). In Tech Terms, The Tech Terms Computer Dictionary.][alpha]
>
> `Beta quality software` refers to computer software that is undergoing testing and has not yet been officially released. [...] Since beta software is a pre-release version of the final application, it may be unstable or lack features that will be be included in the final release. Therefore, beta software often comes with a disclaimer that testers should use the software at their own risk. If you choose to beta test a program, be aware that it may not function as expected.
>
> --- [Beta Software. (2019, June 5). In Tech Terms, The Tech Terms Computer Dictionary.][beta]

You wil be testing [`Alpha quality software`][alpha] and you must understand and accept the implications of this.

The main objective is to gain early feedback on the suitability of the product and to identify features that still need to be implemented. As such, problems are almost gauranteed and your feedback will be critical in rounding out the product. The implied contract for us engaging with you and giving you access to pre-release software is that you test the software in good faith and report any and all problems you find to our issue tracker.

## [Issue Tracker][Issues]

We are using a [BitBucket online issue tracker][Issues] to manage our issues. [Click Here][Issues] to visit the tracker.

**IMPORTANT**: _Issues and any follow-up comments must be reported via the issue tracker_. We will ignore all reports made by other means including email. We appreciate and understand that this means a little more effort on your part, but there are good reasons for this strict requirement.

Note that anonymous issues are disabled. In theory, anonymous issues are a great idea but we've found that in practice we almost always need to follow up with the reporter. With anonymous issues, we have no idea who the reporter is so we can't follow up. This ends up just being a giant waste of time, both for us and the reporter, whose issue generally doesn't get addressed. Therefore we have reluctantly decided to disable anonymous issue creation.

## About the NexDome Control Suite by [Tigra Astronomy][Tigra]

The software control suite consists of several items.

- **ASCOM Server** - an ASCOM `IDomeV2` driver that's capable of acting as a hub and accepting connections from multiple clients. For example, you could have the driver connected in ACP and MaxIm DL at the same time, and perhaps even use the [ASCOM Alpaca Remote Server][Alpaca] to manage the dome remotely.
- **Firmware** - Tigra Astronomy has completely rewritten the firmware from the ground up and the ASCOM driver requires this new firmware. You will need to update the firmware in your control modules in order to use this software. Firmware is supplied as pre-compiled `*.hex` files. You do not need to install or use the Arduino IDE to use this firmware.
  - **Rotator firmware** - controls the rotator part of the dome and connects via USB to the host computer. File name ending in `*.Rotator.hex`.
  - **Shutter firmware** - controls the shutter part of the dome and communicates wirelessly with the Rotator. Can be connected via USB for diagnostic purposes and for updating the firmware, but no direct connection is normally required. File name ending in `*.Shutter.hex`.
  - **XBee Factory Reset firmware** - used to reset the XBee wireless modules to factory settings. Old firmware may not recognize the XBee module after it has been reconfigured for the new firmware. THe module can be reset to factory defaults using this special diagnostic firmware.
  - **Firmware Updater Utility** - a Windows command-line utility for uploading firmware to the rotator and shutter units.

  Instructions for updating the firmware are provided with the firmware files.

  It may be an idea to verify that the correct firmware is installed on both units before proceeding. You can use a terminal emulator to connect to the control modules. Serial settings are 115,200 baud, no parity, 8 data bits, 1 stop bit, RTS/CTS flow control, DTR line enabled. Once you are connected, you should see some sort of output about every 10 seconds. You may see a message such as `XB->Online` or some other output. If you type in the command `@FRR` on the rotator unit or `@FRS` on the shutter unit, then you should see the firmware verion reported as a [Semantic Version string][SemVer]. This may look similar to `1.0.0-beta.6`.

  ## Installing the ASCOM Server

  **Prerequisite**: You should have obtained and uploaded the matching firmware. The ASCOM server will not work with old firmware.

  During the alpha test, there is no installer and manual installation steps are required.

  1. The ASCOM server is provided in a Zip archive. Once you have a copy of this archive, you should extract it to a folder in a convenient location. We suggest putting it on the desktop or in your user home directory, but wherever you put it, don't move or rename the folder or things will break.
  2. Open a command prompt (Windows PowerShell works nicely). Change to the directory where you have saved the driver files and make sure you can see the main executable, `TA.NexDome.Server.exe`.
  3. Enter the command `TA.NexDome.Server.exe /register`
  4. The server will attempt to gain administrator rights in order to perform registration of the ASCOM driver. If you have UAC enabled on your system (recommended) then you will be prompted to allow the program to make changes to your computer. You must allow this or the software will not work. You may also receive a notice from Windows SmartScreen that the software is from an "unknown publisher". The software is not digitally signed so this is expected. If you receive this notice, click "more information" and/or "run anyway".
  5. Once registration is complete, the program will exit. At that point, the dome driver should appear in the ASCOM Chooser and will be available to ASCOM-compatible client applications.

## Using the Software ##

In use, the software is largely invisible and you control your dome using whatever ASCOM-compatible software you have. We have tested the software using MaxIm DL and from the command line using Windows PowerShell.

We can't provide any support or advice for your ASCOM client application. You should refer to the documentation for whatever software you use.

FOr performing simple tests, we find that Windows Powershell is quite a convenient way to quickly load the driver. From Window Powershell, you can load the driver like this:

```powershell
$dome = New-Object -ComObject ASCOM.NexDome.Dome
```

You can then display the Setup screen like this:
```powershell
$dome.SetupDialog()
```

Once you have configured the driver using the setup dialog, click OK and your settings are permanently saved. You should not need to do this again. Then you can connect to the dome hardware using:

```powershell
$dome.Connected=1
```

You can then use all the properties and methods of the driver to control and query the dome. You can see which properties and methods are available using:
```powershell
$dome | Get-Member
```

You can printe out the values of all the dome's properties in one go using just:
```powershell
$dome
```

When you're done, unload the driver using:
```powershell
$dome.Connected=0
$dome.Dispose()
```

## Status Display GUI

The ASCOM server is a small GUI that displays the dome state once it is connected. The display updates in real time to show ongoing motor activity, the rotator azimuth and home sensor, shutter percent open, shutter disposition (open, closed or error), and the status of the wireless link between the rotator and shutter.

![GUI]

In addition to serving as a status display panel, the GUI also offers limited controls:

- Opening and closing the shutter
- Displaying the settings screen
- Closing the server via the window close box (note: this forcibly disconnects all clients and should only be used to recover from a problem)


## Diagnostics

The driver produces a lot of diagnostic information which is very useful to us in diagnosing any problems you may need to report. Logs files are created in the `Logs` directory in your user profile home folder (usually `c:\users\your-username\logs`):

```powershell
C:\>
C:\> cd $Env:USERPROFILE
C:\Users\Tim> cd logs
C:\Users\Tim\logs> dir


    Directory: C:\Users\Tim\logs


Mode                LastWriteTime         Length Name
----                -------------         ------ ----
-a----       03/06/2019     23:44        2123334 TA.Nexdome.Server-2019-06-03-TYCO_Tim-TYCO.log
-a----       04/06/2019     23:57        1341561 TA.Nexdome.Server-2019-06-04-TYCO_Tim-TYCO.log
-a----       05/06/2019     15:34        5954550 TA.Nexdome.Server-2019-06-05-TYCO_Tim-TYCO.log
-a----       03/03/2019     23:16         159411 TA.WeatherListener.Server-2019-03-03-TYCO_Tim-TYCO.log


C:\Users\Tim\logs>
```

Log files are named `TA.NexDome.Server-{date}-{username}-{computername}.log`. When reporting an issue, please attach the log file for the date when your issue occurred, and try to provide the time as accurately as possible, so we can try to correlate what happened with the log file.

If you wish to view the diagnostic output in real time, you can use [SysInternals' DebugView][DbgVw] or [Binary Fortress' LogFusion][LogFu] to view, filter and highlight the live debug output, or you can open the log file with a good editor such as [Visual Studio Code][VSCode].

### Changing the Logging Settings

For advanced diagnostics, if you need more or less verbose output or would like to send diagnostics to different logging targets, you can edit the `NLog.config` file in the program directory. The default configuration is shown below. The last couple of lines of the file are particularly useful for controlling the logging level, by editing the `minlevel=""` attributes and setting them to (from most to least verbose) `Trace`, `Debug`, `Info`, `Warning`, `Error`. Note that setting `minlevel="Trace"` produces a _lot_ of output and may affect performance.

```xml
<?xml version="1.0" encoding="utf-8"?>

<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" autoReload="true">

  <!-- 
  See https://github.com/nlog/nlog/wiki/Configuration-file 
  for information on customizing logging rules and outputs.
   -->
  <targets async="true">
    <target xsi:type="Trace" name="DebugView"
            rawWrite="true"
            layout="${time}|${pad:padding=-5:inner=${uppercase:${level}}}|[NEX]${pad:padding=-16:inner=${callsite:className=true:fileName=false:includeSourcePath=false:methodName=false:includeNamespace=false}}|${message}" />
    <target xsi:type="File" name="LogFile"
            filename="${environment:variable=UserProfile}/logs/TA.Nexdome.Server-${filesystem-normalize:fSNormalize=true:inner=${shortdate}-${windows-identity}-${machinename}.log}"
            layout="${time}|${pad:padding=-5:inner=${uppercase:${level}}}|${pad:padding=-16:inner=${callsite:className=true:fileName=false:includeSourcePath=false:methodName=false:includeNamespace=false}}|${message}" />
  </targets>
  <rules>
    <logger name="*" minlevel="Trace" writeTo="DebugView" />
    <logger name="*" minlevel="Trace" writeTo="LogFile" />
  </rules>
</nlog>
```

## Reporting Issues

Rules of the Road for reporting issues.

**All issues must be reported via the [official issue tracker][Issues], no ifs, no buts, no exceptions**. If it's not in our issue tracker, it doesn't exist. Supporting information should be attached to the issue as file attachments or in the text of the description.

1. All issues **must contain** (as a file attachment) an appropriate diagnostic log captured with `minlevel="Trace"` ([see above](#changing-the-logging-settings)). The issue must state the time that the problem happend as accurately as possible, so that we can correlate it with the log file.
2. Please state the full semantic version string of the driver (from the About Box) and/or from the firmware (`@FRR` and `@FRS` commands). If you can see more than one version string, pick the longest one or include them all. You can copy and paste into the description or take a screen shot and attach that.
3. List steps to reprodice the problem. This is always a sticking point with issue reports because it places a lot of responsibility on the user, but without this we can't usually reproduce the problem and if we can't reproduce it, then we can't fix it. Issues without sufficient information may be closed without resolution.
4. Attach screen shots or videos to help with the previous item. A very under-used tool is the [Windows Problem Steps Recorder][PSR] (aka "Steps Recorder", or "PSR"). Try typing `psr` into the search box on your start menu. The tool records screen snapshots and users actions and produces a Zip archive that we can replay and see your problem happening. This file can be attached to your problem report.
5. Always include the full text of error messages and stack traces. Copy and paste, or take a screen shot.

The more detail you can provide, the more chance there is that we will be able to resolve your issue. We know it takes time and we know it's tedious.That's unavoidable and we are grateful when users make the effort, but ultimately it is in your own bets interests.

Thanks for your help.

| | |
|---|---|
| ![Tigra Astronomy](assets/Tigra-Astronomy.png) | **Tim Long**<br>Software Architect<br>[Tigra Astronomy][Tigra]<br>_Software, Instruments and Automation Systems for Astronomers_<br>[Tim@tigra-astronomy.com][mailto] |
| | |





[Issues]: https://bitbucket.org/tigra-astronomy/nexdome-ascom-driver/issues?status=new&status=open "BitBucket Issue Tracker for NexDome"
[Alpaca]: https://ascom-standards.org/Developer/Alpaca.htm "About ASCOM Alpaca"
[SemVer]: https://semver.org/ "Semantic versioning"
[GUI]: assets/StatusDisplay.png "ASCOM Server Status Display"
[DbgVw]: https://docs.microsoft.com/en-us/sysinternals/downloads/debugview "Live diagnostic output viewer"
[LogFu]: https://www.logfusion.ca/ "Log display, filtering and highlighting software"
[VSCode]: https://code.visualstudio.com/ "Lightweight, advanced, cross-platform text and code editor"
[PSR]: https://support.microsoft.com/en-us/help/22878/windows-10-record-steps "Windows Problem Steps Recorder (PSR)"
[Tigra]: http://tigra-astronomy.com "Tigra Astronomy - Software, Instruments and Automation Systems for Astronomers"
[mailto]: mailto:Tim@tigra-astronomy.com "Send me an email (but use the Issue Tracker to report issues!)"
[NexDome]: https://www.nexdome.com/ "Personal Observatories, Built Smarter"
[alpha]: https://techterms.com/definition/alpha_software "Definition of alpha-quality software"
[beta]: https://techterms.com/definition/beta_software "Definition of beta-quality software"