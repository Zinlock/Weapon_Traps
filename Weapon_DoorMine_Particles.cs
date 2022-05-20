// keep debris2

datablock ParticleData(mine_doorExplosionHazeParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = 0;
	windCoefficient		= 0;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.0;
	lifetimeMS           = 900;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
   	colors[0]     = "0.7 0.7 0.9 0.8";
   	colors[1]     = "0.4 0.4 0.7 0.1";
   	colors[2]     = "0.1 0.1 0.4 0.0";
	times[0]	= 0.0;
	times[1]	= 0.3;
  times[2]	= 1.0;
		
	sizes[0]      = 3.0;
	sizes[1]      = 3.85;
	sizes[2]      = 4.35;

	useInvAlpha = false;
};
datablock ParticleEmitterData(mine_doorExplosionHazeEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 9;
   velocityVariance = 4.0;
   ejectionOffset   = 0.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "mine_doorExplosionHazeParticle";

   useEmitterColors = true;
};

datablock ParticleData(mine_doorExplosionCloudParticle)
{
	dragCoefficient		= 0.5;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.9;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 900;
	lifetimeVarianceMS	= 00;
	spinSpeed		= 0.0;
	spinRandomMin		= -90.0;
	spinRandomMax		= 90.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/star1";
	//animTexName		= "~/data/particles/cloud";

	// Interpolation variables
   colors[0]     = "0.1 0.3 1.0 0.8";
   colors[1]     = "0.1 0.1 0.1 0.0";
	sizes[0]      = 2;
	sizes[1]      = 0.5;
};

datablock ParticleEmitterData(mine_doorExplosionCloudEmitter)
{
   ejectionPeriodMS = 4;
   periodVarianceMS = 0;
   ejectionVelocity = 20;
   velocityVariance = 4.0;
   ejectionOffset   = 1.0;
   thetaMin         = 0;
   thetaMax         = 45;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "mine_doorExplosionCloudParticle";
};