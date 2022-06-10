datablock ParticleData(trap_healthFXAParticle)
{
	dragCoefficient      = 0;
	gravityCoefficient   = 0;
	windCoefficient		= 0;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.0;
	lifetimeMS           = 3000;
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

datablock ParticleEmitterData(trap_healthFXAEmitter)
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

	particles = "trap_healthFXAParticle";

	useEmitterColors = true;
};

datablock ParticleData(trap_healthFXBParticle)
{
	dragCoefficient      = 0;
	gravityCoefficient   = 0;
	windCoefficient		= 0;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.0;
	lifetimeMS           = 3500;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/dot";

	spinSpeed			= 0.0;
	spinRandomMin	= 0.0;
	spinRandomMax = 0.0;

	colors[0]     = "0.0 0.5 0.9 0.5";
	colors[1]     = "0.0 0.26 0.7 0.2";
	colors[2]     = "0.0 0.1 0.4 0.0";

	times[0]	= 0.0;
	times[1]	= 0.3;
  times[2]	= 1.0;
	
	sizes[0]      = 0.05;
	sizes[1]      = 0.2;
	sizes[2]      = 0.15;

	useInvAlpha = false;
};

datablock ParticleEmitterData(trap_healthFXBEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0.0;
	ejectionOffset   = 6.0;
	thetaMin         = 0;//90;
	thetaMax         = 180;//90;
	phiReferenceVel  = 360;
	phiVariance      = 0;

	overrideAdvance = false;

	particles = "trap_healthFXBParticle";
	
	useEmitterColors = true;
};