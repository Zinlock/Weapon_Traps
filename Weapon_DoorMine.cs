exec("./Weapon_DoorMine_Particles.cs");

datablock StaticShapeData(mine_DoorChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/mineDoor.dts";
	health = 45;

	dropOnDetach = false;
	explodeOnDetach = true;

	delayCleanup = true;

	explosionDown = true;
	explosionScale = 0.75;

	triggerDelay = 0;
	triggerProjectile = mine_doorBlastProjectile;

	enterCallback = "";
	tickCallback = "";
	leaveCallback = "";
};

datablock ExplosionData(mine_doorExplosion : grenade_remoteChargeExplosion)
{
	soundProfile = grenade_electroExplosionSound;
	explosionScale = "0.5 0.5 0.5";

	particleEmitter = mine_doorExplosionHazeEmitter;
	particleDensity = 100;
	particleRadius = 0.2;

	emitter[0] = mine_doorExplosionCloudEmitter;
	emitter[1] = mine_tripExplosionDebris2Emitter;
	
	impulseRadius = 0;
	impulseForce = 0;

	damageRadius = 10;
	radiusDamage = 1;
};

datablock ProjectileData(mine_doorBlastProjectile)
{
	directDamageType  = $DamageType::mineTripDirect;
	radiusDamageType  = $DamageType::mineTripDirect;
	explosion           = mine_doorExplosion;

	explodeOnDeath        = false;  

	armingDelay         = 5000;
	lifetime            = 5000;
	fadeDelay           = 5000;
	bounceElasticity    = 0.0;
	bounceFriction  	  = 1.0;
};

datablock AudioProfile(mine_triggerDoorSound)
{
	filename    = "./wav/trigger_Door.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock ItemData(mine_DoorItem : mine_impactItem)
{
	shapeFile = "./dts/mineDoor.dts";

	uiName = "[T] Door Mine";
	iconName = "./ico/DOOR";

	doColorShift = true;
	colorShiftColor = "0.45 0.45 0.45 1.0";

	image = mine_DoorImage;
};

datablock ShapeBaseImageData(mine_DoorImage : mine_impactImage)
{
	shapeFile = "./dts/mineDoorImage.dts";

	item = mine_DoorItem;

	mineShapeData = mine_DoorChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 180;
	mineDeploySound = mine_deploySound;
	mineDeployDistance = 5;

	doColorShift = mine_DoorItem.doColorShift;
	colorShiftColor = mine_DoorItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;
};

function mine_DoorImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_DoorImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_DoorImage::onChargeStart(%this, %obj, %slot) { }

function mine_DoorImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_DoorChargeShape::onAdd(%data, %obj) { mine_impactChargeShape::onAdd(%data, %obj); }

function mine_DoorChargeShape::onRemove(%data, %obj) { mine_impactChargeShape::onRemove(%data, %obj); }

function mine_doorBlastProjectile::radiusDamage(%this, %obj, %col, %distanceFactor, %pos, %damageAmt)
{
	if(vectorDot(vectorNormalize(%obj.getVelocity()), vectorNormalize(vectorSub(%col.getPosition(), %obj.getPosition()))) > 0.65 && checkLOS(%pos, %col, %obj.sourceShape))
	{
		%col.makeDeaf(1.25);
		%col.makeBlind(1.5);
		
		if(%col.zapTicks <= 0)
			%col.zapTicks = 10;
		else
			%col.zapTicks += 10;
		
		%col.zapDamage = 0.9;
		
		%col.zap();
	}
}