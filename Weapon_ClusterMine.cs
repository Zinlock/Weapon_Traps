datablock StaticShapeData(mine_ClusterChargeShape : mine_impactChargeShape)
{
	shapeFile = "./dts/mineCluster.dts";
	health = 45;

	triggerSound = mine_triggerProxySound;
	triggerProjectile = grenade_clusterProjectile;
	triggerDelay = 750;
	triggerDistance = 6;
	triggerSize = "12 12 12";
};

datablock ItemData(mine_ClusterItem : mine_impactItem)
{
	shapeFile = "./dts/mineCluster.dts";

	uiName = "[T] Cluster Mine";
	iconName = "./ico/CLUSTER";

	doColorShift = true;
	colorShiftColor = "1.0 1.0 1.0 1.0";

	image = mine_ClusterImage;
};

datablock ShapeBaseImageData(mine_ClusterImage : mine_impactImage)
{
	shapeFile = "./dts/mineClusterImage.dts";

	item = mine_ClusterItem;

	mineShapeData = mine_ClusterChargeShape;
	mineCanRecover = true;
	
	mineMinSlope = 0;
	mineMaxSlope = 180;
	mineDeploySound = mine_deploySound;
	mineDeployDistance = 5;

	doColorShift = mine_ClusterItem.doColorShift;
	colorShiftColor = mine_ClusterItem.colorShiftColor;

	weaponUseCount = 1;
	weaponReserveMax = 2;
};

function mine_ClusterImage::onReady(%this, %obj, %slot) { mine_impactImage::onReady(%this, %obj, %slot); }

function mine_ClusterImage::onChargeStop(%this, %obj, %slot) { %this.onFire(%obj, %slot); }

function mine_ClusterImage::onChargeStart(%this, %obj, %slot) { }

function mine_ClusterImage::onFire(%this, %obj, %slot) { mine_impactImage::onFire(%this, %obj, %slot); }

function mine_ClusterChargeShape::onAdd(%data, %obj) { mine_impactChargeShape::onAdd(%data, %obj); }

function mine_ClusterChargeShape::onRemove(%data, %obj) { mine_impactChargeShape::onRemove(%data, %obj); }