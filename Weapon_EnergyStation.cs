exec("./Weapon_EnergyStation_Particles.cs");

datablock StaticShapeData(trap_energyChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/EnergyStation.dts";
	energy = 200;
	
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
	triggerProjectile = trap_energyDestroyedProjectile;
	triggerDistance = 8;
	triggerAngle = 0;
	triggerType = customTickTrigger64;
	triggerSize = "10 10 10";
	triggerMask = $TypeMasks::PlayerObjectType;
	
	deployTime = 2500;

	enterCallback = "trap_energyEnter";
	tickCallback = "trap_energyTick";
	leaveCallback = "trap_energyExit";
};

datablock DebrisData(trap_energyShieldRDebris : mine_scrapCogDebris) { shapeFile = "./dts/debrisenergyShieldR.dts"; };
datablock ExplosionData(trap_energyShieldRExplosion : mine_scrapCogExplosion) { debris = trap_energyShieldRDebris; debrisNum = 1; debrisNumVariance = 0; };

datablock DebrisData(trap_energyShieldLDebris : trap_energyShieldRDebris) { shapeFile = "./dts/debrisenergyShieldL.dts"; };
datablock ExplosionData(trap_energyShieldLExplosion : trap_energyShieldRExplosion) { debris = trap_energyShieldLDebris; subExplosion[0] = trap_energyShieldRExplosion; };

datablock DebrisData(trap_energyHandleDebris : trap_energyShieldRDebris) { shapeFile = "./dts/debrisenergyHandle.dts"; };
datablock ExplosionData(trap_energyHandleExplosion : trap_energyShieldRExplosion) { debris = trap_energyHandleDebris; subExplosion[0] = trap_energyShieldLExplosion; };

datablock DebrisData(trap_energyBodyDebris : trap_energyShieldRDebris) { shapeFile = "./dts/debrisenergyBody.dts"; };
datablock ExplosionData(trap_energyBodyExplosion : trap_energyShieldRExplosion) { debris = trap_energyBodyDebris; subExplosion[0] = trap_energyHandleExplosion; };

datablock ExplosionData(trap_energyExplosion)
{
	lifeTimeMS = 150;

	subExplosion[0] = mine_scrapExplosion;
	subExplosion[1] = trap_energyBodyExplosion;

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

datablock ProjectileData(trap_energyDestroyedProjectile)
{
	directDamageType  = $DamageType::mineStationDirect;
	radiusDamageType  = $DamageType::mineStationDirect;
	explosion           = trap_energyExplosion;

	explodeOnDeath        = false;  

	armingDelay         = 5000;
	lifetime            = 5000;
	fadeDelay           = 5000;
	bounceElasticity    = 0.0;
	bounceFriction  	  = 1.0;
};

datablock ItemData(trap_energyItem : mine_impactItem)
{
	shapeFile = "./dts/EnergyStation.dts";

	uiName = "[T] Energy Station";
	iconName = "./ico/ENERGY";

	doColorShift = true;
	colorShiftColor = "0.1 0.1 0.1 1.0";

	image = trap_energyImage;
};

datablock ShapeBaseImageData(trap_energyImage : mine_impactImage)
{
	shapeFile = "./dts/EnergyStationImage.dts";

	offset = "0 0 0";
	rotation = eulerToMatrix("0 0 0");

	item = trap_energyItem;

	mineShapeData = trap_energyChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 60;
	mineDeploySound = mine_deployHealthSound;
	mineDeployDistance = 5;

	doColorShift = trap_energyItem.doColorShift;
	colorShiftColor = trap_energyItem.colorShiftColor;

	defaultColor = "0 0.3792 0.8 1";

	weaponUseCount = 1;
	weaponReserveMax = 1;

	tickenergy = 1;
	timeout = 0;
	energyCharge = 0;
	energyStack = 0;
	energyTeam = 0;
	energyTurrets = 1;
};

function trap_energyImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function trap_energyImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function trap_energyImage::onChargeStart(%this, %obj, %slot) { }

function trap_energyImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function trap_energyImage::onMinePlaced(%this, %obj, %slot, %mine)
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
	
	if(%this.energyCharge > 0)
		%mine.setShapeName(mCeil(%this.energyCharge) @ " hp");
	
	%mine.setShapeNameColor("0.1 1 0.1");
	%mine.setShapeNameDistance(12);

	%mine.playThread(0, deploy);
	%mine.schedule(2500, playThread, 0, openRoot);
	
	%node = new ParticleEmitterNode()
	{
		datablock = GenericEmitterNode;
		emitter = trap_energyFXAEmitter;
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
		emitter = trap_energyFXBEmitter;
		scale = "0 0 0";
	};
	%node.setTransform("0 0 -12");
	%node.setColor(%color);
	%node.schedule(2499, setTransform, vectorAdd(%mine.getPosition(), vectorScale(%mine.getUpVector(), 0.65)));
	%node.schedule(2500, inspectPostApply);
	%mine.fxNodeB = %node;
}

function trap_energyChargeShape::onAdd(%data, %obj)
{
	%obj.energyCharge = trap_energyImage.energyCharge;

	if(trap_energyImage.timeout > 0)
		%obj.schedule(trap_energyImage.timeout * 1000, energyStationDestroy);
	
	mine_impactChargeShape::onAdd(%data, %obj);
}

function trap_energyChargeShape::onRemove(%data, %obj)
{
	if(isObject(%node = %obj.fxNodeA))
		%node.delete();

	if(isObject(%node = %obj.fxNodeB))
		%node.delete();
	
	mine_impactChargeShape::onRemove(%data, %obj);
}

function StaticShape::energyStationDestroy(%obj)
{
	%obj.dead = true;
	%obj.energyCharge = 0;
	%obj.playThread(0, close);
	serverPlay3D(mine_steamHealthSound, %obj.getWorldBoxCenter());
	%obj.fxNodeA.delete();
	%obj.fxNodeB.delete();
	%obj.schedule(1000, mineExplode);
}

function trap_energyTick(%trigger)
{
	for(%i = 0; %i < %trigger.getNumObjects(); %i++)
	{
		%hit = %trigger.getObject(%i);

		trap_energyEnter(%trigger, %hit);
	}
}

function trap_energyEnter(%trigger, %hit)
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

	if(minigameCanDamage(%trigger.sourceClient, %hit) != -1 && %hit.getDamagePercent() < 1.0 && (trap_energyImage.energyTurrets || !%hit.getDataBlock().isTurretArmor))
	{
		if(trap_energyImage.energyTeam || !mineCanTrigger(%trigger.sourceClient, %hit))
		{
			%maxErg = %hit.getDataBlock().maxEnergy;
			if(%hit.getEnergyLevel() < %maxErg && (%hit.energyTrigger == %trigger || !isObject(%hit.energyTrigger) || trap_energyImage.energyStack))
			{
				if(%hit.energyTrigger != %trigger)
				{
					if(!isObject(%hit.energyTrigger))
						%hit.energyTrigger = %trigger;
					
					if(%hit.energyTrigger == %trigger)
						serverPlay3D(mine_triggerHealthSound, %hit.getHackPosition());
				}

				%hit.setWhiteOut(getMax(0.1, %hit.getWhiteOut()));
				%energy = trap_energyImage.tickenergy / 2;
			
				if(trap_energyImage.energyCharge > 0 && %trigger.sourceShape.energyCharge > 0)
				{
					%erg = %hit.getEnergyLevel();

					if(%energy > %erg)
						%energy = %erg;

					%hit.setEnergyLevel(%hit.getEnergyLevel() + %energy);
					%trigger.sourceShape.energyCharge -= %energy;

					if(%trigger.sourceShape.energyCharge <= 0)
						%trigger.sourceShape.energyStationDestroy();

					%trigger.sourceShape.setShapeName(mCeil(%trigger.sourceShape.energyCharge) @ " erg");
				}
				else
					%hit.setEnergyLevel(%hit.getEnergyLevel() + trap_energyImage.tickenergy / 2);
				
				%hit.lasterg = %hit.getEnergyLevel();
			}
		}
	}
}

function trap_energyExit(%trigger, %hit)
{
	if(%hit.energyTrigger == %trigger)
		%hit.energyTrigger = "";
}