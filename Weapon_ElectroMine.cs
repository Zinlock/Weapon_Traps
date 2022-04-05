datablock StaticShapeData(mine_ElectroChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/mineElectro.dts";
	health = 45;

	triggerSound = mine_triggerProxySound;
	triggerProjectile = grenade_electroProjectile;
	triggerDelay = 750;
	triggerDistance = 6;
	triggerSize = "12 12 12";
};

datablock ItemData(mine_ElectroItem : mine_impactItem)
{
	shapeFile = "./dts/mineElectro.dts";

	uiName = "[T] Electric Mine";
	iconName = "./ico/ELECTRO";

	doColorShift = true;
	colorShiftColor = "0.2 0.25 0.325 1.0";

	image = mine_ElectroImage;
};

datablock ShapeBaseImageData(mine_ElectroImage : mine_impactImage)
{
	shapeFile = "./dts/mineElectroImage.dts";

	item = mine_ElectroItem;

	mineShapeData = mine_ElectroChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 180;
	mineDeploySound = mine_deploySound;
	mineDeployDistance = 5;

	doColorShift = mine_ElectroItem.doColorShift;
	colorShiftColor = mine_ElectroItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;
};

function mine_ElectroImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_ElectroImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_ElectroImage::onChargeStart(%this, %obj, %slot) { }

function mine_ElectroImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_ElectroChargeShape::onAdd(%data, %obj) { mine_impactChargeShape::onAdd(%data, %obj); }

function mine_ElectroChargeShape::onRemove(%data, %obj) { mine_impactChargeShape::onRemove(%data, %obj); }