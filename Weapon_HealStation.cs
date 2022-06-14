exec("./Weapon_HealStation_Particles.cs");

datablock StaticShapeData(trap_healthChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/healStation.dts";
	health = 200;
	
	directMult = 1;
	radiusMult = 1.4;
	explodeOnDeath = true;
	explodeOnDetach = false;
	dropOnDetach = true;

	explosionScale = 1;
	explosionForward = false;
	explosionDown = false;
	explosionOffset = 0;

	triggerDelay = 0;
	triggerSound = "";
	triggerProjectile = trap_healthDestroyedProjectile;
	triggerDistance = 8;
	triggerAngle = 0;
	triggerType = customTickTrigger64;
	triggerSize = "10 10 10";
	triggerMask = $TypeMasks::PlayerObjectType;
	
	deployTime = 2500;

	enterCallback = "trap_healthEnter";
	tickCallback = "trap_healthTick";
	leaveCallback = "trap_healthExit";
};

datablock AudioProfile(mine_destroyHealthSound)
{
	filename    = "./wav/destroy_health.wav";
	description = AudioClose3D;
	preload = true;
};

datablock DebrisData(trap_healthShieldRDebris : mine_scrapCogDebris) { shapeFile = "./dts/debrisHealShieldR.dts"; };
datablock ExplosionData(trap_healthShieldRExplosion : mine_scrapCogExplosion) { debris = trap_healthShieldRDebris; debrisNum = 1; debrisNumVariance = 0; };

datablock DebrisData(trap_healthShieldLDebris : trap_healthShieldRDebris) { shapeFile = "./dts/debrisHealShieldL.dts"; };
datablock ExplosionData(trap_healthShieldLExplosion : trap_healthShieldRExplosion) { debris = trap_healthShieldLDebris; subExplosion[0] = trap_healthShieldRExplosion; };

datablock DebrisData(trap_healthShieldFDebris : trap_healthShieldRDebris) { shapeFile = "./dts/debrisHealShieldF.dts"; };
datablock ExplosionData(trap_healthShieldFExplosion : trap_healthShieldRExplosion) { debris = trap_healthShieldFDebris; subExplosion[0] = trap_healthShieldLExplosion; };

datablock DebrisData(trap_healthHandleDebris : trap_healthShieldRDebris) { shapeFile = "./dts/debrisHealHandle.dts"; };
datablock ExplosionData(trap_healthHandleExplosion : trap_healthShieldRExplosion) { debris = trap_healthHandleDebris; subExplosion[0] = trap_healthShieldFExplosion; };

datablock DebrisData(trap_healthBodyDebris : trap_healthShieldRDebris) { shapeFile = "./dts/debrisHealBody.dts"; };
datablock ExplosionData(trap_healthBodyExplosion : trap_healthShieldRExplosion) { debris = trap_healthBodyDebris; subExplosion[0] = trap_healthHandleExplosion; };

datablock ExplosionData(trap_healthExplosion)
{
	lifeTimeMS = 150;

	subExplosion[0] = mine_scrapExplosion;
	subExplosion[1] = trap_healthBodyExplosion;

	soundProfile = mine_destroyHealthSound;
	explosionScale = "1 1 1";

	particleEmitter = "";
	particleDensity = 100;
	particleRadius = 0.2;

	impulseRadius = 0;
	impulseForce = 0;

	damageRadius = 0;
	radiusDamage = 0;
};

datablock ProjectileData(trap_healthDestroyedProjectile)
{
	directDamageType  = $DamageType::mineStationDirect;
	radiusDamageType  = $DamageType::mineStationDirect;
	explosion           = trap_healthExplosion;

	explodeOnDeath        = false;  

	armingDelay         = 5000;
	lifetime            = 5000;
	fadeDelay           = 5000;
	bounceElasticity    = 0.0;
	bounceFriction  	  = 1.0;
};

datablock AudioProfile(mine_deployStationSound)
{
	filename    = "./wav/deploy_station.wav";
	description = AudioShort3D;
	preload = true;
};

datablock AudioProfile(mine_steamHealthSound)
{
	filename    = "./wav/health_steam.wav";
	description = AudioShort3D;
	preload = true;
};

datablock AudioProfile(mine_deployHealthSound)
{
	filename    = "./wav/deploy_health.wav";
	description = AudioShort3D;
	preload = true;
};

datablock AudioProfile(mine_triggerHealthSound)
{
	filename    = "./wav/trigger_health.wav";
	description = AudioShort3D;
	preload = true;
};

datablock ItemData(trap_healthItem : mine_impactItem)
{
	shapeFile = "./dts/healStationImage.dts";

	uiName = "[T] Health Station";
	iconName = "./ico/HEALSTATION";

	doColorShift = true;
	colorShiftColor = "0.1 0.1 0.1 1.0";

	image = trap_healthImage;
};

datablock ShapeBaseImageData(trap_healthImage : mine_impactImage)
{
	shapeFile = "./dts/healStationImage.dts";

	offset = "0 1.1 0.08";
	rotation = eulerToMatrix("90 0 0");

	item = trap_healthItem;

	mineShapeData = trap_healthChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 60;
	mineDeploySound = mine_deployHealthSound;
	mineDeployDistance = 5;

	doColorShift = trap_healthItem.doColorShift;
	colorShiftColor = trap_healthItem.colorShiftColor;

	defaultColor = "0 0.3792 0.8 1";

	weaponUseCount = 1;
	weaponReserveMax = 1;

	tickHeal = 1.5;
	timeout = 0;
	healthCharge = 0;
	healthStack = 0;
	healthTeam = 0;
};

function trap_healthImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function trap_healthImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function trap_healthImage::onChargeStart(%this, %obj, %slot) { }

function trap_healthImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function trap_healthImage::onMinePlaced(%this, %obj, %slot, %mine)
{
	%mat = %obj.getTransform();

	%matrix = vectorToMatrix(%mine.getUpVector());

	if(%mine.getUpVector() $= "0 0 1")
		%matrix = MatrixMultiply("0 0 0 " @ %mine.getUpVector() SPC (getWord(%mat, 6) * getWord(%mat, 5)), %matrix);
	else
		%matrix = MatrixMultiply("0 0 0 " @ %mine.getUpVector() SPC (getWord(%mat, 6) * getWord(%mat, 5) + $pi/2), %matrix); // this gets less accurate as the slope angle increases

	%matrix = MatrixMultiply(%mine.getPosition(), %matrix);

	%mine.setTransform(%matrix);

	%color = %this.defaultColor;

	if(isObject(%team = %obj.Client.slyrTeam))
		%color = getWords(%team.colorRGB, 0, 2) SPC "1";

	%mine.setNodeColor("ALL", %color);
	
	if(%this.healthCharge > 0)
		%mine.setShapeName(mCeil(%this.healthCharge) @ " hp");
	
	%mine.setShapeNameColor("0.1 1 0.1");
	%mine.setShapeNameDistance(12);

	%mine.playThread(0, deploy);
	%mine.schedule(2500, playThread, 0, openRoot);
	
	%node = new ParticleEmitterNode()
	{
		datablock = GenericEmitterNode;
		emitter = trap_healthFXAEmitter;
		scale = "0 0 0";
	};
	%node.setTransform("0 0 -12");
	%node.setColor(%color);
	%node.schedule(2499, setTransform, vectorAdd(%mine.getPosition(), vectorScale(%mine.getUpVector(), 0.65)));
	%node.schedule(2500, inspectPostApply);
	%mine.fxNodeA = %node;
	
	%node = new ParticleEmitterNode()
	{
		datablock = GenericEmitterNode;
		emitter = trap_healthFXBEmitter;
		scale = "0 0 0";
	};
	%node.setTransform("0 0 -12");
	%node.setColor(%color);
	%node.schedule(2499, setTransform, vectorAdd(%mine.getPosition(), vectorScale(%mine.getUpVector(), 0.65)));
	%node.schedule(2500, inspectPostApply);
	%mine.fxNodeB = %node;
}

function trap_healthChargeShape::onAdd(%data, %obj)
{
	%obj.healthCharge = trap_healthImage.healthCharge;

	if(trap_healthImage.timeout > 0)
		%obj.schedule(trap_healthImage.timeout * 1000, healthStationDestroy);
	
	mine_impactChargeShape::onAdd(%data, %obj);
}

function trap_healthChargeShape::onRemove(%data, %obj)
{
	if(isObject(%node = %obj.fxNodeA))
		%node.delete();

	if(isObject(%node = %obj.fxNodeB))
		%node.delete();
	
	mine_impactChargeShape::onRemove(%data, %obj);
}

function StaticShape::healthStationDestroy(%obj)
{
	%obj.dead = true;
	%obj.healthCharge = 0;
	%obj.playThread(0, close);
	serverPlay3D(mine_steamHealthSound, %obj.getWorldBoxCenter());
	%obj.fxNodeA.delete();
	%obj.fxNodeB.delete();
	%obj.schedule(1000, mineExplode);
}

function trap_healthTick(%trigger)
{
	for(%i = 0; %i < %trigger.getNumObjects(); %i++)
	{
		%hit = %trigger.getObject(%i);

		trap_healthEnter(%trigger, %hit);
	}
}

function trap_healthEnter(%trigger, %hit)
{
	if(%trigger.sourceShape.dead)
		return;

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

	if(minigameCanDamage(%trigger.sourceClient, %hit) != -1)
	{
		if(trap_healthImage.healthTeam || !mineCanTrigger(%trigger.sourceClient, %hit))
		{
			if(%hit.getDamageLevel() > 0 && (%hit.healthTrigger == %trigger || !isObject(%hit.healthTrigger) || trap_healthImage.healthStack))
			{
				if(!isObject(%hit.mollyTarget) && !isObject(%hit.dynamiteTarget))
				{
					if(%hit.healthTrigger != %trigger || (%hit.getDamageLevel() > %hit.lastdmg && !isEventPending(%hit.afterburn)))
					{
						if(!isObject(%hit.healthTrigger))
							%hit.healthTrigger = %trigger;
						
						if(%hit.healthTrigger == %trigger)
							serverPlay3D(mine_triggerHealthSound, %hit.getHackPosition());
					}

					%hit.setWhiteOut(getMax(0.1, %hit.getWhiteOut()));
					%heal = trap_healthImage.tickHeal / 2;
				
					if(trap_healthImage.healthCharge > 0 && %trigger.sourceShape.healthCharge > 0)
					{
						%dmg = %hit.getDamageLevel();

						if(%heal > %dmg)
							%heal = %dmg;

						%hit.setDamageLevel(%hit.getDamageLevel() - %heal);
						%trigger.sourceShape.healthCharge -= %heal;

						if(%trigger.sourceShape.healthCharge <= 0)
							%trigger.sourceShape.healthStationDestroy();

						%trigger.sourceShape.setShapeName(mCeil(%trigger.sourceShape.healthCharge) @ " hp");
					}
					else
						%hit.setDamageLevel(%hit.getDamageLevel() - trap_healthImage.tickHeal / 2);
					
					%hit.lastdmg = %hit.getDamageLevel();
				}
			}
		}
	}
}

function trap_healthExit(%trigger, %hit)
{
	if(%hit.healthTrigger == %trigger)
		%hit.healthTrigger = "";
}