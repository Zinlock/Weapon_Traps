datablock ParticleData(trap_energyFXAParticle)
{
	dragCoefficient      = 0;
	gravityCoefficient   = 0;
	windCoefficient		= 0;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.0;
	lifetimeMS           = 2000;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/thinRing";

	spinSpeed			= 0.0;
	spinRandomMin	= 0.0;
	spinRandomMax	= 0.0;

	colors[0]     = "0.0 0.5 0.9 0.0";
	colors[1]     = "0.0 0.26 0.7 0.1";
	colors[2]     = "0.0 0.1 0.4 0.0";

	times[0]	= 0.0;
	times[1]	= 0.3;
  times[2]	= 1.0;
		
	sizes[0]      = 0.15;
	sizes[1]      = 2.0;
	sizes[2]      = 5.25;

	useInvAlpha = false;
};

datablock ParticleEmitterData(trap_energyFXAEmitter)
{
	ejectionPeriodMS = 50;
	periodVarianceMS = 0;
	ejectionVelocity = 0.07;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 1;
	phiReferenceVel  = 360;
	phiVariance      = 0;

	overrideAdvance = false;

	orientParticles = false;
	orientOnVelocity = false;

	particles = "trap_energyFXAParticle";

	useEmitterColors = true;
};

datablock ParticleData(trap_energyFXBParticle)
{
	dragCoefficient      = 0;
	gravityCoefficient   = 0;
	windCoefficient		= 0;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.5;
	lifetimeMS           = 2000;
	lifetimeVarianceMS   = 0;
	textureName          = "./dts/bolt";

	spinSpeed			= 0.0;
	spinRandomMin	= 0.0;
	spinRandomMax = 0.0;

	colors[0]     = "0.0 0.5 1.0 0.8";
	colors[1]     = "0.0 0.26 0.9 0.5";
	colors[2]     = "0.0 0.1 0.8 0.0";

	times[0]	= 0.0;
	times[1]	= 0.3;
  times[2]	= 1.0;
	
	sizes[0]      = 0.5;
	sizes[1]      = 0.4;
	sizes[2]      = 0.3;

	useInvAlpha = false;
};

datablock ParticleEmitterData(trap_energyFXBEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = -1.0;
	velocityVariance = 0.0;
	ejectionOffset   = 6.0;
	thetaMin         = 0;//90;
	thetaMax         = 180;//90;
	phiReferenceVel  = 360;
	phiVariance      = 0;

	orientParticles = true;
	orientOnVelocity = true;

	overrideAdvance = false;

	particles = "trap_energyFXBParticle";
	
	useEmitterColors = true;
};