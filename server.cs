if(ForceRequiredAddOn("Weapon_Grenades") == $Error::AddOn_NotFound)
	return error("Weapon_Traps Error: required add-on Weapon_Grenades not found");

if ($RTB::Hooks::ServerControl)
{
	RTB_registerPref("Max Player Traps", "Traps", "$Pref::XMines::trapLimit", "int 0 1000", "Weapon_Traps", 4, false, false, "");
	RTB_registerPref("Allow Stacking Traps", "Traps", "$Pref::XMines::mineStack", "bool", "Weapon_Traps", 0, false, false, "");
	RTB_registerPref("Allow Floating Traps", "Traps", "$Pref::XMines::mineFloat", "bool", "Weapon_Traps", 0, false, false, "");
	RTB_registerPref("Clear Traps on Death", "Traps", "$Pref::XMines::mineClear", "bool", "Weapon_Traps", 0, false, false, "");
}
else
{
	if ($Pref::XMines::trapLimit $= "") $Pref::XMines::trapLimit = 4;
	if ($Pref::XMines::mineStack $= "") $Pref::XMines::mineStack = 0;
	if ($Pref::XMines::mineFloat $= "") $Pref::XMines::mineFloat = 0;
	if ($Pref::XMines::mineClear $= "") $Pref::XMines::mineClear = 0;
}

$trapTriggerDebug = false;

exec("./package.cs");
exec("./functions.cs");
exec("./triggers.cs");

exec("./Explosion_Scrap.cs");
exec("./Weapon_APMine.cs");
exec("./Weapon_SAPMine.cs");
exec("./Weapon_ATMine.cs");
exec("./Weapon_ProxMine.cs");
exec("./Weapon_TripMine.cs");
exec("./Weapon_Claymore.cs");
exec("./Weapon_IncenMine.cs");
exec("./Weapon_ElectroMine.cs");
exec("./Weapon_HealStation.cs");
exec("./Weapon_BearTrap.cs");
exec("./Weapon_ClusterMine.cs");
exec("./Weapon_EnergyStation.cs"); // todo: clean this code up

registerDataPref("Default Reserve Anti-Tank Mines", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_tankImage, weaponUseCount);
registerDataPref("Max Reserve Anti-Tank Mines", "Ammo", "Weapon_Traps", "int 0 1000", 2, false, false, mine_tankImage, weaponReserveMax);

registerDataPref("Default Reserve Bear Traps", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_bearImage, weaponUseCount);
registerDataPref("Max Reserve Bear Traps", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_bearImage, weaponReserveMax);

registerDataPref("Default Reserve Blast Mines", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_impactImage, weaponUseCount);
registerDataPref("Max Reserve Blast Mines", "Ammo", "Weapon_Traps", "int 0 1000", 2, false, false, mine_impactImage, weaponReserveMax);

registerDataPref("Default Reserve Claymores", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_claymoreImage, weaponUseCount);
registerDataPref("Max Reserve Claymores", "Ammo", "Weapon_Traps", "int 0 1000", 2, false, false, mine_claymoreImage, weaponReserveMax);

registerDataPref("Default Reserve Cluster Mines", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_ClusterImage, weaponUseCount);
registerDataPref("Max Reserve Cluster Mines", "Ammo", "Weapon_Traps", "int 0 1000", 2, false, false, mine_ClusterImage, weaponReserveMax);

registerDataPref("Default Reserve Electric Mines", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_ElectroImage, weaponUseCount);
registerDataPref("Max Reserve Electric Mines", "Ammo", "Weapon_Traps", "int 0 1000", 2, false, false, mine_ElectroImage, weaponReserveMax);

registerDataPref("Default Reserve Energy Stations", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, trap_energyImage, weaponUseCount);
registerDataPref("Max Reserve Energy Stations", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, trap_energyImage, weaponReserveMax);

registerDataPref("Default Reserve Health Stations", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, trap_healthImage, weaponUseCount);
registerDataPref("Max Reserve Health Stations", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, trap_healthImage, weaponReserveMax);

registerDataPref("Default Reserve Incendiary Mines", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_IncendiaryImage, weaponUseCount);
registerDataPref("Max Reserve Incendiary Mines", "Ammo", "Weapon_Traps", "int 0 1000", 2, false, false, mine_IncendiaryImage, weaponReserveMax);

registerDataPref("Default Reserve Laser Tripmines", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_tripImage, weaponUseCount);
registerDataPref("Max Reserve Laser Tripmines", "Ammo", "Weapon_Traps", "int 0 1000", 2, false, false, mine_tripImage, weaponReserveMax);

registerDataPref("Default Reserve Proxy Mines", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_proxyImage, weaponUseCount);
registerDataPref("Max Reserve Proxy Mines", "Ammo", "Weapon_Traps", "int 0 1000", 2, false, false, mine_proxyImage, weaponReserveMax);

registerDataPref("Default Reserve Stealth Mines", "Ammo", "Weapon_Traps", "int 0 1000", 1, false, false, mine_stealthImage, weaponUseCount);
registerDataPref("Max Reserve Stealth Mines", "Ammo", "Weapon_Traps", "int 0 1000", 2, false, false, mine_stealthImage, weaponReserveMax);

registerDataPref("Damage (25)", "Bear Traps", "Weapon_Traps", "int 0 1000", 25, false, false, mine_bearImage, damage);
registerDataPref("Escape Velocity (20)", "Bear Traps", "Weapon_Traps", "int 0 200", 20, false, false, mine_bearImage, escapeVelocity);
registerDataPref("Escape Damage (25)", "Bear Traps", "Weapon_Traps", "int 0 1000", 25, false, false, mine_bearImage, escapeDamage);
registerDataPref("Stun Time (2s)", "Bear Traps", "Weapon_Traps", "num 0 300", 2, false, false, mine_bearImage, stunTime);

registerDataPref("Energy Regen (1.5)", "Energy Stations", "Weapon_Traps", "num 0 1000", 1, false, false, trap_energyImage, tickHeal);
registerDataPref("Max Energy Regen (0)", "Energy Stations", "Weapon_Traps", "int 0 1000", 0, false, false, trap_energyImage, energyCharge);
registerDataPref("Despawn Time (0s)", "Energy Stations", "Weapon_Traps", "int 0 1000", 0, false, false, trap_energyImage, timeout);
registerDataPref("Can Recover (Y)", "Energy Stations", "Weapon_Traps", "bool", 1, false, false, trap_energyImage, mineCanRecover);
registerDataPref("Can Stack Charging (N)", "Energy Stations", "Weapon_Traps", "bool", 0, false, false, trap_energyImage, energyStack);
registerDataPref("Can Charge Enemies (N)", "Energy Stations", "Weapon_Traps", "bool", 0, false, false, trap_energyImage, energyTeam);
registerDataPref("Can Charge Turrets (Y)", "Energy Stations", "Weapon_Traps", "bool", 1, false, false, trap_energyImage, energyTurrets);

registerDataPref("Health Regen (1.5)", "Health Stations", "Weapon_Traps", "num 0 1000", 1.5, false, false, trap_healthImage, tickHeal);
registerDataPref("Max Health Regen (0)", "Health Stations", "Weapon_Traps", "int 0 1000", 0, false, false, trap_healthImage, healthCharge);
registerDataPref("Despawn Time (0s)", "Health Stations", "Weapon_Traps", "int 0 1000", 0, false, false, trap_healthImage, timeout);
registerDataPref("Can Recover (Y)", "Health Stations", "Weapon_Traps", "bool", 1, false, false, trap_healthImage, mineCanRecover);
registerDataPref("Can Stack Healing (N)", "Health Stations", "Weapon_Traps", "bool", 0, false, false, trap_healthImage, healthStack);
registerDataPref("Can Heal Enemies (N)", "Health Stations", "Weapon_Traps", "bool", 0, false, false, trap_healthImage, healthTeam);
registerDataPref("Can Heal Turrets (N)", "Health Stations", "Weapon_Traps", "bool", 0, false, false, trap_healthImage, healthTurrets);

registerDataPref("Tripmine Laser Length (16u)", "Laser Tripmines", "Weapon_Traps", "int 0 1024", 16, false, false, mine_tripImage, tripBeamLength);

registerDataPref("Stealth Mine Opacity (7%)", "Stealth Mines", "Weapon_Traps", "int 0 100", 7, false, false, mine_stealthChargeShape, opacity);

// todo:
// * concussion blast + proxy
// * impulse proxy
// * poison spike
// * punji spikes
// * barbed wire
// * spike strips
// * bouncing betty
// * caltrops