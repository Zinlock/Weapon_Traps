datablock StaticShapeData(mine_tankChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/mineAntiTank.dts";

	health = 80;
	deployTime = 1500;

	triggerDelay = 250;
	triggerProjectile = mine_tankBlastProjectile;
	triggerMask = $TypeMasks::VehicleObjectType;
};

datablock ExplosionData(mine_tankExplosion : grenade_concussionExplosion)
{
	soundProfile = mine_blastSound;

	impulseRadius = 5;
	impulseForce = 3500;

	damageRadius = 11;
	radiusDamage = 310;
};

datablock ProjectileData(mine_tankBlastProjectile)
{
	directDamageType  = $DamageType::mineTankDirect;
	radiusDamageType  = $DamageType::mineTankDirect;
	explosion           = mine_tankExplosion;

	explodeOnDeath        = false;  

	armingDelay         = 5000;
	lifetime            = 5000;
	fadeDelay           = 5000;
	bounceElasticity    = 0.0;
	bounceFriction  	  = 1.0;
};

datablock ItemData(mine_tankItem : mine_impactItem)
{
	shapeFile = "./dts/mineAntiTank.dts";

	uiName = "[T] Anti-Tank Mine";
	iconName = "./ico/TANK";

	doColorShift = true;
	colorShiftColor = "0.358 0.796 0.153 1.0";

	image = mine_tankImage;
};

datablock ShapeBaseImageData(mine_tankImage : mine_impactImage)
{
	shapeFile = "./dts/mineAntiTankImage.dts";

	item = mine_tankItem;

	mineShapeData = mine_tankChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 60;
	mineDeploySound = mine_deploySound;
	mineDeployDistance = 5;

	doColorShift = mine_tankItem.doColorShift;
	colorShiftColor = mine_tankItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;
};

registerDataPref("Default Reserve Anti-Tank Mines", "Anti-Tank Mines", "Weapon_Traps", "int 0 1000", 1, false, false, mine_tankImage, weaponUseCount);
registerDataPref("Max Reserve Anti-Tank Mines", "Anti-Tank Mines", "Weapon_Traps", "int 0 1000", 2, false, false, mine_tankImage, weaponReserveMax);

function mine_tankImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_tankImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_tankImage::onChargeStart(%this, %obj, %slot) { }

function mine_tankImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_tankChargeShape::onAdd(%data, %obj) { mine_impactChargeShape::onAdd(%data, %obj); }

function mine_tankChargeShape::onRemove(%data, %obj) { mine_impactChargeShape::onRemove(%data, %obj); }

function mine_tankBlastProjectile::radiusDamage(%this, %obj, %col, %distanceFactor, %pos, %damageAmt)
{
	if(%distanceFactor <= 0)
		return;
	else if(%distanceFactor > 1)
		%distanceFactor = 1;

	%damageAmt *= %distanceFactor;

	if(%damageAmt && checkLOS(%pos, %col, %obj.sourceShape))
	{
		//use default damage type if no damage type is given
		%damageType = $DamageType::Radius;
		if(%this.RadiusDamageType)
				%damageType = %this.RadiusDamageType;

		if(%col.getType() & ($TypeMasks::VehicleObjectType))
			%col.damage(%obj, %pos, %damageAmt, %damageType);
		else
			%col.damage(%obj, %pos, %damageAmt / 4, %damageType);
	}
}