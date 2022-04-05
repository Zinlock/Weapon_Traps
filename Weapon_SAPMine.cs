datablock StaticShapeData(mine_stealthChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/mineAntiPersonnel.dts";
	health = 30;
};

datablock ItemData(mine_stealthItem : mine_impactItem)
{
	shapeFile = "./dts/mineAntiPersonnel.dts";

	uiName = "[T] Stealth Mine";
	iconName = "./ico/IMPACT";

	doColorShift = true;
	colorShiftColor = "0.15 0.15 0.15 1.0";

	image = mine_stealthImage;
};

datablock ShapeBaseImageData(mine_stealthImage : mine_impactImage)
{
	shapeFile = "./dts/mineAntiPersonnelImage.dts";

	item = mine_stealthItem;

	mineShapeData = mine_stealthChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 100;
	mineDeploySound = mine_deploySound;
	mineDeployDistance = 5;

	doColorShift = mine_stealthItem.doColorShift;
	colorShiftColor = mine_stealthItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;
};

function mine_stealthImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_stealthImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_stealthImage::onChargeStart(%this, %obj, %slot) { }

function mine_stealthImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_stealthChargeShape::onAdd(%data, %obj)
{
	mine_impactChargeShape::onAdd(%data, %obj);

	%obj.startFade(1,0,1);
	%s = %obj.schedule(%data.deployTime, setNodeColor, "ALL", vectorScale(groundPlane.color, 1/255) SPC "0.07");

	initContainerRadiusSearch(%obj.getPosition(), 0.1, $TypeMasks::FxBrickObjectType);
	if(isObject(%brk = ContainerSearchNext()))
	{
		cancel(%s);
		%obj.schedule(%data.deployTime, setNodeColor, "ALL", getWords(getColorIdTable(%brk.getColorId()), 0, 2) SPC "0.07");
	}
}

function mine_stealthChargeShape::onRemove(%data, %obj) { mine_impactChargeShape::onRemove(%data, %obj); }