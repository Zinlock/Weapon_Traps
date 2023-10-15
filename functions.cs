function WeaponImage::minePlaceCheck(%this, %obj, %slot, %ray)
{
	%pos = posFromRaycast(%ray);
	%vec = normalFromRaycast(%ray);
	%ang = mFloor(mRadToDeg(mAcos(vectorDot(%vec, "0 0 1"))));
	if(%ang > %this.mineMaxSlope || %ang < %this.mineMinSlope)
	{
		%obj.client.centerPrint("<font:arial:14>\c5Surface too steep!", 1.5);
		return 0;
	}

	if(!$Pref::XMines::mineStack && !%this.allowMineStacking)
	{
		%col = containerRayCast(vectorAdd(%pos, vectorScale(%vec, 0.1)), vectorAdd(%pos, vectorScale(%vec, -0.1)), $trapStaticTypemask | $TypeMasks::FxBrickObjectType);
		if(%col && %col.isLandMine && !%col.allowMineStacking)
		{
			%obj.client.centerPrint("<font:arial:14>\c5Can't stack mines!", 1.5);
			return 0;
		}
	}

	return 1;
}

function WeaponImage::onMinePlaced(%this, %obj, %slot, %mine) { }
function WeaponImage::onMineFailed(%this, %obj, %slot, %pos) { }

function StaticShape::mineDamage(%obj, %dmg)
{
	%db = %obj.getDatablock();

	if(!%db.isLandMineData)
		return;

	%obj.health -= %dmg;

	if(%obj.health <= 0)
	{
		%obj.health = 0;

		%obj.schedule(0, mineExplode);
	}
	else if(%obj.health > %db.health)
		%obj.health = %db.health;
}

function StaticShape::mineExplode(%obj, %scale, %offset, %dir)
{
	%db = %obj.getDatablock();

	if(!%db.isLandMineData)
		return;
	
	if(%scale $= "")
	{
		if((%scale = %db.explosionScale) $= "")
			%scale = 1;
	}

	%scale = mClampF(%scale, 0.1, 5);

	%trigger = %obj.trigger;

	if(!%trigger.exploded)
	{
		%trigger.exploded = true;

		%explo = new Projectile()
		{
			dataBlock = %trigger.projectile;
			initialPosition = vectorAdd(%obj.getPosition(), %offset);
			initialVelocity = (%dir !$= "" ? vectorNormalize(%dir) : (%db.explosionForward ? %obj.getForwardVector() : vectorScale(%obj.getUpVector(), %db.explosionDown ? -1 : 1)));
			sourceObject = %trigger.sourceObject;
			sourceShape = %obj;
			client = %trigger.sourceClient;
		};

		%explo.setScale(%scale SPC %scale SPC %scale);

		if(!%db.delayCleanup)
			%obj.delete();
		
		%explo.explode();

		if(isObject(%obj))
			%obj.schedule(0, delete);
	}
}

function StaticShape::mineDropItem(%obj, %off, %vel)
{
	%db = %obj.getDatablock();
	
	if(!%db.isLandMineData)
		return;
	
	if(!%obj.trigger.exploded && !%obj.trigger.triggered)
	{
		%itm = new Item()
		{
			dataBlock = %obj.recoverItem;
			position = vectorAdd(%obj.getPosition(), vectorScale(%obj.getUpVector(), %off));
			canPickup = true;
			static = false;
			minigame = getMinigameFromObject(%obj);
			bl_id = (isObject(%cl = %obj.Client) ? %cl.getBLID() : -1);
		};

		%itm.setCollisionTimeout(%obj.client.Player);
		%itm.schedulePop();
		%itm.setVelocity(vectorScale(%obj.getUpVector(), %vel));
		%itm.weaponCharges = (%obj.recoverCharges !$= "" ? %obj.recoverCharges : 1);

		%obj.delete();
	}
}

function StaticShape::mineDetachLoop(%obj)
{
	%db = %obj.getDatablock();
	
	if(!%db.isLandMineData)
		return;

	if(%obj.trigger.exploded || %obj.trigger.triggered)
		return;

	cancel(%obj.detachLoop);

	%pos = %obj.getPosition();
	%vec = %obj.getUpVector();
	%ray = containerRayCast(vectorAdd(%pos, vectorScale(%vec, 0.1)), vectorAdd(%pos, vectorScale(%vec, -0.1)), $TypeMasks::FxBrickObjectType | $trapStaticTypemask, %obj);
	if(%ray != %obj && !isObject(%ray) && !$Pref::XMines::mineFloat)
	{
		if(%db.explodeOnDetach)
			return %obj.schedule(0, mineExplode);
		else if(%db.dropOnDetach)
			return %obj.schedule(0, mineDropItem, 0.4, 1.5);
	}

	%obj.detachLoop = %obj.schedule(128, mineDetachLoop);
}

function mineCanTrigger(%src, %col)
{
	if(!isObject(%src)) return 1;
	if(!isObject(%col)) return 0;

	if(isObject(%src.client))
		%src = %src.client;

	if(%col.getDataBlock().isTurretArmor)
		return 0;

	if(%col.getType() & $TypeMasks::VehicleObjectType)
	{
		if(isObject(%con = %col.getControllingObject()))
			%col = %con;
		else return minigameCanDamage(%src, %col) == 1;
	}

	if(%src.role $= "Traitor" && %col.client.role $= "Traitor")
		return 0;

	if((%mini = getMinigameFromObject(%src)) == getMinigameFromObject(%col) && %src != %col.client && %src != %col)
	{
		if(%mini.isSlayerMinigame)
		{
			%srcTeam = (isObject(%src) ? %src.getTeam() : 0);
			if(%col.IsA("GameConnection"))
				%colTeam = %col.getTeam();
			else
				%colTeam = (isObject(%col.client) ? %col.client.getTeam() : 0);

			if(%srcTeam == 0 || %colTeam == 0 || %srcTeam != %colTeam && !%srcTeam.isAlliedTeam(%colTeam))
				return minigameCanDamage(%src, %col) == 1;
		}
		else return minigameCanDamage(%src, %col) == 1;
	}
	
	return 0;
}

function checkLOS(%pos, %col, %ex0, %ex1, %ex2, %ex3)
{
	//%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $trapStaticTypemask;
	%mask = $TypeMasks::FxBrickObjectType | $trapStaticTypemask;
	checkLOSMask(%pos, %col, %mask, %ex0, %ex1, %ex2, %ex3);
}

function checkLOSMask(%pos, %col, %mask, %ex0, %ex1, %ex2, %ex3)
{
	if(isObject(%col))
	{
		%type = %col.getType();

		if(%type & $TypeMasks::PlayerObjectType)
		{
			%ray = containerRayCast(%pos, %col.getEyePoint(), %mask, %ex0, %ex1, %ex2, %ex3);
			if(!isObject(%ray) || %ray == %col)
				return 1;
			else
			{
				%ray = containerRayCast(%pos, %col.getHackPosition(), %mask, %ex0, %ex1, %ex2, %ex3);
				if(!isObject(%ray) || %ray == %col)
					return 1;
				else
				{
					%ray = containerRayCast(%pos, %col.getPosition(), %mask, %ex0, %ex1, %ex2, %ex3);
					if(!isObject(%ray) || %ray == %col)
						return 1;
				}
			}
		}
		else if(%type & $TypeMasks::VehicleObjectType | $trapStaticTypemask | $TypeMasks::FxBrickObjectType)
		{
			%ray = containerRayCast(%pos, %col.getWorldBoxCenter(), %mask, %ex0, %ex1, %ex2, %ex3);
			if(!isObject(%ray) || %ray == %col)
				return 1;
		}
		else return 1;
	}

	return 0;
}

function nearestLinePoint(%src, %from, %to)
{
	%src = vectorScale(%src, 1); // make sure these are all proper vectors
	%from = vectorScale(%from, 1);
	%to = vectorScale(%to, 1);

	if(vectorDist(%from, %to) == 0)
		return %src;

	%dir = vectorScale(vectorSub(%to, %from), 1 / vectorDist(%from, %to));
	%vec = vectorSub(%src, %from);
	%dot = vectorDot(%vec, %dir);
	%pos = vectorAdd(%from, vectorScale(%dir, %dot));

	return %pos;
}

function nearestLinePointClamped(%src, %from, %to)
{
	%src = vectorScale(%src, 1);
	%from = vectorScale(%from, 1);
	%to = vectorScale(%to, 1);

	if(vectorDist(%from, %to) == 0)
		return %from;
	
	%pos = nearestLinePoint(%src, %from, %to);
	
	%fpd = vectorDist(%from, %pos);
	%tpd = vectorDist(%to, %pos);
	%ftd = vectorDist(%from, %to);

	if(%fpd + %tpd > %ftd)
	{
		if(%fpd > %tpd)
			return %to;
		else if(%tpd > %fpd)
			return %from;
	}

	return %pos;
}

function vectorToMatrix(%vector)
{
	%y = mACos(getWord(%vector, 2) / vectorLen(%vector));
	%z = mATan(getWord(%vector, 1), getWord(%vector, 0));
	return MatrixCreateFromEuler("0 " @ %y SPC %z);
}