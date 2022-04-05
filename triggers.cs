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