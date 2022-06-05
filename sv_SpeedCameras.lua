local subscribed = {}

function speedcams(source, args, rawCommand)
	if isAuthorized(source) then
		TriggerClientEvent("SpeedCameras:BlipsToggle", source)
	end
end

RegisterCommand('speedcams', speedcams, false)

function speedcamsub(source, args, rawCommand)
	if isAuthorized(source) then
		print("Subscribed")
		table.insert(subscribed, source)
	end
end

RegisterCommand('speedcamsub', speedcamsub, false)

function speedcamunsub(source, args, rawCommand)
	if isAuthorized(source) then
		print("Unsubscribed")
		for i=#subscribed,1,-1 do
			if subscribed[i] == source then
				table.remove(subscribed, i)
			end
		end
	end
end

RegisterCommand('speedcamunsub', speedcamunsub, false)

RegisterServerEvent("SpeedCameras:svFlash")
AddEventHandler('SpeedCameras:svFlash', function(x, y, z, heading)
	TriggerClientEvent("SpeedCameras:Flash", -1, x, y, z, heading)
end)

RegisterServerEvent("SpeedCameras:svCameraHit")
AddEventHandler('SpeedCameras:svCameraHit', function(source, infomessage)
	for subCount = 1, #subscribed do
		TriggerClientEvent("SpeedCameras:CameraHit", subscribed[subCount], GetPlayerName(source).. ' '..infomessage)
	end
end)
