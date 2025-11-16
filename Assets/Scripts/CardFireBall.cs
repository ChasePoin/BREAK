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
        ball.AddComponent<ParticleSystem>();
        ParticleSystem instantiatedParticles = Instantiate(flames, ball.transform.position, Quaternion.identity);
        instantiatedParticles.transform.parent = transform;
        instantiatedParticles.Play();
    }
}
