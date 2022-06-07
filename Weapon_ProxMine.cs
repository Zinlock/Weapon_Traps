datablock StaticShapeData(mine_proxyChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/mineProxy.dts";
	health = 45;

	triggerSound = mine_triggerProxySound;
	triggerDelay = 750;
	triggerDistance = 6;
	triggerSize = "12 12 12";
};

datablock AudioProfile(mine_triggerProxySound)
{
	filename    = "./wav/trigger_proxy.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock ItemData(mine_proxyItem : mine_impactItem)
{
	shapeFile = "./dts/mineProxy.dts";

	uiName = "[T] Proxy Mine";
	iconName = "./ico/PROXY";

	doColorShift = true;
	colorShiftColor = "0.45 0.45 0.45 1.0";

	image = mine_proxyImage;
};

datablock ShapeBaseImageData(mine_proxyImage : mine_impactImage)
{
	shapeFile = "./dts/mineProxyImage.dts";

	item = mine_proxyItem;

	mineShapeData = mine_proxyChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 180;
	mineDeploySound = mine_deploySound;
	mineDeployDistance = 5;

	doColorShift = mine_proxyItem.doColorShift;
	colorShiftColor = mine_proxyItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;
};

registerDataPref("Default Reserve Proxy Mines", "Proxy Mines", "Weapon_Traps", "int 0 1000", 1, false, false, mine_proxyImage, weaponUseCount);
registerDataPref("Max Reserve Proxy Mines", "Proxy Mines", "Weapon_Traps", "int 0 1000", 2, false, false, mine_proxyImage, weaponReserveMax);

function mine_proxyImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_proxyImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_proxyImage::onChargeStart(%this, %obj, %slot) { }

function mine_proxyImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_proxyChargeShape::onAdd(%data, %obj) { mine_impactChargeShape::onAdd(%data, %obj); }

function mine_proxyChargeShape::onRemove(%data, %obj) { mine_impactChargeShape::onRemove(%data, %obj); }