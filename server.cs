if(ForceRequiredAddOn("Weapon_Grenades") == $Error::AddOn_NotFound)
	return error("Weapon_Traps Error: required add-on Weapon_Grenades not found");

if ($RTB::Hooks::ServerControl)
{
	RTB_registerPref("Max Player Traps", "Traps", "$Pref::XMines::trapLimit", "int 0 1000", "Weapon_Traps", 4, false, false, "");
	RTB_registerPref("Allow Stacking Mines", "Traps", "$Pref::XMines::mineStack", "bool", "Weapon_Traps", 0, false, false, "");
	RTB_registerPref("Allow Floating Mines", "Traps", "$Pref::XMines::mineFloat", "bool", "Weapon_Traps", 0, false, false, "");
}
else
{
	if ($Pref::XMines::trapLimit $= "") $Pref::XMines::trapLimit = 4;
	if ($Pref::XMines::mineStack $= "") $Pref::XMines::mineStack = 0;
	if ($Pref::XMines::mineFloat $= "") $Pref::XMines::mineFloat = 0;
}

$trapTriggerDebug = false;

exec("./package.cs");
exec("./functions.cs");
exec("./triggers.cs");

exec("./Weapon_APMine.cs");
exec("./Weapon_SAPMine.cs");
exec("./Weapon_ATMine.cs");
exec("./Weapon_ProxMine.cs");
exec("./Weapon_TripMine.cs");
exec("./Weapon_Claymore.cs");
exec("./Weapon_IncenMine.cs");
exec("./Weapon_ElectroMine.cs");
exec("./Weapon_DoorMine.cs");

// todo:
// * concussion blast + proxy
// * impulse proxy
// * poison spike
// * punji spikes
// * bear trap
// * barbed wire
// * spike strips
// * bouncing betty
// * caltrops