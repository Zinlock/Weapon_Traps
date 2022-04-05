package TrapWeaponPackage
{
	function ProjectileData::onCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity)
	{
		if(isObject(%col) && %col.IsA("StaticShape") && %col.isLandMine && (%db = %col.getDatablock()).health > 0 && (mineCanTrigger(%obj) || %obj.client == %col.client || minigameCanDamage(%obj.client, %col) == 1))
		{
			%dmg = %this.directDamage * %db.directMult;
			%col.mineDamage(%dmg);
		}

		return Parent::onCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity);
	}

	function ProjectileData::onExplode(%this, %obj, %pos)
	{
		initContainerRadiusSearch(%pos, %this.explosion.damageRadius, $TypeMasks::StaticObjectType);
		while(isObject(%col = ContainerSearchNext()))
		{
			if(%col.isLandMine && (%db = %col.getDatablock()).health > 0 && checkLOS(%pos, %col, %obj.sourceShape) && (mineCanTrigger(%obj) || %obj.client == %col.client || minigameCanDamage(%obj.client, %col) == 1))
			{
				%dmg = %this.explosion.radiusDamage * (1 - vectorDist(%pos, %col.getPosition()) / %this.explosion.damageRadius) * %db.radiusMult;
				%col.schedule(getRandom(300), mineDamage, %dmg);

				if(%dmg > %col.health)
					%col.trigger.triggered = true;
			}
		}
		
		return Parent::onExplode(%this, %obj, %pos);
	}

	function GameConnection::onDrop(%cl, %msg)
	{
		if(isObject(%cl.trapSet))
		{
			%cl.trapSet.deleteAll();
			%cl.trapSet.schedule(0, delete);
		}

		Parent::onDrop(%cl, %msg);
	}
	
	function Player::activateStuff(%pl)
	{
		%ray = containerRayCast(%pl.getEyePoint(), vectorAdd(%pl.getEyePoint(), vectorScale(%pl.getEyeVector(), 5)), $TypeMasks::StaticObjectType | $TypeMasks::FxBrickObjectType, %pl);

		if(%ray && %ray.isLandMine && %ray.client == %pl.client && %ray.canRecover && getSimTime() - %ray.trigger.creationTime > 500)
			%ray.mineDropItem(0.4, 3);

	 	Parent::activateStuff(%pl);
	}
	
	function MinigameSO::addMember(%mini,%client)
	{
		Parent::addMember(%mini,%client);

		if(isObject(%client.trapSet))
			%client.trapSet.deleteAll();
	}

	function MinigameSO::removeMember(%mini,%client)
	{
		Parent::removeMember(%mini,%client);

		if(isObject(%client.trapSet))
			%client.trapSet.deleteAll();
	}
	
	function MinigameSO::reset(%mini,%client)
	{
		Parent::reset(%mini,%client);

		for(%i = 0; %i < %mini.numMembers; %i++)
		{
			%cl = %mini.member[%i];
			if(isObject(%cl.trapSet))
				%cl.trapSet.deleteAll();
		}
	}
};
activatePackage(TrapWeaponPackage);