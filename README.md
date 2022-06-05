# SpeedCameras (+ UK Road Signs YMAPs)
SpeedCameras is a resource for FiveM by Albo1125 that provides fixed speed camera script functionality. The release also includes custom UK road signs ymaps by Albo1125 that place additional traffic signs, motorway barriers and speed cameras around the map. This is based on Razor792's UK road signs textures, which have been converted into streaming folder format. Download the latest release at [https://github.com/Albo1125/SpeedCameras/releases](https://github.com/Albo1125/SpeedCameras/releases)

## Installation & Usage
1. Download the latest release.
2. Unzip the SpeedCameras folder into your resources folder on your FiveM server.
3. Add the following to your server.cfg file:
```text
ensure SpeedCameras
ensure BritishRoadSigns
```

4. The `BritishRoadSigns/stream/roadsigns` files come from [here](https://www.gta5-mods.com/misc/united-kingdom-road-signs) and have been added into FiveM streaming folder format for convenience. Optionally, you can customise this.
5. The `BritishRoadSigns/stream/ymaps` ymap files by Albo1125 contain additional UK road sign placements to appropriately place speed limit signs and speed cameras for the default `cameras.json`. Optionally, you can customise this.
5. Optionally, customise the SpeedCameras notifications whitelist in `vars.lua` and add identifiers. This affects all the below commands.
6. Optionally, customise the speed camera locations and names in `cameras.json`.
7. Optionally, customise the commands in `sv_SpeedCameras.lua`.

## Commands
* /speedcams - Toggle speed camera blips
* /speedcamsub - Subscribe to speed camera hit notifications (there is a speeding tolerance of 12 MPH by default, so a driver would have to be travelling at 42 MPH in a 30 MPH limit to be flashed).
* /speedcamunsub - Unsubscribe from speed camera hit notifications


## Improvements & Licencing
Please view the license. Improvements and new feature additions are very welcome, please feel free to create a pull request. As a guideline, please do not release separate versions with minor modifications, but contribute to this repository directly. However, if you really do wish to release modified versions of my work, proper credit is always required and you should always link back to this original source and respect the licence.

## Libraries used (many thanks to their authors)
* [CitizenFX.Core.Client](https://www.nuget.org/packages/CitizenFX.Core.Client)
* [Newtonsoft.Json](https://www.nuget.org/packages/newtonsoft.json/12.0.2)
* [Razor792's UK road signs](https://www.gta5-mods.com/misc/united-kingdom-road-signs)