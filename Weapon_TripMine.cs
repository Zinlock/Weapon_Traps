exec("./Weapon_TripMine_Particles.cs");

datablock StaticShapeData(mine_tripChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/mineTrip.dts";
	health = 45;

	explodeOnDetach = true;

	delayCleanup = true;

	triggerDelay = 225;
	triggerSound = mine_triggerTripSound;
	triggerProjectile = mine_tripBlastProjectile;
	triggerSize = "0.1 0.1 0.1";
};

datablock StaticShapeData(mine_tripBeamShape) { shapeFile = "./dts/mineTripBeam.dts"; };

datablock AudioProfile(mine_triggerTripSound)
{
	filename    = "./wav/trigger_trip.wav";
	description = AudioDefault3D;
	preload = true;

	pitchRange = 2;
};

datablock AudioProfile(mine_blastTripSound)
{
	filename    = "./wav/blast_trip.wav";
	description = AudioDefault3D;
	preload = true;

	pitchRange = 4;
};

datablock ExplosionData(mine_tripExplosion : grenade_remoteChargeExplosion)
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

	damageRadius = 28; // doesn't actually deal damage over 28 units,
	blastRadius = 8;	// this is the real blast radius applied around the tripmine's beam in a pill shape instead of a sphere
	radiusDamage = 150;
};

datablock ProjectileData(mine_tripBlastProjectile)
{
	directDamageType  = $DamageType::mineTripDirect;
	radiusDamageType  = $DamageType::mineTripDirect;
	explosion           = mine_tripExplosion;

	explodeOnDeath        = false;  

	armingDelay         = 5000;
	lifetime            = 5000;
	fadeDelay           = 5000;
	bounceElasticity    = 0.0;
	bounceFriction  	  = 1.0;
};

datablock ItemData(mine_tripItem : mine_impactItem)
{
	shapeFile = "./dts/mineTrip.dts";

	uiName = "[T] Laser Tripmine";
	iconName = "./ico/TRIP";

	doColorShift = true;
	colorShiftColor = "0.65 0.65 0.65 1.0";

	image = mine_tripImage;
};

datablock ShapeBaseImageData(mine_tripImage : mine_impactImage)
{
	shapeFile = "./dts/mineTripImage.dts";

	item = mine_tripItem;

	mineShapeData = mine_tripChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 180;
	mineDeploySound = mine_deploySound;
	mineDeployDistance = 5;

	tripBeamLength = 16;

	doColorShift = mine_tripItem.doColorShift;
	colorShiftColor = mine_tripItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = $Pref::XMines::mineMax;
};

function mine_tripImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_tripImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_tripImage::onChargeStart(%this, %obj, %slot) { }

function mine_tripImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_tripImage::minePlaceCheck(%this, %obj, %slot, %ray)
{
	if(!(%ray2 = containerRayCast(posFromRaycast(%ray), vectorAdd(posFromRaycast(%ray), vectorScale(normalFromRaycast(%ray), %this.tripBeamLength)), $TypeMasks::FxBrickObjectType | $TypeMasks::StaticObjectType)))
	{
		%obj.client.centerPrint("<font:arial:14>\c5No surface to connect to!", 1.5);
		return 0;
	}

	return Parent::minePlaceCheck(%this, %obj, %slot, %ray);
}

function mine_tripImage::onMinePlaced(%this, %obj, %slot, %mine)
{
	%data = %mine.getDatablock();
	%pos = %mine.getPosition();

	%ray = containerRayCast(%pos, vectorAdd(%pos, vectorScale(%mine.getUpVector(), %this.tripBeamLength)), $TypeMasks::FxBrickObjectType | $TypeMasks::StaticObjectType);

	%beam = new StaticShape()
	{
		datablock = mine_tripBeamShape;
		position = %mine.getPosition();
		rotation = Normal2Rotation(%mine.getUpVector());
		sourceObject = %mine.sourceObject;
		sourceInv = %mine.sourceInv;
		client = %mine.client;
		timePlaced = getSimTime();
	};

	%dist = vectorDist(%mine.getPosition(), posFromRaycast(%ray));
	%beam.setScale("0.5 0.5 " @ %dist);
	%mine.trigger.setScale("0.15 0.15 " @ %dist);

	%rot = getWords(%mine.trigger.getTransform(), 3, 6);

	%mine.trigger.setTransform(vectorAdd(%mine.getPosition(), vectorScale(%mine.getUpVector(), %dist / 2)) SPC %rot);

	%mine.beam = %beam;

	%beam.hideNode("ALL");
	%beam.schedule(%data.deployTime, unHideNode, "ALL");

	%mine.beamLength = %this.tripBeamLength;
	%mine.beamloop = %mine.schedule(%data.deployTime, TripmineBeamLoop);
}

function mine_tripChargeShape::onAdd(%data, %obj) { mine_impactChargeShape::onAdd(%data, %obj); }

function mine_tripChargeShape::onRemove(%data, %obj)
{
	mine_impactChargeShape::onRemove(%data, %obj);

	if(isObject(%obj.beam))
		%obj.beam.delete();
}

function mine_tripBlastProjectile::radiusDamage(%this, %obj, %col, %distanceFactor, %pos, %damageAmt)
{
	if(%distanceFactor <= 0)
		return;
	else if(%distanceFactor > 1)
		%distanceFactor = 1;

	%isPlayer = %col.getType() & $TypeMasks::PlayerObjectType;

	%vec = vectorNormalize(%obj.getVelocity());
	%point = (%isPlayer ? %col.getHackPosition() : %col.getWorldBoxCenter());
	%end = vectorAdd(%pos, vectorScale(%vec, getWord(%obj.sourceShape.beam.getScale(), 2)));
	%near = nearestLinePointClamped(%point, %pos, %end);

	%damageAmt *= mClampF(1 - (vectorDist(%point, %near) / %this.explosion.blastRadius), 0, 1);

	if(%damageAmt && (checkLOS(%near, %col, %obj.sourceShape) || checkLOS(%pos, %col, %obj.sourceShape)))
	{
		%damageType = $DamageType::Radius;
		if(%this.RadiusDamageType)
			%damageType = %this.RadiusDamageType;

		if(%isPlayer)
			%col.damage(%obj, %pos, %damageAmt, %damageType);
		else
			%col.damage(%obj, %pos, %damageAmt / 3, %damageType);
	}
}

function StaticShape::TripmineBeamLoop(%obj) // I honestly don't know why I'm using both raycasts and triggers. Please cry about it.
{
	if(!isObject(%obj.beam))
		return;

	cancel(%obj.beamloop);

	%pos = %obj.getPosition();

	%ray = containerRayCast(%pos, vectorAdd(%pos, vectorScale(%obj.getUpVector(), %obj.beamLength)), $TypeMasks::FxBrickObjectType | $TypeMasks::StaticObjectType, %obj);

	if(%ray)
	{
		%dist = vectorDist(%obj.getPosition(), posFromRaycast(%ray));
		%obj.beam.setScale("0.5 0.5 " @ %dist);
		%obj.trigger.setScale("0.15 0.15 " @ %dist);

		%rot = getWords(%obj.trigger.getTransform(), 3, 6);

		%obj.trigger.setTransform(vectorAdd(%obj.getPosition(), vectorScale(%obj.getUpVector(), %dist / 2)) SPC %rot);
	}
	else
	{
		%obj.trigger.triggered = true;
		%obj.schedule(%obj.trigger.delay, mineExplode, %obj.trigger.explosionScale, vectorScale(%obj.getUpVector(), %obj.trigger.explosionOffset));
		serverPlay3D(%obj.trigger.sound, %trigger.getPosition());
		return;
	}

	%obj.beamloop = %obj.schedule(128, TripmineBeamLoop);
}