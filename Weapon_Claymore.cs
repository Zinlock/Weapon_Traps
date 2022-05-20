datablock StaticShapeData(mine_claymoreChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/mineClaymore.dts";
	health = 30;

	explodeOnDetach = true;
	explosionForward = true;

	triggerSound = mine_triggerClaymoreSound;
	triggerProjectile = mine_claymoreBlastProjectile;
	triggerDistance = 4;
	triggerSize = "8 8 8";
	triggerAngle = 70;
};

datablock ExplosionData(mine_claymoreExplosion : grenade_remoteChargeExplosion)
{
	soundProfile = mine_blastTripSound;
	explosionScale = "0.5 0.5 0.5";

	particleEmitter = mine_tripExplosionCloudEmitter;
	particleDensity = 100;
	particleRadius = 0.2;

	emitter[0] = mine_tripExplosionHazeEmitter;
	emitter[1] = mine_tripExplosionDebrisEmitter;
	emitter[2] = mine_tripExplosionDebris2Emitter;
	emitter[3] = mine_tripExplosionConeEmitter;

	impulseRadius = 13;
	impulseForce = 700;

	damageRadius = 9;
	radiusDamage = 125;
};

datablock ProjectileData(mine_claymoreBlastProjectile)
{
	directDamageType  = $DamageType::mineClaymoreDirect;
	radiusDamageType  = $DamageType::mineClaymoreDirect;
	explosion           = mine_claymoreExplosion;

	explodeOnDeath        = false;  

	armingDelay         = 5000;
	lifetime            = 5000;
	fadeDelay           = 5000;
	bounceElasticity    = 0.0;
	bounceFriction  	  = 1.0;
};

datablock AudioProfile(mine_deployClaymoreSound)
{
	filename    = "./wav/deploy_claymore.wav";
	description = AudioShort3D;
	preload = true;
};

datablock AudioProfile(mine_triggerClaymoreSound)
{
	filename    = "./wav/trigger_claymore.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock ItemData(mine_claymoreItem : mine_impactItem)
{
	shapeFile = "./dts/mineClaymoreItem.dts";

	uiName = "[T] Claymore";
	iconName = "./ico/CLAYMORE";

	doColorShift = true;
	colorShiftColor = "0.244 0.736 0.364 1.0";

	image = mine_claymoreImage;
};

datablock ShapeBaseImageData(mine_claymoreImage : mine_impactImage)
{
	shapeFile = "./dts/mineClaymoreImage.dts";

	item = mine_claymoreItem;

	mineShapeData = mine_claymoreChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 60;
	mineDeploySound = mine_deployClaymoreSound;
	mineDeployDistance = 5;

	doColorShift = mine_claymoreItem.doColorShift;
	colorShiftColor = mine_claymoreItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;
};

function mine_claymoreImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_claymoreImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_claymoreImage::onChargeStart(%this, %obj, %slot) { }

function mine_claymoreImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_claymoreImage::onMinePlaced(%this, %obj, %slot, %mine)
{
	%mat = %obj.getTransform();

	%matrix = vectorToMatrix(%mine.getUpVector());

	if(%mine.getUpVector() $= "0 0 1")
		%matrix = MatrixMultiply("0 0 0 " @ %mine.getUpVector() SPC (getWord(%mat, 6) * getWord(%mat, 5)), %matrix);
	else
		%matrix = MatrixMultiply("0 0 0 " @ %mine.getUpVector() SPC (getWord(%mat, 6) * getWord(%mat, 5) + $pi/2), %matrix); // this gets less accurate as the slope angle increases

	%matrix = MatrixMultiply(%mine.getPosition(), %matrix);

	%mine.setTransform(%matrix);
}

function mine_claymoreChargeShape::onAdd(%data, %obj)
{
	mine_impactChargeShape::onAdd(%data, %obj);

	%obj.hideNode("beams");
	%obj.schedule(%data.deployTime, unHideNode, "beams");

	schedule(%data.deployTime, %obj, serverPlay3D, grenade_remoteBeepSound, %obj.getPosition());
}

function mine_claymoreChargeShape::onRemove(%data, %obj) { mine_impactChargeShape::onRemove(%data, %obj); }

function mine_claymoreBlastProjectile::radiusDamage(%this, %obj, %col, %distanceFactor, %pos, %damageAmt)
{
	if(%distanceFactor <= 0)
		return;
	else if(%distanceFactor > 1)
		%distanceFactor = 1;

	%damageAmt *= %distanceFactor;

	if(vectorDot(vectorNormalize(%obj.getVelocity()), vectorNormalize(vectorSub(%col.getPosition(), %obj.getPosition()))) < 0)
		%damageAmt /= 3;

	if(%damageAmt && checkLOS(%pos, %col, %obj.sourceShape))
	{
		//use default damage type if no damage type is given
		%damageType = $DamageType::Radius;
		if(%this.RadiusDamageType)
				%damageType = %this.RadiusDamageType;

		if(%col.getType() & ($TypeMasks::PlayerObjectType))
			%col.damage(%obj, %pos, %damageAmt, %damageType);
		else
			%col.damage(%obj, %pos, %damageAmt / 3, %damageType);
	}
}