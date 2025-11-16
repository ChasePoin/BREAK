using UnityEngine;

public class CardFireBall : Card
{
    [SerializeField]
    private ParticleSystem flames;
    public override void UseCard(Ball ballToApplyTo = null, PlayerController playerToApplyTo = null)
    {
        ballToApplyTo.catchable = false;
        ApplyFlames(ballToApplyTo);
    }

    private void ApplyFlames(Ball ballToApplyTo)
    {
        GameObject ball = ballToApplyTo.gameObject;

        // Spawn a particle system and attach it to the ball
        ParticleSystem instantiatedParticles = Instantiate(flames, ball.transform);
        instantiatedParticles.transform.localPosition = Vector3.zero;

        instantiatedParticles.Play();
}
}
