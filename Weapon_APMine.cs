datablock StaticShapeData(mine_impactChargeShape)
{
	dynamicType = $TypeMasks::TerrainObjectType; // i forgot to let aebase guns hit static shapes so.  Oops

	shapeFile = "./dts/mineAntiPersonnel.dts";
	isLandMineData = true;

	deployTime = 1000; // time before the trigger activates
	
	health = 60;
	directMult = 1;
	radiusMult = 1;
	explodeOnDeath = true;
	explodeOnDetach = false; // blow up...
	dropOnDetach = true; // ...or drop the mine when it is no longer stuck to a surface

	explosionScale = 1;
	explosionForward = false;
	explosionOffset = 0;

	triggerDelay = 150; // time after triggering before blowing up
	triggerSound = mine_triggerSound; // sound to play when triggered
	triggerProjectile = mine_impactBlastProjectile; // projectile to spawn for the explosion
	triggerDistance = 0; // max distance to trigger (<=0 to use the entire trigger)
	triggerAngle = 0; // max angle to trigger from forward
	triggerType = genericTrigger128; // trigger datablock to use
	triggerPolyhedron = "-0.5 -0.5 -0.5 1 0 0 0 1 0 0 0 1"; // s.e.
	triggerSize = "0.8 0.8 0.45"; // s.e.
	triggerMask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType; // s.e.

	// callbacks only work for generic triggers
	enterCallback = "mine_impactCollision"; // call when an object enters (%trigger, %obj)
	tickCallback = "mine_impactTick"; // call every tick while objects are inside (%trigger)
	leaveCallback = ""; // call when an object leaves (%trigger, %obj)
};

datablock AudioProfile(mine_deploySound)
{
	filename    = "./wav/deploy_mine.wav";
	description = AudioShort3D;
	preload = true;

	pitchRange = 4;
};

datablock AudioProfile(mine_triggerSound)
{
	filename    = "./wav/trigger_mine.wav";
	description = AudioDefault3D;
	preload = true;

	pitchRange = 4;
};

datablock AudioProfile(mine_blastSound)
{
	filename    = "./wav/blast_mine.wav";
	description = AudioDefault3D;
	preload = true;

	pitchRange = 4;
};

datablock ExplosionData(mine_impactExplosion : grenade_remoteChargeExplosion)
{
	soundProfile = mine_blastSound;

	impulseRadius = 13;
	impulseForce = 1000;

	damageRadius = 10;
	radiusDamage = 120;
};

datablock ProjectileData(mine_impactBlastProjectile)
{
	directDamageType  = $DamageType::mineImpactDirect;
	radiusDamageType  = $DamageType::mineImpactDirect;
	explosion           = mine_impactExplosion;

	explodeOnDeath        = false;  

	armingDelay         = 5000;
	lifetime            = 5000;
	fadeDelay           = 5000;
	bounceElasticity    = 0.0;
	bounceFriction  	  = 1.0;
};

datablock ItemData(mine_impactItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./dts/mineAntiPersonnel.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "[T] Blast Mine";
	iconName = "./ico/IMPACT";
	doColorShift = true;
	colorShiftColor = "1.0 0.956 0.658 1.0";

	image = mine_impactImage;
	canDrop = true;
};

datablock ShapeBaseImageData(mine_impactImage)
{
	shapeFile = "./dts/mineAntiPersonnelImage.dts";
	emap = true;

	item = mine_impactItem;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = eulerToMatrix( "0 0 0" );
	className = "WeaponImage";
	armReady = false;

	mineShapeData = mine_impactChargeShape;
	mineCanRecover = true;

	mineMinSlope = 0;
	mineMaxSlope = 60;
	mineDeploySound = mine_deploySound;
	mineDeployDistance = 5;

	doColorShift = mine_impactItem.doColorShift;
	colorShiftColor = mine_impactItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;

	stateName[0]                     = "Ready";
	stateScript[0]								= "onReady";
	stateSequence[0]			 = "root";
	stateTransitionOnTriggerDown[0]  = "Fire";

	stateName[2]                     = "Fire";
	stateTransitionOnTimeout[2]      = "Next";
	stateScript[2]                   = "onFire";
	stateTimeoutValue[2]		   = 0.2;

	stateName[3]                     = "Next";
	stateTimeoutValue[3]		   = 0.2;
	stateTransitionOnTimeout[3]      = "Ready";
	stateWaitForTimeout[3] = true;
};

function mine_impactImage::onReady(%this, %obj, %slot)
{
	%obj.playThread(1, root);
	%obj.weaponAmmoStart();
	
	if(isObject(%trapSet = %obj.client.trapSet))
	{
		if(%trapSet.getCount() >= $Pref::XMines::trapLimit && $Pref::XMines::trapLimit > 0)
			%obj.client.centerPrint("<font:arial:14>\c5 You already have " @ %trapSet.getCount() @ " active traps!<br>\c5Placing new ones will automatically discard old ones.", 3);
	}
}

function mine_impactImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_impactImage::onChargeStart(%this, %obj, %slot) { }

function mine_impactImage::onFire(%this, %obj, %slot)
{
	%eye = %obj.getEyePoint();
	%vec = %obj.getEyeVector();
	%end = vectorAdd(%eye, vectorScale(%vec, %this.mineDeployDistance));

	%ray = containerRayCast(%eye, %end, $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::StaticObjectType, %obj);

	if(isObject(%ray) && %ray.getType() & ($TypeMasks::FxBrickObjectType | $TypeMasks::StaticObjectType))
	{
		if(%this.minePlaceCheck(%obj, %slot, %ray))
		{
			%obj.weaponAmmoUse();
			
			%charge = new StaticShape()
			{
				datablock = %this.mineShapeData;
				position = posFromRaycast(%ray);
				rotation = Normal2Rotation(normalFromRaycast(%ray));
				sourceObject = %obj;
				sourceInv = %obj.currTool;
				minigame = %obj.client.minigame;
				client = %obj.client;
				timePlaced = getSimTime();

				isLandMine = true;
				canRecover = %this.mineCanRecover;
				recoverItem = %this.item;
				recoverCharges = 1;
			};

			%charge.setNodeColor("ALL", %this.item.colorShiftColor);

			%trapSet = %obj.client.trapSet;
			if(!isObject(%trapSet))
				%trapSet = new SimSet();
			
			if(!isObject(globalChargeSet))
				new SimSet(globalChargeSet);
			
			%trapSet.add(%charge);
			globalChargeSet.add(%charge);
			MissionCleanup.add(%charge);

			serverPlay3D(%this.mineDeploySound, %charge.getPosition());

			%obj.client.trapSet = %trapSet;

			if(%trapSet.getCount() > $Pref::XMines::trapLimit && $Pref::XMines::trapLimit > 0)
			{
				%lowest = -1;
				for(%i = 0; %i < %trapSet.getCount(); %i++)
				{
					%mine = %trapSet.getObject(%i);

					if(!isObject(%lowest) || %mine.timePlaced < %lowest.timePlaced)
						%lowest = %mine;
				}

				if(isObject(%lowest))
					%lowest.mineDropItem(0.4, 3);//delete();
			}

			%this.onMinePlaced(%obj, %slot, %charge);
		}
		else %this.onMineFailed(%obj, %slot, %end);
	}
}

function mine_impactChargeShape::onAdd(%data, %obj)
{
	%obj.health = %data.health;

	%trig = new Trigger()
	{
		datablock = %data.triggerType;
		position = %obj.getPosition();
		rotation = %obj.rotation;
		polyhedron = %data.triggerPolyhedron;
		creationTime = getSimTime();
		sourceObject = %obj.sourceObject;
		sourceClient = %obj.client;
		sourceShape = %obj;

		deployTime = %data.deployTime;
		delay = %data.triggerDelay;
		sound = %data.triggerSound;
		maxDistance = %data.triggerDistance;
		projectile = %data.triggerProjectile;
		explosionScale = %data.explosionScale;
		explosionOffset = %data.explosionOffset;
		explosionForward = %data.explosionForward;
		maxAngle = %data.triggerAngle;
		mask = %data.triggerMask;

		enterCallback = %data.enterCallback;
		tickCallback = %data.tickCallback;
		leaveCallback = %data.leaveCallback;
	};

	if($trapTriggerDebug)
		%trig.scopeToClient(%obj.client);

	missionCleanup.add(%trig);
	%trig.setScale(%data.triggerSize);

	%obj.trigger = %trig;

	if(%data.dropOnDetach || %data.explodeOnDetach)
		%obj.schedule(0, mineDetachLoop);
}

function mine_impactChargeShape::onRemove(%data, %obj)
{
	if(isObject(%obj.trigger))
		%obj.trigger.delete();
}

function mine_impactTick(%trigger)
{
	for(%i = 0; %i < %trigger.getNumObjects(); %i++)
	{
		%hit = %trigger.getObject(%i);

		mine_impactCollision(%trigger, %hit);
	}
}

function mine_impactCollision(%trigger, %hit)
{
	if(!(%hit.getType() & %trigger.mask))
		return;

	if(getSimTime() - %trigger.creationTime < %trigger.deployTime)
		return;

	%pos = ((%hit.getType() & $TypeMasks::PlayerObjectType) ? %hit.getHackPosition() : %hit.getWorldBoxCenter());

	if(%trigger.maxDistance > 0)
	{
		if(vectorDist(%trigger.getPosition(), %pos) > %trigger.maxDistance)
			return;
	}
	
	if(!checkLOS(%trigger.sourceShape.getWorldBoxCenter(), %hit, %trigger.sourceShape))
		return;

	if(%trigger.maxAngle > 0)
	{
		if(mFloor(mRadToDeg(mAcos(vectorDot(vectorNormalize(vectorSub(%pos, %trigger.getPosition())), %trigger.sourceShape.getForwardVector())))) > %trigger.maxAngle)
			return;
	}

	if(!%trigger.triggered && mineCanTrigger(%trigger.sourceClient, %hit))
	{
		%trigger.triggered = true;
		%trigger.sourceShape.schedule(%trigger.delay, mineExplode, %trigger.explosionScale, vectorScale(%trigger.sourceShape.getUpVector(), %trigger.explosionOffset));
		serverPlay3D(%trigger.sound, %trigger.getPosition());
		return;
	}
}

function mine_impactBlastProjectile::radiusDamage(%this, %obj, %col, %distanceFactor, %pos, %damageAmt)
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

		if(%col.getType() & ($TypeMasks::PlayerObjectType))
			%col.damage(%obj, %pos, %damageAmt, %damageType);
		else
			%col.damage(%obj, %pos, %damageAmt / 3, %damageType);
	}
}