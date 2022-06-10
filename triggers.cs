datablock TriggerData(genericTrigger128) { tickPeriodMS = 128; };
datablock TriggerData(genericTrigger64) { tickPeriodMS = 64; };

function genericTrigger128::onEnterTrigger(%db, %trig, %obj)
{
	Parent::onEnterTrigger(%db, %trig, %obj);

	if(isFunction(%trig.enterCallback))
		call(%trig.enterCallback, %trig, %obj);
}

function genericTrigger128::onLeaveTrigger(%db, %trig, %obj)
{
	Parent::onLeaveTrigger(%db, %trig, %obj);
	
	if(isFunction(%trig.leaveCallback))
		call(%trig.leaveCallback, %trig, %obj);
}

function genericTrigger128::onTickTrigger(%db, %trig)
{
	Parent::onTickTrigger(%db, %trig);

	if(isFunction(%trig.tickCallback))
		call(%trig.tickCallback, %trig);
}

function genericTrigger64::onEnterTrigger(%db, %trig, %obj) { genericTrigger128::onEnterTrigger(%db, %trig, %obj); }
function genericTrigger64::onLeaveTrigger(%db, %trig, %obj) { genericTrigger128::onLeaveTrigger(%db, %trig, %obj); }
function genericTrigger64::onTickTrigger(%db, %trig, %obj)  { genericTrigger128::onTickTrigger(%db, %trig, %obj);  }


datablock TriggerData(customTrigger128) { tickPeriodMS = 128; };
datablock TriggerData(customTrigger64) { tickPeriodMS = 64; };

function customTrigger128::onEnterTrigger(%db, %trig, %obj)
{
	Parent::onEnterTrigger(%db, %trig, %obj);

	if(isFunction(%trig.enterCallback))
		call(%trig.enterCallback, %trig, %obj);
	
	if(!isEventPending(%trig.triggerLoop))
		%trig.triggerLoop = %db.schedule(%db.tickPeriodMS, onTickCheck, %trig);
}

function customTrigger128::onLeaveTrigger(%db, %trig, %obj)
{
	Parent::onLeaveTrigger(%db, %trig, %obj);
	
	if(%trig.getNumObjects() <= 0)
		cancel(%trig.triggerLoop);

	if(isFunction(%trig.leaveCallback))
		call(%trig.leaveCallback, %trig, %obj);
}

function customTrigger128::onTickCheck(%db, %trig)
{
	cancel(%trig.triggerLoop);

	if(isFunction(%trig.tickCallback))
	{
		%cts = %trig.getNumObjects();
		
		for(%i = 0; %i < %cts; %i++)
			call(%trig.tickCallback, %trig, %trig.getObject(%i));
	}

	%trig.triggerLoop = %db.schedule(%db.tickPeriodMS, onTickCheck, %trig);
}

function customTrigger64::onEnterTrigger(%db, %trig, %obj) { customTrigger128::onEnterTrigger(%db, %trig, %obj); }
function customTrigger64::onLeaveTrigger(%db, %trig, %obj) { customTrigger128::onLeaveTrigger(%db, %trig, %obj); }
function customTrigger64::onTickCheck(%db, %trig)          { customTrigger128::onTickCheck(%db, %trig);          }

datablock TriggerData(customTickTrigger128) { tickPeriodMS = 128; };
datablock TriggerData(customTickTrigger64) { tickPeriodMS = 64; };

function customTickTrigger128::onEnterTrigger(%db, %trig, %obj) { customTrigger128::onEnterTrigger(%db, %trig, %obj); }
function customTickTrigger128::onLeaveTrigger(%db, %trig, %obj) { customTrigger128::onLeaveTrigger(%db, %trig, %obj); }

function customTickTrigger128::onTickCheck(%db, %trig)
{
	cancel(%trig.triggerLoop);

	if(isFunction(%trig.tickCallback) && %trig.getNumObjects() > 0)
		call(%trig.tickCallback, %trig);

	%trig.triggerLoop = %db.schedule(%db.tickPeriodMS, onTickCheck, %trig);
}

function customTickTrigger64::onEnterTrigger(%db, %trig, %obj) { customTickTrigger128::onEnterTrigger(%db, %trig, %obj); }
function customTickTrigger64::onLeaveTrigger(%db, %trig, %obj) { customTickTrigger128::onLeaveTrigger(%db, %trig, %obj); }
function customTickTrigger64::onTickCheck(%db, %trig)          { customTickTrigger128::onTickCheck(%db, %trig);          }