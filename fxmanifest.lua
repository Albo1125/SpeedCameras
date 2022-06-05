fx_version 'cerulean'
games { 'gta5' }

name 'SpeedCameras'
author 'Albo1125'

files {
	'Newtonsoft.Json.dll',
	'cameras.json'
}
server_script "vars.lua"
server_script 'sv_SpeedCameras.lua'
client_script 'SpeedCameras.net.dll'