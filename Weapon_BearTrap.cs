datablock StaticShapeData(mine_bearChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/bearTrap.dts";

	health = 80;
	deployTime = 500;

	triggerDelay = 0;
	triggerSound = mine_triggerBearSound;
	triggerProjectile = mine_bearDestroyedProjectile;
	triggerMask = $TypeMasks::PlayerObjectType;
	triggerType = customTrigger128;
	triggerSize = "1.8 1.8 1";

	enterCallback = "trap_bearEnter";
	tickCallback = "trap_bearEnter";
};

datablock AudioProfile(mine_deployBearSound)
{
	filename    = "./wav/deploy_bear.wav";
	description = AudioShort3D;
	preload = true;

	pitchRange = 4;
};

datablock AudioProfile(mine_activateBearSound)
{
	filename    = "./wav/activate_bear.wav";
	description = AudioShort3D;
	preload = true;

	pitchRange = 4;
};

datablock AudioProfile(mine_triggerBearSound)
{
	filename    = "./wav/trigger_bear.wav";
	description = AudioDefault3D;
	preload = true;

	pitchRange = 4;
};

datablock AudioProfile(mine_triggerBreakBearSound)
{
	filename    = "./wav/trigger_bear_2.wav";
	description = AudioDefault3D;
	preload = true;

	pitchRange = 4;
};

datablock AudioProfile(mine_destroyBearSound)
{
	filename    = "./wav/destroy_bear.wav";
	description = AudioDefault3D;
	preload = true;

	pitchRange = 4;
};

datablock DebrisData(mine_bearJawBDebris : mine_scrapCogDebris) { shapeFile = "./dts/debrisBearJawB.dts"; };
datablock ExplosionData(mine_bearJawBExplosion : mine_scrapCogExplosion) { debris = mine_bearJawBDebris; debrisNum = 1; debrisNumVariance = 0; };

datablock DebrisData(mine_bearJawADebris : mine_bearJawBDebris) { shapeFile = "./dts/debrisBearJawA.dts"; };
datablock ExplosionData(mine_bearJawAExplosion : mine_bearJawBExplosion) { debris = mine_bearJawADebris; subExplosion[0] = mine_bearJawBExplosion; };

datablock DebrisData(mine_bearBodyDebris : mine_bearJawBDebris) { shapeFile = "./dts/debrisBearBody.dts"; };
datablock ExplosionData(mine_bearBodyExplosion : mine_bearJawBExplosion) { debris = mine_bearBodyDebris; subExplosion[0] = mine_bearJawAExplosion; };

datablock ExplosionData(mine_bearExplosion)
{
	lifeTimeMS = 150;

	subExplosion[1] = mine_bearBodyExplosion;

	soundProfile = mine_destroyBearSound;
	explosionScale = "1 1 1";

	particleEmitter = "";
	particleDensity = 100;
	particleRadius = 0.2;

	impulseRadius = 0;
	impulseForce = 0;

	damageRadius = 0;
	radiusDamage = 0;
};

datablock ProjectileData(mine_bearDestroyedProjectile)
{
	directDamageType  = $DamageType::mineStationDirect;
	radiusDamageType  = $DamageType::mineStationDirect;
	explosion           = mine_bearExplosion;

	explodeOnDeath        = false;  

	armingDelay         = 5000;
	lifetime            = 5000;
	fadeDelay           = 5000;
	bounceElasticity    = 0.0;
	bounceFriction  	  = 1.0;
};

datablock ItemData(mine_bearItem : mine_impactItem)
{
	shapeFile = "./dts/bearTrapItem.dts";

	uiName = "[T] Bear Trap";
	iconName = "./ico/bear";

	doColorShift = true;
	colorShiftColor = "0.8 0.8 0.8 1.0";

	image = mine_bearImage;
};

datablock ShapeBaseImageData(mine_bearImage : mine_impactImage)
{
	shapeFile = "./dts/bearTrapImage.dts";

	item = mine_bearItem;

	mineShapeData = mine_bearChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 60;
	mineDeploySound = mine_deployBearSound;
	mineDeployDistance = 5;

	damage = 25;
	escapeVelocity = 20;
	escapeDamage = 25;
	stunTime = 2;

	doColorShift = mine_bearItem.doColorShift;
	colorShiftColor = mine_bearItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;
};

function mine_bearImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_bearImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_bearImage::onChargeStart(%this, %obj, %slot) { }

function mine_bearImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_bearChargeShape::onAdd(%data, %obj)
{
	%obj.playThread(0, rootClose);
	%obj.schedule(%data.deployTime, playThread, 0, open);
	schedule(%data.deployTime, %obj, serverPlay3D, mine_activateBearSound, %obj.getPosition());

	mine_impactChargeShape::onAdd(%data, %obj);
}

function mine_bearChargeShape::onRemove(%data, %obj) { mine_impactChargeShape::onRemove(%data, %obj); }

function trap_bearEnter(%trigger, %hit)
{
	if(%trigger.sourceShape.dead)
		return;

	if(!(%hit.getType() & %trigger.mask))
		return;

	if(getSimTime() - %trigger.creationTime < %trigger.deployTime)
		return;
	
	if(!checkLOS(%trigger.sourceShape.getWorldBoxCenter(), %hit, %trigger.sourceShape))
		return;

	if(!isObject(%hit.activeBearTrap) && !%trigger.triggered && mineCanTrigger(%trigger.sourceClient, %hit))
	{
		%trigger.triggered = true;
		%trigger.sourceShape.schedule(mine_bearImage.stunTime * 1000, mineExplode, %trigger.explosionScale, vectorScale(%trigger.sourceShape.getUpVector(), %trigger.explosionOffset));
		serverPlay3D(%trigger.sound, %trigger.getPosition());

		%trigger.sourceShape.playThread(0, clap);

		%hit.playThread(0, activate2);

		%hit.damage(%trigger.sourceShape.client, %trigger.sourceShape.getPosition(), mine_bearImage.damage, $DamageType::Direct);

		%hit.activeBearTrap = %trigger.sourceShape;

		trap_bearLoop(%trigger, %hit);
	}
}

function trap_bearLoop(%trigger, %hit)
{
	cancel(%trigger.bearLoop);

	if(!isObject(%hit))
		return;

	if(vectorLen(%hit.getVelocity()) > mine_bearImage.escapeVelocity)
	{
		serverPlay3D(mine_triggerBreakBearSound, %trigger.getPosition());
		%hit.damage(%trigger.sourceShape.client, %trigger.sourceShape.getPosition(), mine_bearImage.escapeDamage, $DamageType::Direct);
		%trigger.sourceShape.mineExplode(%trigger.explosionScale, vectorScale(%trigger.sourceShape.getUpVector(), %trigger.explosionOffset));
		return;
	}

	%hit.setTransform(%trigger.sourceShape.getPosition());
	%hit.setVelocity("0 0 0");

	%trigger.bearLoop = schedule(64, %trigger, trap_bearLoop, %trigger, %hit);
}