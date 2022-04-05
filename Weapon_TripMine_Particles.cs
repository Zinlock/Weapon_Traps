datablock ParticleData(mine_tripExplosionHazeParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = 0;
	windCoefficient		= 0;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.0;
	lifetimeMS           = 1900;
	lifetimeVarianceMS   = 500;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]     = "1 1 1 0.1";
	colors[1]     = "0.9 0.5 0.0 0.9";
	colors[2]     = "0.1 0.05 0.025 0.1";
	colors[3]     = "0.1 0.05 0.025 0.0";

	sizes[0]	= 4.0;
	sizes[1]	= 3.3;
	sizes[2]	= 5.5;
	sizes[3]	= 4.5;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(mine_tripExplosionHazeEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 13;
   velocityVariance = 9.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 45;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "mine_tripExplosionHazeParticle";
};

datablock ParticleData(mine_tripExplosionCloudParticle)
{
	dragCoefficient		= 0.3;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1200;
	lifetimeVarianceMS	= 00;
	spinSpeed		= 5.0;
	spinRandomMin		= -5.0;
	spinRandomMax		= 5.0;
	useInvAlpha		= false;
	animateTexture		= false;

	textureName		= "base/data/particles/cloud";
	
	colors[0]     = "0.1 0.1 0.1 0.9";
	colors[1]     = "0.0 0.0 0.0 0.0";
	sizes[0]      = 2;
	sizes[1]      = 1;
};

datablock ParticleEmitterData(mine_tripExplosionCloudEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   lifeTimeMS	   = 90;
   ejectionVelocity = 7;
   velocityVariance = 1.0;
   ejectionOffset   = 1.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "mine_tripExplosionCloudParticle";
};

datablock ParticleData(mine_tripExplosionDebrisParticle)
{
	dragCoefficient      = 1.78;
	gravityCoefficient   = 0.03;
	windCoefficient		= 0.8;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.0;
	lifetimeMS           = 3500;
	lifetimeVarianceMS   = 800;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	colors[0]     = "0.4 0.4 0.4 0.4";
	colors[1]     = "0.1 0.1 0.1 0.2";
	colors[2]     = "0.05 0.05 0.05 0.1";
	colors[3]     = "0.05 0.05 0.05 0.0";

	sizes[0]	= 4.0;
	sizes[1]	= 3.3;
	sizes[2]	= 5.5;
	sizes[3]	= 7.5;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1.0;

	useInvAlpha = true;
};
datablock ParticleEmitterData(mine_tripExplosionDebrisEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 12.5;
	velocityVariance = 12.5;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 30;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "mine_tripExplosionDebrisParticle";
};

datablock ParticleData(mine_tripExplosionDebris2Particle)
{
	dragCoefficient		= 1.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.4;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 2000;
	lifetimeVarianceMS	= 1990;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/dot";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]     = "1 0.5 0.0 1";
	colors[1]     = "0.9 0.5 0.0 0.9";
	colors[2]     = "1 1 1 0.0";

	sizes[0]	= 0.3;
	sizes[1]	= 0.3;
	sizes[2]	= 0.3;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(mine_tripExplosionDebris2Emitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS       = 140;
	ejectionVelocity = 12;
	velocityVariance = 6.0;
	ejectionOffset   = 1.0;
	thetaMin         = 0;
	thetaMax         = 40;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "mine_tripExplosionDebris2Particle";
};

datablock ParticleData(mine_tripExplosionConeParticle)
{
	dragCoefficient		= 1.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.9;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 2800;
	lifetimeVarianceMS	= 600;
	spinSpeed		= 0.0;
	spinRandomMin		= -35.0;
	spinRandomMax		= 35.0;
	useInvAlpha		= true;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/chunk";
	//animTexName		= "~/data/particles/cloud";

	// Interpolation variables
	colors[0]     = "0.2 0.2 0.2 1.0";
	colors[1]     = "0.0 0.0 0.0 0.2";
	sizes[0]      = 0.9;
	sizes[1]      = 0.3;
};

datablock ParticleEmitterData(mine_tripExplosionConeEmitter)
{
	ejectionPeriodMS = 6;
	periodVarianceMS = 3;
	ejectionVelocity = 11;
	velocityVariance = 9.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 90;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "mine_tripExplosionConeParticle";
};