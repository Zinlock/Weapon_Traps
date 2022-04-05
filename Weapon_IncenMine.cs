datablock StaticShapeData(mine_IncendiaryChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/mineIncendiary.dts";
	health = 45;

	triggerSound = mine_triggerProxySound;
	triggerProjectile = grenade_mollyProjectile;
	triggerDelay = 750;
	triggerDistance = 6;
	triggerSize = "12 12 12";
};

datablock ItemData(mine_IncendiaryItem : mine_impactItem)
{
	shapeFile = "./dts/mineIncendiary.dts";

	uiName = "[T] Incendiary Mine";
	iconName = "./ico/INCEN";

	doColorShift = true;
	colorShiftColor = "0.633 0.057 0.048 1.0";

	image = mine_IncendiaryImage;
};

datablock ShapeBaseImageData(mine_IncendiaryImage : mine_impactImage)
{
	shapeFile = "./dts/mineIncendiaryImage.dts";

	item = mine_IncendiaryItem;

	mineShapeData = mine_IncendiaryChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 180;
	mineDeploySound = mine_deploySound;
	mineDeployDistance = 5;

	doColorShift = mine_IncendiaryItem.doColorShift;
	colorShiftColor = mine_IncendiaryItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;
};

function mine_IncendiaryImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_IncendiaryImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_IncendiaryImage::onChargeStart(%this, %obj, %slot) { }

function mine_IncendiaryImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_IncendiaryChargeShape::onAdd(%data, %obj) { mine_impactChargeShape::onAdd(%data, %obj); }

function mine_IncendiaryChargeShape::onRemove(%data, %obj) { mine_impactChargeShape::onRemove(%data, %obj); }